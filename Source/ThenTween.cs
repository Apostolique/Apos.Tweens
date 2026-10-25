namespace Apos.Tweens {
    /// <summary>Plays one tween, then another.</summary>
    public class ThenTween<T> : ITween<T> {
        public ThenTween(ITween<T> tween1, ITween<T> tween2) {
            _tween1 = tween1;
            _tween2 = tween2;
        }

        public T A => _tween1.A;
        public T B => _tween2.B;
        public double StartTime {
            get => _tween1.StartTime;
            set => _tween1.StartTime = value;
        }
        public double Duration => _tween1.Duration + _tween2.Duration;
        public TweenClock Clock {
            get => _tween1.Clock;
            set => _tween1.Clock = value;
        }

        public T Value => ValueAt(Clock.TotalMS - StartTime);
        public T ValueAt(double ms) {
            double duration1 = _tween1.Duration;
            if (ms <= duration1) return _tween1.ValueAt(ms);

            return _tween2.ValueAt(ms - duration1);
        }

        protected ITween<T> _tween1;
        protected ITween<T> _tween2;
    }

    public static class ThenExtensions {
        /// <summary>Plays <paramref name="tween1"/>, then <paramref name="tween2"/>.</summary>
        public static ITween<T> Then<T>(this ITween<T> tween1, ITween<T> tween2) {
            return new ThenTween<T>(tween1, tween2);
        }
    }
}
