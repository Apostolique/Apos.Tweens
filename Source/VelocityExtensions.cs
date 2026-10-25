using System;

namespace Apos.Tweens {
    public static class VelocityExtensions {
        /// <summary>
        /// How fast the value of <paramref name="tween"/> changes at <paramref name="ms"/>, in units per
        /// millisecond. It compares the values <paramref name="delta"/> milliseconds before and after. It's 0
        /// before the start and after the end. It needs <see cref="TweenMath{T}.Subtract"/> and <see cref="TweenMath{T}.Scale"/>.
        /// </summary>
        public static T VelocityAt<T>(this ITween<T> tween, double ms, double delta = 1) {
            var subtract = TweenMath<T>.Subtract;
            var scale = TweenMath<T>.Scale;
            if (subtract == null || scale == null) {
                throw new InvalidOperationException($"Velocities need TweenMath<{typeof(T).Name}>.Subtract and Scale to be set.");
            }

            double duration = tween.Duration;
            double before = Math.Max(ms - delta, 0);
            double after = Math.Min(ms + delta, duration);
            if (ms < 0 || ms > duration || !(after > before)) return scale(tween.ValueAt(ms), 0f);

            return scale(subtract(tween.ValueAt(after), tween.ValueAt(before)), (float)(1 / (after - before)));
        }
    }
}
