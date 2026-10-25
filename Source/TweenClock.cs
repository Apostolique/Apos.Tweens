using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    /// <summary>
    /// The time that tweens read their value from. Each clock can be paused or slowed down on its own.
    /// </summary>
    public class TweenClock {
        /// <summary>The current time in milliseconds.</summary>
        public double TotalMS { get; set; }
        /// <summary>How fast time goes by on this clock. 1 is normal speed, 0.5 is half speed, 0 is paused.</summary>
        public float Scale { get; set; } = 1f;

        /// <summary>
        /// Moves the clock forward by the game time that went by since the last call, multiplied by
        /// <see cref="Scale"/>. The first call only remembers the game time. Calling it twice in the same
        /// update doesn't move the clock twice.
        /// </summary>
        public void Update(GameTime gameTime) {
            double total = gameTime.TotalGameTime.TotalMilliseconds;
            if (_lastGameMS is double last) {
                Update(total - last);
            }
            _lastGameMS = total;
        }
        /// <summary>Moves the clock forward by <paramref name="elapsedMS"/> multiplied by <see cref="Scale"/>.</summary>
        public void Update(double elapsedMS) {
            TotalMS += elapsedMS * Scale;
        }

        private double? _lastGameMS;
    }
}
