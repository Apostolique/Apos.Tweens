namespace Apos.Tweens {
    /// <summary>Plays a tween over and over, forever or a set number of times.</summary>
    public class LoopTween<T> : ITween<T> {
        /// <summary>Loops forever.</summary>
        public LoopTween(ITween<T> tween) : this(tween, double.PositiveInfinity) { }
        /// <summary>Plays <paramref name="tween"/> <paramref name="count"/> times.</summary>
        public LoopTween(ITween<T> tween, double count) {
            _tween = tween;
            Count = count;
        }

        public T A => _tween.A;
        public T B => _tween.B;
        public double StartTime {
            get => _tween.StartTime;
            set => _tween.StartTime = value;
        }
        public double Duration => _tween.Duration <= 0 ? 0 : Count * _tween.Duration;
        public TweenClock Clock {
            get => _tween.Clock;
            set => _tween.Clock = value;
        }
        /// <summary>How many times the tween plays. <see cref="double.PositiveInfinity"/> loops forever.</summary>
        public double Count { get; set; }

        public T Value => ValueAt(Clock.TotalMS - StartTime);
        public T ValueAt(double ms) {
            double duration = _tween.Duration;
            if (ms <= 0 || duration <= 0) return _tween.ValueAt(ms);
            if (ms >= Duration) return _tween.ValueAt(duration);

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
