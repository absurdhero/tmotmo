using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NUnit.Framework;

[TestFixture]
public class CameraTests
{
	static Viewport iPhoneViewport ()
	{
		Viewport viewport = new Viewport ();
		viewport.Height = 320;
		viewport.Width = 480;
		viewport.MaxDepth = 1.0f;
		viewport.MinDepth = 0.0f;
		return viewport;
	}

	Vector2 iPhoneDimensions = new Vector2 (480, 320);

	[Test]
	public void originIsAtCenterOfScreen()
	{
		var viewport = iPhoneViewport();
		Camera camera = new Camera(viewport);
		
		Assert.That (camera.WorldToScreen2D(Vector2.Zero), Is.EqualTo(iPhoneDimensions / 2));
	}

	[Test]
	public void topOfScreenIs100UnitsFromMiddle()
	{
		var viewport = iPhoneViewport();
		Camera camera = new Camera(viewport);
		
		Assert.That (camera.WorldToScreen2D(new Vector2(100 * viewport.AspectRatio, 100)), Is.EqualTo(iPhoneDimensions));
	}

	[Test]
	public void centerPixelIsAtOrigin()
	{
		var viewport = iPhoneViewport();
		Camera camera = new Camera(viewport);
		
		Assert.That (camera.ScreenToWorld2D(iPhoneDimensions / 2), Is.EqualTo(Vector2.Zero));
	}

	[Test]
	public void centerOfViewportIsAtOrigin()
	{
		var viewport = iPhoneViewport();
		Camera camera = new Camera(viewport);
		
		Assert.That (camera.ViewportToWorld2D(new Vector2(0.5f, 0.5f)), Is.EqualTo(Vector2.Zero));
		Assert.That (camera.ViewportToWorld2D(new Vector2(0.5f, 0f)), Is.EqualTo(new Vector2(0, -100)));
	}

	[Test]
	public void cameraDoesThings ()
	{
		var viewport = iPhoneViewport();
		Camera camera = new Camera(viewport);

		var center = new Vector2(40, 10) + camera.WorldToScreen2D(Vector2.Zero);

		System.Console.WriteLine ("s0,0 : " + camera.ScreenToWorld2D(Vector2.Zero));
		System.Console.WriteLine ("v1,1 : " + camera.ViewportToWorld2D(Vector2.One));
		System.Console.WriteLine ("wcenter : " + camera.ScreenToWorld2D(new Vector2(center.X, center.Y)));

		var layoutpos = camera.ViewportToWorld2D(new Vector2(0.5f, 0.65f));
		System.Console.WriteLine ("screenEdgesToWorld " + camera.ScreenToWorld2D(Vector2.Zero) + ", " + camera.ScreenToWorld2D(Vector2.One));
		System.Console.WriteLine ("viewToworld(0.5f, 0.65f) => " + layoutpos);
		System.Console.WriteLine ("viewToScreen => " + camera.WorldToScreenPoint(new Vector3(layoutpos, Camera.NEAR)));
		System.Console.WriteLine ("minus center viewToworld(0.5f, 0.65f) => " + layoutpos);
		System.Console.WriteLine ("minus center viewToScreen(0.5f, 0.65f) => " + camera.WorldToScreenPoint(new Vector3(layoutpos, Camera.NEAR)));

	}
}

