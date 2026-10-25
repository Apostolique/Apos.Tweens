namespace Apos.Tweens {
    /// <summary>Plays a tween forward, then backward to where it started.</summary>
    public class YoyoTween<T> : ITween<T> {
        public YoyoTween(ITween<T> tween) {
            _tween = new ThenTween<T>(tween, new ReverseTween<T>(tween));
        }

        public T A => _tween.A;
        public T B => _tween.B;
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
            return _tween.ValueAt(ms);
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
