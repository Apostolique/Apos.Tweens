using System;

namespace Apos.Tweens {
    /// <summary>Plays a tween on its own time. Each millisecond of this tween reads the other one at a remapped time.</summary>
    public class RemapTween<T> : ITween<T> {
        /// <param name="tween">The tween to read.</param>
        /// <param name="duration">The length in milliseconds.</param>
        /// <param name="time">Takes milliseconds from 0 to <paramref name="duration"/> and returns when to read <paramref name="tween"/>.</param>
        public RemapTween(ITween<T> tween, double duration, Func<double, double> time) {
            _tween = tween ?? throw new ArgumentNullException(nameof(tween));
            _time = time ?? throw new ArgumentNullException(nameof(time));
            Duration = duration;
        }

        public double Duration { get; }

        public T ValueAt(double ms) {
            return _tween.ValueAt(_time(Math.Clamp(ms, 0, Duration)));
        }

        protected ITween<T> _tween;
        private readonly Func<double, double> _time;
    }

    /// <summary>Plays a tween at <c>scale</c> times its speed, starting from <c>offset</c> milliseconds into it.</summary>
    internal sealed class LinearRemapTween<T>(ITween<T> tween, double duration, double scale, double offset) : ITween<T> {
        public double Duration { get; } = duration;

        public T ValueAt(double ms) {
            return tween.ValueAt(Math.Clamp(ms, 0, Duration) * scale + offset);
        }
    }

    public static class RemapExtensions {
        /// <summary>
        /// Plays <paramref name="tween"/> on its own time. <paramref name="time"/> takes milliseconds from 0 to
        /// <paramref name="duration"/> and returns when to read <paramref name="tween"/>.
        /// </summary>
        public static ITween<T> Remap<T>(this ITween<T> tween, double duration, Func<double, double> time) {
            return new RemapTween<T>(tween, duration, time);
        }
        /// <summary>Plays the part of <paramref name="tween"/> from <paramref name="start"/> to <paramref name="end"/> milliseconds.</summary>
        public static ITween<T> Slice<T>(this ITween<T> tween, double start, double end) {
            if (tween == null) throw new ArgumentNullException(nameof(tween));
            if (end < start) throw new ArgumentException("The end needs to come after the start.", nameof(end));
            return new LinearRemapTween<T>(tween, end - start, 1, start);
        }
        /// <summary>Plays <paramref name="tween"/> <paramref name="speed"/> times as fast. 2 is twice as fast, 0.5 is half as fast.</summary>
        public static ITween<T> Speed<T>(this ITween<T> tween, double speed) {
            if (tween == null) throw new ArgumentNullException(nameof(tween));
            if (!(speed > 0)) throw new ArgumentOutOfRangeException(nameof(speed), "The speed needs to be more than 0.");
            return new LinearRemapTween<T>(tween, tween.Duration / speed, speed, 0);
        }
        /// <summary>Holds the start value of <paramref name="tween"/> for <paramref name="delay"/> milliseconds, then plays it.</summary>
        public static ITween<T> Delay<T>(this ITween<T> tween, double delay) {
            if (tween == null) throw new ArgumentNullException(nameof(tween));
            if (!(delay >= 0)) throw new ArgumentOutOfRangeException(nameof(delay), "The delay can't be negative.");
            return new LinearRemapTween<T>(tween, delay + tween.Duration, 1, -delay);
        }
    }
}
