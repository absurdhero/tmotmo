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
	
	private UnityInput input;

	public TitleScene(SceneManager manager, ContentManager content, GraphicsDeviceManager graphics) : base(manager, content, graphics) {
		this.input = new UnityInput();
	}

	public override void Setup(float startTime) {
		background = new FullScreenQuad(graphics, content, this, new[] {"bg"});
		title = new Sprite(graphics, content, this, new[] {"tmo1", "tmo2", "tmo3", "tmo4", "tmo5", "tmo6"});
		subtitle = new Sprite(graphics, content, this, new[] {"p1", "p2", "p3", "p4", "p5", "p6"});
		news = new Sprite(graphics, content, this, new[] {"news1", "news2"});
		buyMusic = new Sprite(graphics, content, this, new[] {"itunes1", "itunes2"});
		startButton = new Sprite(graphics, content, this, new[] {"tap1", "tap2", "tap3"});
		
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
		
//		// place buttons in the bottom corners
		news.screenPosition = new Vector3(4, camera.pixelHeight - news.height - 4, 0);
		buyMusic.screenPosition = new Vector3(camera.pixelWidth - buyMusic.PixelWidth() - 4, camera.pixelHeight - buyMusic.height - 4, 0);
		
		// animate title
		cycle_title = new Cycler(0.4f, 5);
		cycle_title.AddSprite(title);
		cycle_title.AddSprite(subtitle);
	}

	public override void Draw() {
		background.Draw();
		title.Draw();
		subtitle.Draw();
		news.Draw();
		buyMusic.Draw();
		startButton.Draw();
	}

	public override void Update () {
		var touch = new TouchSensor(input);
		
		if (touch.insideSprite(Camera.main, buyMusic)) {
//			Application.OpenURL("http://itunes.apple.com/us/album/same-not-same-ep/id533347009");
		}
		else if (touch.insideSprite(Camera.main, news)) {
//			Application.OpenURL("http://themakingofthemakingof.com");
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
		Sprite.Destroy(title);
		Sprite.Destroy(subtitle);
		Sprite.Destroy(news);
		Sprite.Destroy(buyMusic);
		Sprite.Destroy(startButton);
		Sprite.Destroy(background);
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

