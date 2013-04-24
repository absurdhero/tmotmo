using System.Collections.Generic;
using Microsoft.Xna.Framework;

class Wiggler : Repeater {
	protected Dictionary<Sprite, Matrix> centerPivots;
    Dictionary<Sprite, Vector3> initialScales;
	float sceneStart;
	float sceneLength;
	protected float totalRotation;
	
	bool doWiggle;
	
	protected const int zoomTicks = 5;
	protected const int wiggleTicks = 15;

    private static Dictionary<Sprite, Matrix> pivotsFromSprite(Sprite[] sprites) {
        var pivots = new Dictionary<Sprite, Matrix>();

        foreach (var sprite in sprites) {
            // sets the transform matrix on the sprite and copy of it
            pivots.Add(sprite, sprite.createPivotOnCenter());
        }
		return pivots;
	}

	public Wiggler(float startTime, float sceneLength, Sprite sprite) :
	this(startTime, sceneLength, new[] {sprite}) {}

	public Wiggler(float startTime, float sceneLength, Sprite[] sprites) :
	this(startTime, sceneLength, pivotsFromSprite(sprites)) {}

    public Wiggler(float startTime, float sceneLength, Dictionary<Sprite, Matrix> centerPivots) : base(0.05f, 0, startTime) {
		sceneStart = startTime;
		this.sceneLength = sceneLength;
		this.centerPivots = centerPivots;

		// preserve the scale of each pivot
		initialScales = new Dictionary<Sprite, Vector3>();
		foreach(var sprite in centerPivots.Keys) {
            Vector3 scale;
            Quaternion rotation;
            Vector3 translation;
            centerPivots[sprite].Decompose(out scale, out rotation, out translation);
			initialScales.Add(sprite, scale);
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
                Vector3 scale;
                Quaternion rotation;
                Vector3 translation;
                centerPivots[sprite].Decompose(out scale, out rotation, out translation);
                
//				pivot.transform.Rotate(Vector3.back, -totalRotation, Space.Self);
                // XXX need to update the matrix on the sprite
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
		foreach(var pivot in centerPivots) {
			// GameObject.Destroy also destroys children so we must detach each Sprite from its pivot
//			pivot.transform.DetachChildren();
//			GameObject.Destroy(pivot);
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
		foreach(var sprite in centerPivots) {
//			centerPivots[sprite].transform.localScale = initialScales[sprite] * (1f + tick / (float) zoomTicks / 24f);
		}
	}
	
	virtual protected void wiggle() {
//		float angle = -Mathf.PingPong(currentTick - zoomTicks / 64f * Mathf.PI, Mathf.PI / 16f);
//		totalRotation += angle;
//		foreach(var pivot in centerPivots) {
//			pivot.transform.Rotate(Vector3.back, angle, Space.Self);
//		}
	}
}

class ReverseWiggler : Wiggler {
	public ReverseWiggler(float startTime, float sceneLength, Sprite sprite) :
	base(startTime, sceneLength, sprite) {}

	public ReverseWiggler(float startTime, float sceneLength, Sprite[] sprites) :
	base(startTime, sceneLength, sprites) {}

	public ReverseWiggler(float startTime, float sceneLength, Dictionary<Sprite, Matrix> centerPivots) :
	base(startTime, sceneLength, centerPivots) {}
   
	override protected void wiggle() {
//		float angle = Mathf.PingPong(currentTick - zoomTicks / 64f * Mathf.PI, Mathf.PI / 16f);
//		totalRotation += angle;
//		foreach(var pivot in centerPivots) {
//			pivot.transform.Rotate(Vector3.back, angle, Space.Self);
//		}
	}
}
