using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

[Register("AppDelegate")]
class Program : UIApplicationDelegate
{
	Game1 game;

	public override void FinishedLaunching (UIApplication app)
	{
		// Fun begins..
		game = new Game1 ();
		game.Run ();
	}

	public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations (UIApplication application, UIWindow forWindow)
	{
		return UIInterfaceOrientationMask.Landscape;
	}

	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	static void Main (string[] args)
	{
		UIApplication.Main (args, null, "AppDelegate");
		
	}
}    



