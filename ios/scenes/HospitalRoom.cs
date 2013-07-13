using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

public class HospitalRoom {
    SpriteRenderer spriteRenderer;

	Sprite room;
	
	int guyCenterOffset = 6;
	public Sprite guyLeft, guyRight;
	Vector3 guyLeftInitialPosition;
	Vector3 guyRightInitialPosition;
	public float guySplitDistance { get; private set; }
	public float guyCenterPoint {
		get {
			return camera.pixelWidth / 2.0f + (float) guyCenterOffset;
		}
	}

	public Sprite cover;
	
	Sprite zzz;
	ZzzAnimator zzzAnimator;
    Sprite footBoard;
    Sprite clipBoard;
    Sprite eyes;
	Camera camera;
    Sprite heartRate;
	Cycler heartRateCycler;

    Dictionary<Sprite, ActionResponsePair[]> interactions;

    public HospitalRoom(SpriteRenderer spriteRenderer, Camera camera) {
        this.spriteRenderer = spriteRenderer;
		this.camera = camera;
	}

	public void createBackground() {
        room = spriteRenderer.add(new FullScreenQuad(), this.GetType(), "hospital_bg");
	}

	private void DestroyIfNotNull(Sprite sprite) {
        if (sprite != null)
            spriteRenderer.remove(sprite);
	}

	public void Destroy() {
		DestroyIfNotNull(room);
		DestroyIfNotNull(guyLeft);
		DestroyIfNotNull(guyRight);
		DestroyIfNotNull(eyes);
		DestroyIfNotNull(cover);
		DestroyIfNotNull(footBoard);
		DestroyIfNotNull(clipBoard);
		DestroyIfNotNull(heartRate);
		
		heartRateCycler = null;
		removeZzz();
	}
	
	public void addPerson() {
        guyLeft = spriteRenderer.add(new Sprite(), this.GetType(), "guy1-fixed");
        guyRight = spriteRenderer.add(new Sprite(), this.GetType(), "guy2-fixed");

		guyLeft.setScreenPosition((int) camera.pixelWidth / 2 - guyLeft.PixelWidth() + guyCenterOffset,
									 (int) camera.pixelHeight / 2 - guyLeft.PixelHeight() / 2);
        guyLeft.setDepth(-2);
		guyRight.setScreenPosition((int) camera.pixelWidth / 2 + guyCenterOffset,
									  (int) camera.pixelHeight / 2 - guyRight.PixelHeight() / 2);
        guyRight.setDepth(-2);
		guyLeftInitialPosition = guyLeft.getScreenPosition();
		guyRightInitialPosition = guyRight.getScreenPosition();
		
        eyes = spriteRenderer.add(new Sprite(), this.GetType(), "eyes1", "eyes2");
		eyes.setScreenPosition(232.2f, 89f);
        eyes.setDepth(-3);
	}
	
	public void addZzz() {
        zzz = spriteRenderer.add(new Sprite(), this.GetType(), "z");
		zzz.setScreenPosition(320, 70);
        zzz.setDepth(-1);
		zzzAnimator = new ZzzAnimator(zzz);
	}
	
	public void removeZzz() {
		DestroyIfNotNull(zzz);
		zzzAnimator = null;
	}
	
	public void openEyes() {
		if (eyes.LastTexture()) {
            eyes.isVisible = false;
			return;
		}
		eyes.DrawNextFrame();
	}
	
	private bool eyesTotallyOpen {
        get { return !eyes.isVisible; }
	}
	
	public void addCover() {
		if (cover != null) {
            cover.isVisible = true;
			return;
		}
        cover = spriteRenderer.add(new Sprite(), this.GetType(), "cover-fixed");
		cover.setCenterToViewportCoord(0.515f, 0.34f);
        cover.setDepth(-4f);
	}
	
	public void removeCover() {
        cover.isVisible = false;
	}
	
	public void addFootboard() {
		addClipBoard();

        footBoard = spriteRenderer.add(new Sprite(), this.GetType(), "footrest-fixed");
        footBoard.setScreenPosition(169, 226);
        footBoard.setDepth(-5f);
	}
	
