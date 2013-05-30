using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class FullScreenQuad : Sprite {
    const float DEPTH = 20;

	public FullScreenQuad(GraphicsDeviceManager graphics, ContentManager content, object obj, params string[] textureNames)
		: base(graphics, content, obj, textureNames) { }
	public FullScreenQuad(GraphicsDeviceManager graphics, ContentManager content, params string[] texturePaths)
	: base(graphics, content, texturePaths) { }
	
	public FullScreenQuad(GraphicsDeviceManager graphics, Texture2D[] textures)
		: base (graphics, textures) { }


	override protected void createMesh()
	{
		var width = graphics.GraphicsDevice.Viewport.Width;
		var height = graphics.GraphicsDevice.Viewport.Height;

		var upperLeft = new Vector3(-(width / 2), -(height / 2), DEPTH);
		quad = new Quad(upperLeft, Vector3.Forward, Vector3.Down, Vector3.Right, width, height);
	}
}

