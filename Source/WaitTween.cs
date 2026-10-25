namespace Apos.Tweens {
    /// <summary>Holds a value without changing it.</summary>
    public class WaitTween<T> : ITween<T> {
        public WaitTween(T value, double duration, TweenClock clock = null) {
            A = value;
            Clock = clock ?? TweenHelper.Clock;
            StartTime = Clock.TotalMS;
            Duration = duration;
        }
        public WaitTween(T value, double duration, double startTime, TweenClock clock = null) {
            A = value;
            Clock = clock ?? TweenHelper.Clock;
            StartTime = startTime;
            Duration = duration;
        }

        public T A { get; set; }
        public T B => A;
        public double StartTime { get; set; }
        public double Duration { get; set; }
        public TweenClock Clock { get; set; }

        public T Value => ValueAt(Clock.TotalMS - StartTime);
        public T ValueAt(double ms) {
            return A;
        }
    }

    public static class WaitExtensions {
        /// <summary>Plays <paramref name="tween"/>, then holds its end value for <paramref name="duration"/> milliseconds.</summary>
        public static ITween<T> Wait<T>(this ITween<T> tween, double duration) {
            return tween.Then(NextTween<T>.Wait(tween, duration));
        }
    }
}