	private void addClipBoard() {
        clipBoard = spriteRenderer.add(new Sprite(), this.GetType(), "chart-fixed");
        clipBoard.setScreenPosition(220, 220);
        clipBoard.setDepth(-5f);
	}
	
	public void addHeartRate(float startTime) {
        heartRate = spriteRenderer.add(new Sprite(), this.GetType(), "heart1", "heart2", "heart3", "heart4", "heart5", "heart6", "heart7");
        heartRate.setScreenPosition(54, 110);
        heartRate.setDepth(-1);
		
		// Repeat 7 frame animation every 2 seconds
		// That's 30bpm which might mean he is dying!
		heartRateCycler = new Cycler(1f/7f*2f, 0, startTime);
		heartRateCycler.AddSprite(heartRate);
	}
	
	public void doubleHeartRate(float startTime) {
		heartRateCycler = new Cycler(1f/7f, 0, startTime);
		heartRateCycler.AddSprite(heartRate);
	}
	
	
	public Rectangle guyBoundsPlus(int inflation) {
		var leftRect = guyLeft.ScreenRect();
		var rightRect = guyRight.ScreenRect();
        var rect = new Rectangle(leftRect.X - inflation,
						leftRect.Y - inflation,
                        leftRect.Width + rightRect.Width + inflation * 2,
                                 leftRect.Height + rightRect.Height + inflation * 2);
	
		return rect;
	}
	
	public bool guyContains(Vector2 position) {
		return guyLeft.Contains(Camera.main, position)
			|| guyRight.Contains(Camera.main, position);
	}
	
	public void separateHalves(float distance) {
		guySplitDistance = distance;
	}

	private void setGuySplit() {
		guyLeft.setScreenPosition((int) (guyLeftInitialPosition.X - guySplitDistance), (int) guyLeftInitialPosition.Y);
		guyRight.setScreenPosition((int) (guyRightInitialPosition.X + guySplitDistance), (int) guyRightInitialPosition.Y);
	}
	
	public void addSplitLine() {
		separateHalves(1);
	}

	public void Update() {		
		if(heartRateCycler != null) {
			heartRateCycler.Update(Time.time);
		}
		
		setGuySplit();
		
		if (zzzAnimator != null && !eyesTotallyOpen) {
			zzzAnimator.Update(Time.time);
		}
	}

	public void hintWhenTouched(Action<Sprite> onCompleted, MessagePromptCoordinator messagePromptCoordinator, TouchSensor touch) {
		if (interactions == null) {
			interactions = new Dictionary<Sprite, ActionResponsePair[]> {
				{clipBoard, new [] {new ActionResponsePair("look at chart", new [] {"even the doctors don't understand the test results"})}},
				{zzz,       new [] {new ActionResponsePair("catch z", new [] {"that's not going to wake him up"})}},
				{heartRate, new [] {new ActionResponsePair("look at monitor", new []{"things are stable, for now"})}},
				{cover,     new [] {new ActionResponsePair("prod him", new[] {"He doesn't want to wake up"}),
						            new ActionResponsePair("prod him until he wakes up", new [] {"OK"}),
							        new ActionResponsePair("expose him to the cold",
										new [] {
										"you remove the blankets, security and otherwise.",
										"there are now two distinct halves.",
										"are they the same person?"}),
					}},
			};
		}
		messagePromptCoordinator.hintWhenTouched(onCompleted, touch, Time.time, interactions);
	}
	
	public bool touchedBed(TouchSensor touch) {
		return touch.insideSprite(Camera.main, cover, new[] {TouchInput.TouchPhase.Began});
	}

	class ZzzAnimator : Repeater {
		Sprite zzz;
		Vector3 initialPosition;
		int frame = 0;
		
		const int totalFrames = 4;
		
		
		public ZzzAnimator(Sprite zzz) : base(0.5f) {
			this.zzz = zzz;
			initialPosition = zzz.worldPosition;
		}
		
		public override void OnTick() {
			frame = (frame + 1) % totalFrames;
            zzz.worldPosition = initialPosition + new Vector3(frame * 2, -frame * 2, 0);
		}
		
		public void Reset() {
            zzz.worldPosition = initialPosition;
		}
	}
}
