using System;
using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public static class TweenHelper {
        public static void UpdateSetup(long totalMS) {
            TotalMS = totalMS;
        }
        public static void UpdateSetup(GameTime gameTime) {
            TotalMS = gameTime.TotalGameTime.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static long TotalMS { get; set; }
    }
}
