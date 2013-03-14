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
	SpriteBatch spriteBatch;
	Texture2D logoTexture;
	Camera camera;
	BasicEffect basicEffect;
	TitleScene titleScene;
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
		camera = new Camera(graphics.GraphicsDevice.Viewport);
		Camera.main = camera;
		basicEffect = new BasicEffect (graphics.GraphicsDevice);
		basicEffect.VertexColorEnabled = true;
		basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height, 0, 0, 1);

		base.Initialize ();
	}


	/// <summary>
	/// Load your graphics content.
	/// </summary>
	protected override void LoadContent ()
	{
		// Create a new SpriteBatch, which can be use to draw textures.
		spriteBatch = new SpriteBatch (graphics.GraphicsDevice);

		titleScene = new TitleScene (Content, spriteBatch);			
		titleScene.Setup (new GameTime().TotalGameTime.Milliseconds);

		// TODO: use this.Content to load your game content here eg.
		logoTexture = Content.Load<Texture2D> ("logo");
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
		// TODO: Add your update logic here			
        		
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


		//spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.ViewMatrix());
		spriteBatch.Begin ();

		// draw the logo
		spriteBatch.Draw (logoTexture, new Vector2 (130, 200), Color.White);

		// draw scene
		titleScene.Draw();

		spriteBatch.End();
		
		basicEffect.CurrentTechnique.Passes[0].Apply();

		//TODO: Add your drawing code here
		base.Draw (gameTime);
	}

#endregion
}

