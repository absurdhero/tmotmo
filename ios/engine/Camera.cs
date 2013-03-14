using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class Camera
{
	public static Camera main;

	private readonly Viewport viewport;
	private Matrix world, view, projection;

	public const float NEAR = 0.1f;
	public const float FAR = 1000f;
	// half the camera height
	public const float orthographicSize = 100;

	public Camera (Viewport viewport)
	{
		this.viewport = viewport;

		// Leave the world be
		world = Matrix.Identity; 
		
		// Place the camera at vector (0,0,10) and look at vector (0,0,0)
		view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.Up);
		
		// Create an orthographic projection
		projection = Matrix.CreateOrthographic(2 * orthographicSize * viewport.AspectRatio, orthographicSize * 2, NEAR, FAR);
	}

	public Matrix ViewMatrix()
	{
		return Matrix.CreateScale(1, -1, 1) * Matrix.CreateTranslation(0, viewport.Height, 0);
	}

	public Vector3 WorldToScreenPoint(Vector3 source) {
		return viewport.Project(source, projection, view, world); 
	}

	public Vector2 WorldToScreen2D (Vector2 screenPosition)
	{
		// Programmer needs swizzling, badly.
		var p = WorldToScreenPoint (new Vector3 (screenPosition, NEAR));
		return new Vector2 (p.X, p.Y);
	}

	public Vector3 ScreenToWorldPoint(Vector3 source) {
		return viewport.Unproject(source, projection, view, world); 
	}
	
	public Vector2 ScreenToWorld2D(Vector2 source) {
		var p = viewport.Unproject (new Vector3 (source, NEAR), projection, view, world);
		return new Vector2 (p.X, p.Y);
	}

	public Vector3 ViewportToWorldPoint (Vector3 pos)
	{
		var source = new Vector3 (pos.X * viewport.Width, pos.Y * viewport.Height, NEAR);
		return ScreenToWorldPoint (source - view.Translation);

//		var source = new Vector3 (pos.X * viewport.Width - orthographicSize, orthographicSize - pos.Y * viewport.Height, NEAR);
//		return ScreenToWorldPoint (source);
	}

	public Vector2 ViewportToWorld2D (Vector2 pos)
	{
		var p = ViewportToWorldPoint (new Vector3 (pos, NEAR));
		return new Vector2 (p.X, p.Y);
//		var source = new Vector3 (pos.X * viewport.Width - orthographicSize, orthographicSize - pos.Y * viewport.Height, NEAR);
//		return ScreenToWorld2D(new Vector2(source.X, source.Y));
	}

	public Vector2 MouseToWorld(int x, int y)
	{
		return Vector2.Transform(new Vector2(x, y), Matrix.CreateScale(1, -1, 1) * Matrix.CreateTranslation(0, viewport.Height, 0));
	}
}

