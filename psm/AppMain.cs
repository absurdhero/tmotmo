using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace tmotmo.psm
{
	public class AppMain
	{
		private static Game1 game;
		
		public static void Main (string[] args)
		{
			game = new Game1();
			game.Run();

		}
	}
}
