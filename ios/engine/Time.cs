using System.Collections.Generic;

using Microsoft.Xna.Framework;

/// Cycles through sprite frames at a defined rate
public class Time
{
    private static GameTime gameTime;

    public static float time {
        get;
        private set;
    }

    public static void setFromGameTime(GameTime newTime) {
        gameTime = newTime;
        time = (float) gameTime.TotalGameTime.Seconds + (float) gameTime.TotalGameTime.Milliseconds / 1000f;
    }
}

