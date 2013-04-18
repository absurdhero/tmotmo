using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class TitleScene {
	public Sprite title;
	public Sprite subtitle;
	public Sprite news;
	public Sprite buyMusic;
	public Sprite startButton;
	public Sprite background;

	ContentManager content;
	GraphicsDeviceManager graphics;

	//private Cycler cycle_title, cycle_start;
	
	//private UnityInput input;
	
//	public TitleScene(SceneManager manager) : base(manager) {
//		input = new UnityInput();
//	}
	
	public TitleScene(ContentManager content, GraphicsDeviceManager graphics) {
		this.graphics = graphics;
		this.content = content;
	}

	public void Setup(float startTime) {
		//background = FullScreenQuad.create(this, "bg");
		title = new Sprite(graphics, content, this, new[] {"tmo1", "tmo2", "tmo3", "tmo4", "tmo5", "tmo6"});
		subtitle = new Sprite(graphics, content, this, new[] {"p1", "p2", "p3", "p4", "p5", "p6"});
		news = new Sprite(graphics, content, this, new[] {"news1", "news2"});
		buyMusic = new Sprite(graphics, content, this, new[] {"itunes1", "itunes2"});
		startButton = new Sprite(graphics, content, this, new[] {"tap1", "tap2", "tap3"});
		
		Camera cam = Camera.main;

		System.Console.WriteLine ("center " + title.Center ());
		var layoutpos = cam.ViewportToWorld2D(new Vector2(0.5f, 0.65f));
		System.Console.WriteLine ("layoutpos " + layoutpos);
		layoutpos -= title.Center();
		title.worldPosition = layoutpos;

//		// Anchor the subtitle an absolute distance from wherever the title ended up
//		subtitle.worldPosition = title.worldPosition + title.Center() + new Vector3(15f, -20f, -1f);
		
		// add blinking start text below title but don't display it yet
//		startButton.setCenterToViewportCoord(0.5f, 0.4f);
//		startButton.visible(false);
		
//		// place buttons in the bottom corners
//		news.setScreenPosition(4, 4);
//		buyMusic.setScreenPosition((int) cam.GetScreenWidth() - buyMusic.PixelWidth() - 4, 4);
		
		// animate title
//		cycle_title = new Cycler(0.4f, 5);
//		cycle_title.AddSprite(title);
//		cycle_title.AddSprite(subtitle);
	}

	public void Draw() {
		title.Draw ();
	}
//	public override void Update () {
//		var touch = new TouchSensor(input);
//		
//		if (touch.insideSprite(Camera.main, buyMusic)) {
//			Application.OpenURL("http://itunes.apple.com/us/album/same-not-same-ep/id533347009");
//		}
//		else if (touch.insideSprite(Camera.main, news)) {
//			Application.OpenURL("http://themakingofthemakingof.com");
//		}
//		else if (touch.hasTaps()) {
//			endScene();
//		}
//
//		if (cycle_title.Complete()) {
//			animateStartButton();
//		}
//
//		cycle_title.Update(Time.time);
//	}

	public void Destroy() {
		Sprite.Destroy(title);
//		Sprite.Destroy(subtitle);
//		Sprite.Destroy(news);
//		Sprite.Destroy(buyMusic);
//		Sprite.Destroy(startButton);
//		Sprite.Destroy(background);
	}

//	private void animateStartButton() {
//		if (cycle_start == null) {
//			cycle_start = new Cycler(0.4f, 2);
//			cycle_start.AddSprite(startButton);
//			startButton.visible(true);
//		}
//		cycle_start.Update(Time.time);
//	}
}

