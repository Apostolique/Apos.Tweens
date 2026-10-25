using System;

namespace Apos.Tweens {
    /// <summary>
    /// Goes through each point in order along a smooth curve, a Catmull-Rom spline. It needs
    /// <see cref="TweenMath{T}.Add"/>, <see cref="TweenMath{T}.Subtract"/> and <see cref="TweenMath{T}.Scale"/>.
    /// </summary>
    public class SplineTween<T> : ITween<T> {
        /// <summary>Each point to the next takes the same time.</summary>
        /// <param name="points">The points to go through, at least 1.</param>
        /// <param name="duration">The length in milliseconds.</param>
        /// <param name="easing">How fast it moves along the whole spline, usually one of the functions in <see cref="Easing"/>.</param>
        public SplineTween(T[] points, double duration, EasingFunction easing)
            : this(points, EvenTimes(points, duration)) {
            _easing = easing ?? throw new ArgumentNullException(nameof(easing));
        }
        /// <summary>
        /// Reaches each point at its time. Before the first time, it holds the first point. The speed changes
        /// smoothly from one point to the next.
        /// </summary>
        /// <param name="points">The points to go through, at least 1.</param>
        /// <param name="times">When to reach each point, in milliseconds from the start. Each one comes after the one before.</param>
        public SplineTween(T[] points, double[] times) {
            if (points == null) throw new ArgumentNullException(nameof(points));
            if (times == null) throw new ArgumentNullException(nameof(times));
            if (points.Length == 0) throw new ArgumentException("A spline needs at least 1 point.", nameof(points));
            if (times.Length != points.Length) throw new ArgumentException("There needs to be a time for each point.", nameof(times));
            for (int i = 1; i < times.Length; i++) {
                if (!(times[i] > times[i - 1])) throw new ArgumentException("Each time needs to come after the one before.", nameof(times));
            }
            TweenMath<T>.RequireVectorMath(nameof(SplineTween<T>));

            _points = (T[])points.Clone();
            _times = (double[])times.Clone();
            _velocities = Velocities(_points, _times);
        }

        public double Duration => _times[_times.Length - 1];

        public T ValueAt(double ms) {
            int last = _points.Length - 1;
            if (_easing != null && Duration > 0) {
                ms = _easing((float)Math.Clamp(ms / Duration, 0, 1)) * Duration;
            }
            if (last == 0 || ms <= _times[0]) return _points[0];
            if (ms >= _times[last]) return _points[last];

            int i = StepSearch.Find(_times, last + 1, ref _hint, ms) - 1;
            double start = _times[i];
            double duration = _times[i + 1] - start;
            return HermiteTween<T>.Hermite(_points[i], _velocities[i], _points[i + 1], _velocities[i + 1], duration, (float)((ms - start) / duration));
        }

        private static double[] EvenTimes(T[] points, double duration) {
            if (points == null) throw new ArgumentNullException(nameof(points));
            double[] times = new double[points.Length];
            if (times.Length == 1) times[0] = duration;
            for (int i = 1; i < times.Length; i++) {
                times[i] = duration * i / (times.Length - 1);
            }
            return times;
        }

        /// <summary>
        /// The velocity at each point goes from the point before to the point after. The first and last points
        /// count themselves as the point before or after.
        /// </summary>
        private static T[] Velocities(T[] points, double[] times) {
            var subtract = TweenMath<T>.Subtract;
            var scale = TweenMath<T>.Scale;

            int last = points.Length - 1;
            T[] velocities = new T[points.Length];
            for (int i = 0; i <= last; i++) {
                int before = Math.Max(i - 1, 0);
                int after = Math.Min(i + 1, last);
                double span = times[after] - times[before];
                if (i == 0 || i == last) span *= 2;
                velocities[i] = span > 0 ? scale(subtract(points[after], points[before]), (float)(1 / span)) : scale(points[i], 0f);
            }
            return velocities;
        }

        private readonly T[] _points;
        private readonly double[] _times;
        private readonly T[] _velocities;
        private readonly EasingFunction _easing;
        private int _hint;
    }

    public static class SplineExtensions {
        /// <summary>
        /// Plays <paramref name="tween"/>, then goes from its end value through each of <paramref name="points"/>
        /// along a smooth curve. Each point to the next takes the same time.
        /// </summary>
        public static Tween<T> SplineTo<T>(this ITween<T> tween, T[] points, double duration, EasingFunction easing) {
            if (points == null) throw new ArgumentNullException(nameof(points));
            Tween<T> head = Tween<T>.From(tween);
            T[] all = new T[points.Length + 1];
            all[0] = head.B;
            Array.Copy(points, 0, all, 1, points.Length);
            return head.AppendTween(new SplineTween<T>(all, duration, easing));
        }
    }
}
