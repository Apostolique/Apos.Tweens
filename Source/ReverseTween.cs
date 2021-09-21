namespace Apos.Tweens {
    public class ReverseTween<T> : ITween<T> {
        public ReverseTween(ITween<T> tween) {
            _tween = tween;
        }

        public T A => _tween.B;
        public T B => _tween.A;
        public long StartTime => _tween.StartTime;
        public long Duration => _tween.Duration;

        public T Value => ValueAt(TweenHelper.TotalMS - StartTime);
        public T ValueAt(long ms) {
            if (ms < 0f) return B;
            else if (ms > Duration) return A;

            return _tween.ValueAt(_tween.Duration - ms);
        }

        protected ITween<T> _tween;
    }

    public static class ReverseExtensions {
        public static ITween<T> Reverse<T>(this ITween<T> tween) {
            return new ReverseTween<T>(tween);
        }
    }
}
