namespace Apos.Tweens {
    public class WaitTween<T> : ITween<T> {
        public WaitTween(T value, long duration) {
            A = value;
            StartTime = TweenHelper.TotalMS;
            Duration = duration;
        }
        public WaitTween(T value, long duration, long startTime) {
            A = value;
            StartTime = startTime;
            Duration = duration;
        }

        public T A { get; set; }
        public T B => A;
        public long StartTime { get; set; }
        public long Duration { get; set; }

        public T Value => ValueAt(TweenHelper.TotalMS - StartTime);
        public T ValueAt(long ms) {
            return A;
        }
    }

    public static class WaitExtensions {
        public static ITween<T> Wait<T>(this ITween<T> tween, long duration) {
            return tween.Then(new WaitTween<T>(tween.B, duration, tween.StartTime));
        }
    }
}
