namespace Apos.Tweens {
    /// <summary>
    /// Goes from one value to another with an easing, while the velocity the value already had fades out. The
    /// motion keeps going the way it was, then curves toward the target instead of turning a sharp corner.
    /// </summary>
    internal sealed class RetargetTween<T> : ITween<T> {
        public RetargetTween(T from, T to, T velocity, double duration, EasingFunction easing, Lerp<T> lerp) {
            TweenMath<T>.RequireVectorMath("Retarget");
            _from = from;
            _to = to;
            _kick = Kick(velocity, duration);
            _easing = easing;
            _lerp = lerp;
            Duration = duration;
        }

        public double Duration { get; }

        public T ValueAt(double ms) {
            if (ms >= Duration) return _to;
            if (ms <= 0) return _from;
            return Eval(_from, _to, _kick, _easing, _lerp, (float)(ms / Duration));
        }

        /// <summary>How far the velocity would carry the value over the whole move, before it fades.</summary>
        internal static T Kick(T velocity, double duration) {
            return TweenMath<T>.Scale(velocity, (float)duration);
        }

        /// <summary>
        /// The eased value at <paramref name="t"/> from 0 to 1, plus the kick scaled by t(1 - t)². That curve
        /// starts at 0 with a slope of 1, so the velocity carries over, and it's back to 0 with no slope at the end.
        /// </summary>
        internal static T Eval(T from, T to, T kick, EasingFunction easing, Lerp<T> lerp, float t) {
            float fade = t * (1f - t) * (1f - t);
            return TweenMath<T>.Add(lerp(from, to, easing(t)), TweenMath<T>.Scale(kick, fade));
        }

        private readonly T _from;
        private readonly T _to;
        private readonly T _kick;
        private readonly EasingFunction _easing;
        private readonly Lerp<T> _lerp;
    }
}
