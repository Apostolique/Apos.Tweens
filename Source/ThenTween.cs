namespace Apos.Tweens {
    public class ThenTween<T> : ITween<T> {
        public ThenTween(ITween<T> tween1, ITween<T> tween2) {
            _tween1 = tween1;
            _tween2 = tween2;
        }

        public T A => _tween1.A;
        public T B => _tween2.B;
        public long StartTime => _tween1.StartTime;
        public long Duration => _tween1.Duration + _tween2.Duration;

        public T Value => ValueAt(TweenHelper.TotalMS - StartTime);
        public T ValueAt(long ms) {
            if (ms <= _tween1.Duration) return _tween1.ValueAt(ms);

            return _tween2.ValueAt(ms - _tween1.Duration);
        }

        protected ITween<T> _tween1;
        protected ITween<T> _tween2;
    }

    public static class ThenExtensions {
        public static ITween<T> Then<T>(this ITween<T> tween1, ITween<T> tween2) {
            return new ThenTween<T>(tween1, tween2);
        }
    }
}
