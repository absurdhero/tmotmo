using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class FullScreenQuad : Sprite {
    const float DEPTH = 20;

    public FullScreenQuad() : base () {}

	override public Quad createMesh()
	{
		var width = Camera.main.viewport.Width;
        var height = Camera.main.viewport.Height;

		var upperLeft = new Vector3(-(width / 2), -(height / 2), DEPTH);
		return new Quad(upperLeft, Vector3.Forward, Vector3.Down, Vector3.Right, width, height);
	}
}

