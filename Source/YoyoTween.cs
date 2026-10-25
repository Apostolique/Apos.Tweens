using System;

namespace Apos.Tweens {
    /// <summary>Plays a tween forward, then backward to where it started.</summary>
    public class YoyoTween<T> : ITween<T> {
        public YoyoTween(ITween<T> tween) {
            _tween = tween;
        }

        public double Duration => 2 * _tween.Duration;

        public T ValueAt(double ms) {
            double duration = _tween.Duration;
            return _tween.ValueAt(ms <= duration ? ms : duration - Math.Min(ms - duration, duration));
        }

        protected ITween<T> _tween;
    }

    public static class YoyoExtensions {
        /// <summary>Plays <paramref name="tween"/> forward, then backward to where it started.</summary>
        public static ITween<T> Yoyo<T>(this ITween<T> tween) {
            return new YoyoTween<T>(tween);
        }
    }
}
