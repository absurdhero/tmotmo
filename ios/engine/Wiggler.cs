using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class Wiggler : Repeater {
	protected Dictionary<Sprite, Transform> centerPivots;
    Dictionary<Transform, Vector3> initialScales;
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
		initialScales = new Dictionary<Transform, Vector3>();
		foreach(var sprite in centerPivots.Keys) {
			initialScales.Add(centerPivots[sprite], centerPivots[sprite].localScale);
		}

		doWiggle = false;
		totalRotation = 0f;
	}
	
	public override void OnTick() {
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
                centerPivots[sprite].rotateLocal(Vector3.Backward, -totalRotation);
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
		foreach(var sprite in centerPivots.Keys) {
            centerPivots[sprite].localScale = initialScales[centerPivots[sprite]] * (1f + tick / (float) zoomTicks / 24f);
		}
	}
	
	virtual protected void wiggle() {
		float angle = -pingPong(currentTick / MathHelper.Pi / 2f - zoomTicks / 32f, 1 / 32f);
		totalRotation += angle;
		foreach(var sprite in centerPivots.Keys) {
            centerPivots[sprite].rotateLocal(Vector3.Backward, angle);
        }
	}

    static public float pingPong(float t, float length) {
        float p = t % (length * 2);
        if (p > length) {
            return length - (p - length);
        } else {
            return p;
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
        foreach(var sprite in centerPivots.Keys) {
            centerPivots[sprite].rotateLocal(Vector3.Backward, angle);
        }
	}
}
