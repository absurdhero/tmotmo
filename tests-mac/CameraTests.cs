using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NUnit.Framework;

[TestFixture]
public class CameraTests
{
	static Viewport iPhoneViewport {
		get {
			Viewport viewport = new Viewport ();
			viewport.Height = 320;
			viewport.Width = 480;
			viewport.MaxDepth = 1.0f;
			viewport.MinDepth = 0.0f;
			return viewport;
		}
	}

	float orthographicSize = 160;
	Vector2 iPhoneDimensions = new Vector2 (480, 320);
	Camera camera;

	[SetUp]
	public void createCamera() {
		 camera = new Camera (iPhoneViewport);
	}

	[Test]
	public void originIsAtCenterOfScreen()
	{
		Assert.That (camera.WorldToScreen2D(Vector2.Zero), Is.EqualTo(iPhoneDimensions / 2));
	}

	[Test]
	public void topOfScreenIs100UnitsFromMiddle()
	{
		Assert.That (camera.WorldToScreen2D(new Vector2(orthographicSize * iPhoneViewport.AspectRatio, orthographicSize)), Is.EqualTo(iPhoneDimensions));
	}

	[Test]
	public void centerPixelIsAtOrigin()
	{
		Assert.That (camera.ScreenToWorld2D(iPhoneDimensions / 2), Is.EqualTo(Vector2.Zero));
	}

	[Test]
	public void centerOfViewportIsAtOrigin()
	{
		Assert.That (camera.ViewportToWorld2D(new Vector2(0.5f, 0.5f)), Is.EqualTo(Vector2.Zero));
		Assert.That (camera.ViewportToWorld2D(new Vector2(0.5f, 0f)), Is.EqualTo(new Vector2(0, orthographicSize)));
	}
}

