using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class Camera
{
	public static Camera main;

	public readonly Viewport viewport;
	public Matrix world {get; private set;}
	public Matrix view  {get; private set;}
	public Matrix projection  {get; private set;}

	public const float NEAR = 0.1f;
	public const float FAR = 1000f;
	// half the camera height
    public float orthographicSize { get { return 160; } }

    public int pixelWidth { get { return viewport.Width; } }
    public int pixelHeight { get { return viewport.Height; } }

	public Camera (Viewport viewport)
	{
		this.viewport = viewport;

		// Leave the world be
		world = Matrix.Identity; 

//		// build a camera with dimensions that match the screen resolution
//		Vector2 center;
//		center.X = viewport.Width * 0.5f;
//		center.Y = viewport.Height * 0.5f;

//		view = Matrix.CreateLookAt(new Vector3(center, -10), new Vector3(center, 0), Vector3.Down);

		view = Matrix.CreateLookAt(new Vector3(0, 0, -10), new Vector3(0, 0, 0), Vector3.Down);

		// Create an orthographic projection
		projection = Matrix.CreateOrthographic(2 * orthographicSize * viewport.AspectRatio, orthographicSize * 2, NEAR, FAR);
	}

	public Vector3 WorldToScreenPoint(Vector3 source) {
		return viewport.Project(source, projection, view, world); 
	}

	public Vector2 WorldToScreen2D (Vector2 screenPosition)
	{
		var p = WorldToScreenPoint (new Vector3 (screenPosition, NEAR));
		// Programmer needs swizzling, badly.
		return new Vector2 (p.X, p.Y);
	}

	public Vector3 ScreenToWorldPoint(Vector3 source) {
		return viewport.Unproject(source, projection, view, world); 
	}
	
	public Vector2 ScreenToWorld2D(Vector2 source) {
		var p = ScreenToWorldPoint(new Vector3 (source, NEAR));
		return new Vector2 (p.X, p.Y);
	}

	public Vector3 ViewportToWorldPoint (Vector3 pos)
	{
		var source = new Vector3 (pos.X * viewport.Width, viewport.Height - (pos.Y * viewport.Height), NEAR);
		return ScreenToWorldPoint (source);
	}

	public Vector2 ViewportToWorld2D (Vector2 pos)
	{
		var p = ViewportToWorldPoint (new Vector3 (pos, NEAR));
		return new Vector2 (p.X, p.Y);
	}

	public Vector2 MouseToWorld(int x, int y)
	{
		return Vector2.Transform(new Vector2(x, y), Matrix.CreateScale(1, -1, 1) * Matrix.CreateTranslation(0, viewport.Height, 0));
	}
}

