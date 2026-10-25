namespace Apos.Tweens {
    /// <summary>Returns the value at <paramref name="t"/> between <paramref name="a"/> (0) and <paramref name="b"/> (1).</summary>
    public delegate T Lerp<T>(T a, T b, float t);

    /// <summary>
    /// Goes from <see cref="A"/> to <see cref="B"/> over <see cref="Duration"/>. Works with any type that
    /// has a lerp function, like <c>Vector3.Lerp</c>.
    /// </summary>
    public class Tween<T> : ITween<T> {
        /// <summary>Starts now on <paramref name="clock"/>, or on <see cref="TweenHelper.Clock"/> when it's null.</summary>
        /// <param name="a">The value at the start.</param>
        /// <param name="b">The value at the end.</param>
        /// <param name="duration">The length in milliseconds.</param>
        /// <param name="easing">The shape of the motion, usually one of the functions in <see cref="Easing"/>.</param>
        /// <param name="lerp">How to go between two values.</param>
        /// <param name="clock">The clock to start on. Defaults to <see cref="TweenHelper.Clock"/>.</param>
        public Tween(T a, T b, double duration, Interpolator easing, Lerp<T> lerp, TweenClock clock = null) {
            A = a;
            B = b;
            Clock = clock ?? TweenHelper.Clock;
            StartTime = Clock.TotalMS;
            Duration = duration;
            Interpolator = easing;
            Lerp = lerp;
        }
        /// <summary>Starts at <paramref name="startTime"/> on <paramref name="clock"/>, or on <see cref="TweenHelper.Clock"/> when it's null.</summary>
        public Tween(T a, T b, double duration, Interpolator easing, Lerp<T> lerp, double startTime, TweenClock clock = null) {
            A = a;
            B = b;
            Clock = clock ?? TweenHelper.Clock;
            StartTime = startTime;
            Duration = duration;
            Interpolator = easing;
            Lerp = lerp;
        }

        public T A { get; set; }
        public T B { get; set; }
        public double StartTime { get; set; }
        public double Duration { get; set; }
        public TweenClock Clock { get; set; }
        /// <summary>The shape of the motion.</summary>
        public Interpolator Interpolator { get; set; }
        /// <summary>How to go between two values.</summary>
        public Lerp<T> Lerp { get; set; }

        public T Value => ValueAt(Clock.TotalMS - StartTime);
        public T ValueAt(double ms) {
            if (ms <= 0) return A;
            else if (ms >= Duration) return B;

            return Lerp(A, B, Interpolator((float)(ms / Duration)));
        }
    }

    public static class TweenExtensions {
        /// <summary>Plays <paramref name="tween"/>, then goes from its end value to <paramref name="target"/>.</summary>
        public static ITween<T> To<T>(this ITween<T> tween, T target, double duration, Interpolator easing, Lerp<T> lerp) {
            return tween.Then(new Tween<T>(tween.B, target, duration, easing, lerp));
        }

        /// <summary>Starts <paramref name="tween"/> over from the current time on its clock.</summary>
        public static ITween<T> Restart<T>(this ITween<T> tween) {
            tween.StartTime = tween.Clock.TotalMS;
            return tween;
        }
        /// <summary>Milliseconds since the start, according to the tween's clock. Negative before it starts.</summary>
        public static double Elapsed<T>(this ITween<T> tween) {
            return tween.Clock.TotalMS - tween.StartTime;
        }
        /// <summary>True once the tween has played to the end. A tween that loops forever is never done.</summary>
        public static bool IsDone<T>(this ITween<T> tween) {
            return tween.Elapsed() >= tween.Duration;
        }
    }
}
