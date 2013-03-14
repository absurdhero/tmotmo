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

	SpriteBatch spriteBatch;

	//private Cycler cycle_title, cycle_start;
	
	//private UnityInput input;
	
//	public TitleScene(SceneManager manager) : base(manager) {
//		input = new UnityInput();
//	}
	
	public TitleScene(ContentManager content, SpriteBatch spriteBatch) {
		this.spriteBatch = spriteBatch;
		this.content = content;
	}

	public void Setup(float startTime) {
		//background = FullScreenQuad.create(this, "bg");
		title = new Sprite(spriteBatch, content, this, new[] {"tmo1", "tmo2", "tmo3", "tmo4", "tmo5", "tmo6"});
		subtitle = new Sprite(spriteBatch, content, this, new[] {"p1", "p2", "p3", "p4", "p5", "p6"});
		news = new Sprite(spriteBatch, content, this, new[] {"news1", "news2"});
		buyMusic = new Sprite(spriteBatch, content, this, new[] {"itunes1", "itunes2"});
		startButton = new Sprite(spriteBatch, content, this, new[] {"tap1", "tap2", "tap3"});
		
		Camera cam = Camera.main;

		var layoutpos = cam.ViewportToWorld2D(new Vector2(0.5f, 0.65f));
		System.Console.WriteLine ("screenEdgesToWorld " + cam.ScreenToWorld2D(Vector2.Zero) + ", " + cam.ScreenToWorld2D(Vector2.One));
		System.Console.WriteLine ("viewToworld(0.5f, 0.65f) => " + layoutpos);
		System.Console.WriteLine ("viewToScreen => " + cam.WorldToScreenPoint(new Vector3(layoutpos, Camera.NEAR)));
		layoutpos -= title.Center();
		System.Console.WriteLine ("minus center viewToworld(0.5f, 0.65f) => " + layoutpos);
		System.Console.WriteLine ("minus center viewToScreen(0.5f, 0.65f) => " + cam.WorldToScreenPoint(new Vector3(layoutpos, Camera.NEAR)));
		title.worldPosition = layoutpos;
//		// Anchor the subtitle an absolute distance from wherever the title ended up
//		subtitle.transform.position = title.transform.position + title.Center() + new Vector3(15f, -20f, -1f);
		
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

