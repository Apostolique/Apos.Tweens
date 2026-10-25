using System;

namespace Apos.Tweens {
    /// <summary>Plays a tween backward, from its end to its start.</summary>
    public class ReverseTween<T> : ITween<T> {
        public ReverseTween(ITween<T> tween) {
            _tween = tween;
        }

        public T A => _tween.B;
        public T B => _tween.A;
        public double StartTime {
            get => _tween.StartTime;
            set => _tween.StartTime = value;
        }
        public double Duration => _tween.Duration;
        public TweenClock Clock {
            get => _tween.Clock;
            set => _tween.Clock = value;
        }

        public T Value => ValueAt(Clock.TotalMS - StartTime);
        public T ValueAt(double ms) {
            return _tween.ValueAt(Duration - Math.Clamp(ms, 0, Duration));
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
