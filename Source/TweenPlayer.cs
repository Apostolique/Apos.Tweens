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
        /// <summary>How fast <see cref="Value"/> changes, in units per millisecond. See <see cref="VelocityExtensions.VelocityAt"/>.</summary>
        public T Velocity => Tween.VelocityAt(Elapsed);
        /// <summary>Milliseconds since the start. Negative before it starts.</summary>
        public double Elapsed => Clock.TotalMS - StartTime;
        /// <summary>True once the tween has played to the end. A tween that loops forever is never done.</summary>
        public bool IsDone => Elapsed >= Tween.Duration;

        /// <summary>Starts the tween over from the current time on <see cref="Clock"/>.</summary>
        public void Restart() {
            StartTime = Clock.TotalMS;
        }
        /// <summary>
        /// Plays a new tween from now that goes from <see cref="Value"/> to <paramref name="target"/> in
        /// <paramref name="duration"/> milliseconds. It leaves at the current <see cref="Velocity"/> and slows
        /// down to a stop, so the motion doesn't kink when the target changes while it plays.
        /// </summary>
        public void Retarget(T target, double duration) {
            T value = Value;
            T velocity = Velocity;
            Tween = new HermiteTween<T>(value, velocity, target, TweenMath<T>.Scale(target, 0f), duration);
            Restart();
        }
        /// <summary>
        /// Plays a new tween from now that eases from <see cref="Value"/> to <paramref name="target"/> in
        /// <paramref name="duration"/> milliseconds. The current <see cref="Velocity"/> carries over and fades
        /// out. With an easing that starts at rest, like <see cref="Easing.SineInOut"/>, the speed doesn't jump at all.
        /// </summary>
        public void Retarget(T target, double duration, EasingFunction easing, Lerp<T> lerp = null) {
            if (easing == null) throw new System.ArgumentNullException(nameof(easing));
            T value = Value;
            T velocity = Velocity;
            Tween = new RetargetTween<T>(value, target, velocity, duration, easing, lerp ?? TweenMath<T>.Lerp ?? throw TweenMath<T>.NoLerp());
            Restart();
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
