using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public static class TweenHelper {
        /// <summary>Moves <see cref="Clock"/> forward. Call this at the start of your game's update.</summary>
        public static void UpdateSetup(GameTime gameTime) {
            Clock.Update(gameTime);
        }
        /// <summary>Moves <see cref="Clock"/> forward by <paramref name="elapsedMS"/> multiplied by its scale.</summary>
        public static void UpdateSetup(double elapsedMS) {
            Clock.Update(elapsedMS);
        }

        /// <summary>The clock that tweens use when they aren't given one.</summary>
        public static TweenClock Clock { get; set; } = new TweenClock();
        /// <summary>The current time of <see cref="Clock"/> in milliseconds.</summary>
        public static double TotalMS {
            get => Clock.TotalMS;
            set => Clock.TotalMS = value;
        }
    }
}
