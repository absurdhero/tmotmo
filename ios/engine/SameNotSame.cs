using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;


public class SameNotSame : Game
{

	GraphicsDeviceManager graphics;
	Camera camera;

    SceneManager sceneManager;
    Prompt prompt;
    MessageBox messageBox;

	public SameNotSame ()
	{
		graphics = new GraphicsDeviceManager (this);
		
		Content.RootDirectory = "Content";

		graphics.IsFullScreen = true;
	}

	protected override void Initialize ()
	{
		graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        // disable blurriness for drawing retro pixel art
        graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		camera = new Camera(graphics.GraphicsDevice.Viewport);
		Camera.main = camera;

		base.Initialize ();
	}

	protected override void LoadContent ()
	{
        var spriteRenderer = new SpriteRenderer(graphics, Content);
        prompt = new Prompt(spriteRenderer, Content);
        messageBox = new MessageBox(Content, spriteRenderer);
        var sounds = new Sounds();
        sceneManager = new SceneManager(new LoopTracker(sounds),
                                        new MessagePromptCoordinator(prompt, messageBox));
        var sceneFactory = new SceneFactory(sceneManager, spriteRenderer);

        // create the scene objects and preload their content
        sceneManager.LoadAndStart(sceneFactory);
    }

	protected override void Update (GameTime gameTime)
	{
        // set game time for this frame
		Time.setFromGameTime(gameTime);
        // set the touch state for this frame
        TouchInput.UnityInput.touchCollection = TouchPanel.GetState();

        sceneManager.Update();

        base.Update (gameTime);
	}

	protected override void Draw (GameTime gameTime)
	{
		// Clear the backbuffer
		graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

		// draw scene
        sceneManager.Draw();

		base.Draw (gameTime);
	}
}

