using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TouchInput;

public class TitleScene : Scene {
	public Sprite title;
	public Sprite subtitle;
	public Sprite news;
	public Sprite buyMusic;
	public Sprite startButton;
	public Sprite background;

	private Cycler cycle_title, cycle_start;
	
	public TitleScene(SceneManager manager, SpriteRenderer spriteRenderer) : base(manager, spriteRenderer) {
	}

	public override void Setup(float startTime) {
		background = spriteRenderer.add(new FullScreenQuad(), this.GetType(), new[] {"bg"});
        title = spriteRenderer.add(new Sprite(), this.GetType(), new[] {"tmo1", "tmo2", "tmo3", "tmo4", "tmo5", "tmo6"});
        subtitle = spriteRenderer.add(new Sprite(), this.GetType(), new[] {"p1", "p2", "p3", "p4", "p5", "p6"});
        news = spriteRenderer.add(new Sprite(), this.GetType(), new[] {"news1", "news2"});
        buyMusic = spriteRenderer.add(new Sprite(), this.GetType(), new[] {"itunes1", "itunes2"});
        startButton = spriteRenderer.add(new Sprite(), this.GetType(), new[] {"tap1", "tap2", "tap3"});
		
		Camera camera = Camera.main;

		System.Console.WriteLine ("center " + title.Center ());
		var layoutpos = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.65f, 0));
		System.Console.WriteLine ("layoutpos " + layoutpos);
		layoutpos -= new Vector3(title.Center(), 0);
		title.worldPosition = new Vector3(layoutpos.X, layoutpos.Y, title.worldPosition.Z);

		// Anchor the subtitle an absolute distance from wherever the title ended up
		subtitle.worldPosition = title.worldPosition + new Vector3(title.Center() , 0) + new Vector3(16f, 24f, -1f);
		
		// add blinking start text below title but don't display it yet
		startButton.setCenterToViewportCoord(0.5f, 0.4f);
        startButton.visible(false);
		
		// place buttons in the bottom corners
		news.screenPosition = new Vector3(4, camera.pixelHeight - news.height - 4, 0);
		buyMusic.screenPosition = new Vector3(camera.pixelWidth - buyMusic.PixelWidth() - 4, camera.pixelHeight - buyMusic.height - 4, 0);
		
		// animate title
		cycle_title = new Cycler(0.4f, 5);
		cycle_title.AddSprite(title);
		cycle_title.AddSprite(subtitle);
	}

	public override void Update () {
		var touch = new TouchSensor(input);
		
		if (touch.insideSprite(Camera.main, buyMusic)) {
			Application.OpenURL("http://itunes.apple.com/us/album/same-not-same-ep/id533347009");
		}
		else if (touch.insideSprite(Camera.main, news)) {
			Application.OpenURL("http://themakingofthemakingof.com");
		}
		else if (touch.hasTaps()) {
			endScene();
		}

		if (cycle_title.Complete()) {
			animateStartButton();
		}

		cycle_title.Update(Time.time);
	}

	public override void Destroy() {
        spriteRenderer.remove(title);
        spriteRenderer.remove(subtitle);
        spriteRenderer.remove(news);
        spriteRenderer.remove(buyMusic);
        spriteRenderer.remove(startButton);
        spriteRenderer.remove(background);
	}

	private void animateStartButton() {
		if (cycle_start == null) {
			cycle_start = new Cycler(0.4f, 2);
			cycle_start.AddSprite(startButton);
			startButton.visible(true);
		}
		cycle_start.Update(Time.time);
	}
}

