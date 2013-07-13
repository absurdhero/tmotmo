using System;
using System.Collections.Generic;
using TouchInput;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

class SceneOne : Scene {
	public Sprite background;
	public Sprite same;
	public Sprite notSame;
	public Sprite circle;
	public Sprite triangle;
	
	Wiggler wiggler;
	TouchSensor sensor;

	Cycler notSameCycler;
	Cycler circleCycler;
	Cycler triangleCycler;

	const int triangleWaitTime = 4;

 	// animate both shapes at the same frequency
	const float shapeSpeed = 0.5f;

    public SceneOne(SceneManager manager, SpriteRenderer spriteRenderer) : base(manager, spriteRenderer) {
		timeLength = 8.0f;
	}

	public override void LoadAssets() {
        background = spriteRenderer.add(new FullScreenQuad(), "TitleScene/bg");
		background.visible(false);

        same = spriteRenderer.add(new Sprite(), this.GetType(), "same");
        notSame = spriteRenderer.add(new Sprite(), this.GetType(), "notsame", "notsame_caps", "notsame_g1", "notsame_g2");
        circle = spriteRenderer.add(new Sprite(), this.GetType(), "circle1", "circle2", "circle3", "circle4", "circle5");
        triangle = spriteRenderer.add(new Sprite(), this.GetType(), "triangle1", "triangle2", "triangle3");

		same.visible(false);
		notSame.visible(false);
		circle.visible(false);
		triangle.visible(false);
	}

	public override void Setup(float startTime) {
		background.visible(true);
		same.visible(true);
		notSame.visible(true);
		circle.visible(true);
		triangle.visible(true);

		same.setCenterToViewportCoord(0.35f, 0.66f);
		notSame.setCenterToViewportCoord(0.7f, 0.66f);
		circle.setCenterToViewportCoord(0.3f, 0.33f);
		triangle.setCenterToViewportCoord(0.7f, 0.33f);
		
		// hide the triangle to start
		triangle.visible(false);
		
		circleCycler = new Cycler(shapeSpeed, 0, startTime);
		circleCycler.AddSprite(circle);
		
		notSameCycler = new DelayedCycler(0.2f, 4, 1.2f, startTime);
		notSameCycler.AddSprite(notSame);

		wiggler = new Wiggler(startTime, timeLength, new[] {circle, triangle});

		sensor = new TouchSensor(input);
	}

	public override void Destroy() {
		spriteRenderer.remove(circle);
        spriteRenderer.remove(triangle);
        spriteRenderer.remove(same);
        spriteRenderer.remove(notSame);
        spriteRenderer.remove(background);
		wiggler.Destroy();
	}

	public override void Update () {
		float now = Time.time;
		wiggler.Update(now);
		notSameCycler.Update(now);

		if (solved) return;


		if (circle.belowFinger(sensor)
		    && triangle.belowFinger(sensor)
		    && triangleShowing()) {
			messagePromptCoordinator.clearTouch();
			messagePromptCoordinator.hintWhenTouched(gameObject => {
				Application.Vibrate();
				wiggler.wiggleNow(now);
				endScene();
			}, sensor, now, new Dictionary<Sprite, ActionResponsePair[]> {
					{circle,   new [] {new ActionResponsePair("stop shapes from changing", new [] {"OK"})}},
					{triangle, new [] {new ActionResponsePair("stop shapes from changing", new [] {"OK"})}},
			});
		} else {
			messagePromptCoordinator.hintWhenTouched(GameObject => {}, sensor, now,
				new Dictionary<Sprite, ActionResponsePair[]> {
					{circle,   new [] {new ActionResponsePair("stop circle from changing",   new[] {"Nope."})}},
					{triangle, new [] {new ActionResponsePair("stop triangle from changing", new[] {"Nope."})}},
				});
		}

		AnimateShapes(now);

		// if touched circle, draw its bright first frame
		if (sensor.changeInsideSprite(Camera.main, circle)) {
			circle.setFrame(0);
			circle.Animate();
		}

		// if touched triangle, ditto
		if (sensor.changeInsideSprite(Camera.main, triangle) && triangleShowing()) {
			triangle.setFrame(0);
			triangle.Animate();
		}
	}

	bool triangleShowing() {
		return triangleCycler != null;
	}

	void AnimateShapes(float time) {
		if (!triangleShowing() && circleCycler.Count() >= triangleWaitTime) {
			triangleCycler = new DelayedCycler(shapeSpeed, 6, 1f);
			triangle.visible(true);
			triangleCycler.AddSprite(triangle);
		}

		circleCycler.Update(time);

		if (triangleShowing()) {
			triangleCycler.Update(time);
		}
	}
}
