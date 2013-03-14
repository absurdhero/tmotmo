using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NUnit.Framework;

[TestFixture()]
public class CameraTests
{
	[Test()]
	public void cameraDoesThings ()
	{
		System.Console.WriteLine ("Running tests");

		Viewport viewport = new Viewport();
		viewport.Height = 320;
		viewport.Width = 480;

		Camera camera = new Camera(viewport);


		var center = new Vector2(40, 10) + Camera.main.WorldToScreen2D(Vector2.Zero);
		System.Console.WriteLine ("center " + center);
		System.Console.WriteLine ("s0,0 : " + Camera.main.ScreenToWorld2D(Vector2.Zero));
		System.Console.WriteLine ("v1,1 : " + Camera.main.ViewportToWorld2D(Vector2.One));
		System.Console.WriteLine ("wcenter : " + Camera.main.ScreenToWorld2D(new Vector2(center.X, center.Y)));

		var layoutpos = camera.ViewportToWorld2D(new Vector2(0.5f, 0.65f));
		System.Console.WriteLine ("screenEdgesToWorld " + camera.ScreenToWorld2D(Vector2.Zero) + ", " + camera.ScreenToWorld2D(Vector2.One));
		System.Console.WriteLine ("viewToworld(0.5f, 0.65f) => " + layoutpos);
		System.Console.WriteLine ("viewToScreen => " + camera.WorldToScreenPoint(new Vector3(layoutpos, Camera.NEAR)));
		System.Console.WriteLine ("minus center viewToworld(0.5f, 0.65f) => " + layoutpos);
		System.Console.WriteLine ("minus center viewToScreen(0.5f, 0.65f) => " + camera.WorldToScreenPoint(new Vector3(layoutpos, Camera.NEAR)));

	}
}

