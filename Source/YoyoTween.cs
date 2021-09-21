namespace Apos.Tweens {
    public class YoyoTween<T> : ITween<T> {
        public YoyoTween(ITween<T> tween) {
            _tween = new ThenTween<T>(tween, tween.Reverse());
        }

        public T A => _tween.A;
        public T B => _tween.B;
        public long StartTime => _tween.StartTime;
        public long Duration => _tween.Duration;

        public T Value => ValueAt(TweenHelper.TotalMS - StartTime);
        public T ValueAt(long ms) {
            return _tween.ValueAt(ms);
        }

        protected ITween<T> _tween;
    }

    public static class YoyoExtensions {
        public static ITween<T> Yoyo<T>(this ITween<T> tween) {
            return new ThenTween<T>(tween, tween.Reverse());
        }
    }
}
