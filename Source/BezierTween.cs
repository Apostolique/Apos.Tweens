using System;

namespace Apos.Tweens {
    /// <summary>
    /// Goes from <see cref="A"/> to <see cref="B"/> along a curve that bends toward one or two control points.
    /// It only needs a lerp, so it works with any type that <see cref="Tween{T}"/> works with.
    /// </summary>
    public class BezierTween<T> : ITween<T> {
        /// <param name="a">The value at the start.</param>
        /// <param name="control">The point the curve bends toward.</param>
        /// <param name="b">The value at the end.</param>
        /// <param name="duration">The length in milliseconds.</param>
        /// <param name="easing">How fast it moves along the curve, usually one of the functions in <see cref="Easing"/>.</param>
        /// <param name="lerp">How to go between two values. Defaults to <see cref="TweenMath{T}.Lerp"/>.</param>
        public BezierTween(T a, T control, T b, double duration, EasingFunction easing, Lerp<T> lerp = null) {
            _lerp = lerp ?? TweenMath<T>.Lerp ?? throw TweenMath<T>.NoLerp();
            _easing = easing ?? throw new ArgumentNullException(nameof(easing));
            A = a;
            B = b;
            Duration = duration;
            _c1 = _lerp(a, control, 2f / 3f);
            _c2 = _lerp(b, control, 2f / 3f);
        }
        /// <param name="a">The value at the start.</param>
        /// <param name="control1">The point the curve leaves <paramref name="a"/> toward.</param>
        /// <param name="control2">The point the curve arrives at <paramref name="b"/> from.</param>
        /// <param name="b">The value at the end.</param>
        /// <param name="duration">The length in milliseconds.</param>
        /// <param name="easing">How fast it moves along the curve, usually one of the functions in <see cref="Easing"/>.</param>
        /// <param name="lerp">How to go between two values. Defaults to <see cref="TweenMath{T}.Lerp"/>.</param>
        public BezierTween(T a, T control1, T control2, T b, double duration, EasingFunction easing, Lerp<T> lerp = null) {
            _lerp = lerp ?? TweenMath<T>.Lerp ?? throw TweenMath<T>.NoLerp();
            _easing = easing ?? throw new ArgumentNullException(nameof(easing));
            A = a;
            B = b;
            Duration = duration;
            _c1 = control1;
            _c2 = control2;
        }

        /// <summary>The value at the start.</summary>
        public T A { get; }
        /// <summary>The value at the end.</summary>
        public T B { get; }
        public double Duration { get; }

        public T ValueAt(double ms) {
            if (ms <= 0) return A;
            if (ms >= Duration) return B;

            float t = _easing((float)(ms / Duration));
            T ab = _lerp(A, _c1, t);
            T bc = _lerp(_c1, _c2, t);
            T cd = _lerp(_c2, B, t);
            return _lerp(_lerp(ab, bc, t), _lerp(bc, cd, t), t);
        }

        private readonly T _c1;
        private readonly T _c2;
        private readonly EasingFunction _easing;
        private readonly Lerp<T> _lerp;
    }

    public static class BezierExtensions {
        /// <summary>
        /// Plays <paramref name="tween"/>, then goes from its end value to <paramref name="target"/> along a
        /// curve that bends toward <paramref name="control"/>. It uses the same lerp as the step before.
        /// </summary>
        public static Tween<T> CurveTo<T>(this ITween<T> tween, T control, T target, double duration, EasingFunction easing) {
            Tween<T> head = Tween<T>.From(tween);
            return head.AppendTween(new BezierTween<T>(head.B, control, target, duration, easing, head.LastLerp));
        }
        /// <summary>
        /// Plays <paramref name="tween"/>, then goes from its end value to <paramref name="target"/> along a
        /// curve that leaves toward <paramref name="control1"/> and arrives from <paramref name="control2"/>.
        /// It uses the same lerp as the step before.
        /// </summary>
        public static Tween<T> CurveTo<T>(this ITween<T> tween, T control1, T control2, T target, double duration, EasingFunction easing) {
            Tween<T> head = Tween<T>.From(tween);
            return head.AppendTween(new BezierTween<T>(head.B, control1, control2, target, duration, easing, head.LastLerp));
        }
    }
}
