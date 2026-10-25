using System;

namespace Apos.Tweens {
    /// <summary>Plays a tween backward, from its end to its start.</summary>
    public class ReverseTween<T> : ITween<T> {
        public ReverseTween(ITween<T> tween) {
            _tween = tween;
        }

        public double Duration => _tween.Duration;

        public T ValueAt(double ms) {
            double duration = _tween.Duration;
            return _tween.ValueAt(duration - Math.Clamp(ms, 0, duration));
        }

        protected ITween<T> _tween;
    }

    public static class ReverseExtensions {
        /// <summary>Plays <paramref name="tween"/> backward, from its end to its start.</summary>
        public static ITween<T> Reverse<T>(this ITween<T> tween) {
            return new ReverseTween<T>(tween);
        }
    }
}
