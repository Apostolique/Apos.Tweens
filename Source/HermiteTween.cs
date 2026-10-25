namespace Apos.Tweens {
    /// <summary>
    /// Goes from <see cref="A"/> to <see cref="B"/>, leaving at one velocity and arriving at another. Velocities
    /// are in units per millisecond. It needs <see cref="TweenMath{T}.Add"/>, <see cref="TweenMath{T}.Subtract"/>
    /// and <see cref="TweenMath{T}.Scale"/>.
    /// </summary>
    public class HermiteTween<T> : ITween<T> {
        /// <param name="a">The value at the start.</param>
        /// <param name="velocityA">The velocity at the start, in units per millisecond.</param>
        /// <param name="b">The value at the end.</param>
        /// <param name="velocityB">The velocity at the end, in units per millisecond.</param>
        /// <param name="duration">The length in milliseconds.</param>
        public HermiteTween(T a, T velocityA, T b, T velocityB, double duration) {
            TweenMath<T>.RequireVectorMath(nameof(HermiteTween<T>));
            A = a;
            VelocityA = velocityA;
            B = b;
            VelocityB = velocityB;
            Duration = duration;
        }

        /// <summary>The value at the start.</summary>
        public T A { get; }
        /// <summary>The velocity at the start, in units per millisecond.</summary>
        public T VelocityA { get; }
        /// <summary>The value at the end.</summary>
        public T B { get; }
        /// <summary>The velocity at the end, in units per millisecond.</summary>
        public T VelocityB { get; }
        public double Duration { get; }

        public T ValueAt(double ms) {
            if (ms <= 0) return A;
            if (ms >= Duration) return B;
            return Hermite(A, VelocityA, B, VelocityB, Duration, (float)(ms / Duration));
        }

        /// <summary>The value at <paramref name="t"/> from 0 to 1 on a curve that lasts <paramref name="duration"/> milliseconds.</summary>
        internal static T Hermite(T a, T velocityA, T b, T velocityB, double duration, float t) {
            float t2 = t * t;
            float t3 = t2 * t;
            float d = (float)duration;

            var add = TweenMath<T>.Add;
            var scale = TweenMath<T>.Scale;
            T value = add(scale(a, 2f * t3 - 3f * t2 + 1f), scale(b, 3f * t2 - 2f * t3));
            value = add(value, scale(velocityA, (t3 - 2f * t2 + t) * d));
            return add(value, scale(velocityB, (t3 - t2) * d));
        }
    }
}
