using System.Collections.Generic;
using Microsoft.Xna.Framework;

class Wiggler : Repeater {
	protected Dictionary<Sprite, Transform> centerPivots;
    Dictionary<Sprite, Vector3> initialScales;
	float sceneStart;
	float sceneLength;
	protected float totalRotation;
	
	bool doWiggle;
	
	protected const int zoomTicks = 5;
	protected const int wiggleTicks = 15;

    private static Dictionary<Sprite, Transform> pivotsFromSprite(Sprite[] sprites) {
        var pivots = new Dictionary<Sprite, Transform>();

        foreach (var sprite in sprites) {
            // sets the transform matrix to be centered
            pivots.Add(sprite, sprite.createPivotOnCenter());
        }
		return pivots;
	}

	public Wiggler(float startTime, float sceneLength, Sprite sprite) :
	this(startTime, sceneLength, new[] {sprite}) {}

	public Wiggler(float startTime, float sceneLength, Sprite[] sprites) :
	this(startTime, sceneLength, pivotsFromSprite(sprites)) {}

    public Wiggler(float startTime, float sceneLength, Dictionary<Sprite, Transform> centerPivots) : base(0.05f, 0, startTime) {
		sceneStart = startTime;
		this.sceneLength = sceneLength;
		this.centerPivots = centerPivots;

		// preserve the scale of each pivot
		initialScales = new Dictionary<Sprite, Vector3>();
		foreach(var sprite in centerPivots.Keys) {
            Vector3 scale;
            Quaternion rotation;
            Vector3 translation;
            centerPivots[sprite].worldMatrix.Decompose(out scale, out rotation, out translation);
			initialScales.Add(sprite, scale);
		}

		doWiggle = false;
		totalRotation = 0f;
	}
	
	public override void OnTick() {
        Vector3 backward = Vector3.Backward;

		float time = Time.time;
		if (time - sceneStart >= sceneLength && (time - sceneStart) % sceneLength <= interval) {
			wiggleNow(time);
		}
		
		if (!doWiggle) return;
		
		if (currentTick < zoomTicks) {
			zoomIn();
		} else if (currentTick < zoomTicks + wiggleTicks) {
			wiggle();
		} else if (currentTick < zoomTicks + wiggleTicks + zoomTicks) {
			foreach(var sprite in centerPivots.Keys) {
                Vector3 scale;
                Quaternion rotation;
                Vector3 translation;

                var pivot = centerPivots[sprite];
                pivot.localMatrix.Decompose(out scale, out rotation, out translation);
                pivot.localRotate(backward, -totalRotation);
			}
			totalRotation = 0f;
			zoomOut();
		} else {
			doWiggle = false;
		}
	}
	
	public void wiggleNow(float wiggleTime) {
		if (doWiggle) return; // already wiggling
		Reset(wiggleTime);
		doWiggle = true;
	}
	
	public void Destroy() {
		foreach(var sprite in centerPivots.Keys) {
            // fix up the scale
            centerPivots[sprite].localScale(1);
            // remove the wiggly parent transform
            sprite.transform.removeImmediateParent();
		}
	}
	
	private void zoomIn() {
		zoomFor(currentTick);
	}

	private void zoomOut() {
		int zoomOutTicks = currentTick - zoomTicks - wiggleTicks;
		zoomFor(zoomTicks - zoomOutTicks);
	}
	
	private void zoomFor(int tick) {
        return;
		foreach(var sprite in centerPivots.Keys) {
            centerPivots[sprite].localScale(1f + tick / (float) zoomTicks / 24f);
		}
	}
	
	virtual protected void wiggle() {
		float angle = -pingPong(currentTick - zoomTicks / 64f * MathHelper.Pi, MathHelper.Pi / 16f);
		totalRotation += angle;
        Matrix rotation = Matrix.CreateRotationZ(totalRotation);
		foreach(var sprite in centerPivots.Keys) {
            centerPivots[sprite].localMatrix = rotation * centerPivots[sprite].localMatrix;
		}
	}

    protected float pingPong(float t, float length) {
        float p = t % (length * 2);
        if (p > length) {
            return t - (p - t);
        } else {
            return p - t;
        }
    }
}

class ReverseWiggler : Wiggler {
	public ReverseWiggler(float startTime, float sceneLength, Sprite sprite) :
	base(startTime, sceneLength, sprite) {}

	public ReverseWiggler(float startTime, float sceneLength, Sprite[] sprites) :
	base(startTime, sceneLength, sprites) {}

    public ReverseWiggler(float startTime, float sceneLength, Dictionary<Sprite, Transform> centerPivots) :
	base(startTime, sceneLength, centerPivots) {}
   
	override protected void wiggle() {
		float angle = pingPong(currentTick - zoomTicks / 64f * MathHelper.Pi, MathHelper.Pi / 16f);
		totalRotation += angle;
        Matrix rotation = Matrix.CreateRotationZ(totalRotation);
        foreach(var sprite in centerPivots.Keys) {
            centerPivots[sprite].localMatrix = rotation * centerPivots[sprite].localMatrix;
        }
	}
}
