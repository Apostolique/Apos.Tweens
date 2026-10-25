namespace Apos.Tweens {
    /// <summary>
    /// Plays a tween on a clock. The tween only describes the motion, so many players can share one, each
    /// with its own start time.
    /// </summary>
    public class TweenPlayer<T> {
        /// <summary>Starts now on <paramref name="clock"/>, or on <see cref="TweenHelper.Clock"/> when it's null.</summary>
        public TweenPlayer(ITween<T> tween, TweenClock clock = null) {
            Tween = tween;
            Clock = clock ?? TweenHelper.Clock;
            StartTime = Clock.TotalMS;
        }
        /// <summary>Starts at <paramref name="startTime"/> on <paramref name="clock"/>, or on <see cref="TweenHelper.Clock"/> when it's null.</summary>
        public TweenPlayer(ITween<T> tween, double startTime, TweenClock clock = null) {
            Tween = tween;
            Clock = clock ?? TweenHelper.Clock;
            StartTime = startTime;
        }

        /// <summary>The tween that's playing.</summary>
        public ITween<T> Tween { get; set; }
        /// <summary>The clock that drives <see cref="Value"/>.</summary>
        public TweenClock Clock { get; set; }
        /// <summary>When the tween starts, in milliseconds on <see cref="Clock"/>.</summary>
        public double StartTime { get; set; }

        /// <summary>The value at the current time of <see cref="Clock"/>.</summary>
        public T Value => Tween.ValueAt(Elapsed);
        /// <summary>Milliseconds since the start. Negative before it starts.</summary>
        public double Elapsed => Clock.TotalMS - StartTime;
        /// <summary>True once the tween has played to the end. A tween that loops forever is never done.</summary>
        public bool IsDone => Elapsed >= Tween.Duration;

        /// <summary>Starts the tween over from the current time on <see cref="Clock"/>.</summary>
        public void Restart() {
            StartTime = Clock.TotalMS;
        }
    }

    public static class TweenPlayerExtensions {
        /// <summary>Plays <paramref name="tween"/> from now on <paramref name="clock"/>, or on <see cref="TweenHelper.Clock"/> when it's null.</summary>
        public static TweenPlayer<T> Play<T>(this ITween<T> tween, TweenClock clock = null) {
            return new TweenPlayer<T>(tween, clock);
        }
        /// <summary>Plays <paramref name="tween"/> from <paramref name="startTime"/> on <paramref name="clock"/>, or on <see cref="TweenHelper.Clock"/> when it's null.</summary>
        public static TweenPlayer<T> Play<T>(this ITween<T> tween, double startTime, TweenClock clock = null) {
            return new TweenPlayer<T>(tween, startTime, clock);
        }
    }
}
