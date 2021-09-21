namespace Apos.Tweens {
    public class LoopTween<T> : ITween<T> {
        public LoopTween(ITween<T> tween) {
            _tween = tween;
        }

        public T A => _tween.A;
        public T B => _tween.B;
        public long StartTime => _tween.StartTime;
        public long Duration => _tween.Duration;

        public T Value => ValueAt(TweenHelper.TotalMS - StartTime);
        public T ValueAt(long ms) {
            return _tween.ValueAt(ms % _tween.Duration);
        }

        protected ITween<T> _tween;
    }

    public static class LoopExtensions {
        public static ITween<T> Loop<T>(this ITween<T> tween) {
            return new LoopTween<T>(tween);
        }
    }
}
