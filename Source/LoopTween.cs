using System;

namespace Apos.Tweens {
    /// <summary>Plays a tween over and over, forever or a set number of times.</summary>
    public class LoopTween<T> : ITween<T> {
        /// <summary>Loops forever.</summary>
        public LoopTween(ITween<T> tween) : this(tween, double.PositiveInfinity) { }
        /// <summary>Plays <paramref name="tween"/> <paramref name="count"/> times. A count of 2.5 stops halfway through the third time.</summary>
        public LoopTween(ITween<T> tween, double count) {
            _tween = tween;
            Count = count;
        }

        /// <summary>How many times the tween plays. <see cref="double.PositiveInfinity"/> loops forever.</summary>
        public double Count { get; }
        public double Duration => _tween.Duration <= 0 ? 0 : Count * _tween.Duration;

        public T ValueAt(double ms) {
            double duration = _tween.Duration;
            if (ms <= 0 || duration <= 0) return _tween.ValueAt(ms);
            if (ms >= Duration) {
                double last = Count - Math.Floor(Count);
                return _tween.ValueAt(last > 0 ? last * duration : duration);
            }

            return _tween.ValueAt(ms % duration);
        }

        protected ITween<T> _tween;
    }

    public static class LoopExtensions {
        /// <summary>Plays <paramref name="tween"/> over and over, forever.</summary>
        public static ITween<T> Loop<T>(this ITween<T> tween) {
            return new LoopTween<T>(tween);
        }
        /// <summary>Plays <paramref name="tween"/> <paramref name="count"/> times.</summary>
        public static ITween<T> Loop<T>(this ITween<T> tween, double count) {
            return new LoopTween<T>(tween, count);
        }
    }
}
