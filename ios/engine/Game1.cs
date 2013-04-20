#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

#endregion

/// <summary>
/// Default Project Template
/// </summary>
public class Game1 : Game
{

#region Fields
	GraphicsDeviceManager graphics;
	Camera camera;
	TitleScene titleScene;

    SceneManager sceneManager;
#endregion

#region Initialization

	public Game1 ()
	{

		graphics = new GraphicsDeviceManager (this);
		
		Content.RootDirectory = "Content";

		graphics.IsFullScreen = true;
	}

	/// <summary>
	/// Overridden from the base Game.Initialize. Once the GraphicsDevice is setup,
	/// we'll use the viewport to initialize some values.
	/// </summary>
	protected override void Initialize ()
	{
		graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
		camera = new Camera(graphics.GraphicsDevice.Viewport);
		Camera.main = camera;

		base.Initialize ();
	}


	/// <summary>
	/// Load your graphics content.
	/// </summary>
	protected override void LoadContent ()
	{
        sceneManager = new SceneManager(new LoopTracker(), new MessagePromptCoordinator());
        var sceneFactory = new SceneFactory(sceneManager, Content, graphics);

        // create the scene objects and preload their content
        sceneManager.LoadAndStart(sceneFactory);
    }

#endregion

#region Update and Draw

	/// <summary>
	/// Allows the game to run logic such as updating the world,
	/// checking for collisions, gathering input, and playing audio.
	/// </summary>
	/// <param name="gameTime">Provides a snapshot of timing values.</param>
	protected override void Update (GameTime gameTime)
	{
        // set game time for this frame
		Time.setFromGameTime(gameTime);
        // set the touch state for this frame
        TouchInput.UnityInput.touchCollection = TouchPanel.GetState();

        sceneManager.Update();

        base.Update (gameTime);
	}

	/// <summary>
	/// This is called when the game should draw itself. 
	/// </summary>
	/// <param name="gameTime">Provides a snapshot of timing values.</param>
	protected override void Draw (GameTime gameTime)
	{
		// Clear the backbuffer
		graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

		// draw scene
        sceneManager.Draw();

		base.Draw (gameTime);
	}

#endregion
}

