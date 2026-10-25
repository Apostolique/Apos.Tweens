using System;

namespace Apos.Tweens {
    /// <summary>
    /// Starts where <c>previous</c> ends, read every time so it follows changes to it. It ends on a target,
    /// or relative to its start with an offset. Without a lerp, it holds its start.
    /// </summary>
    internal sealed class NextTween<T> : ITween<T> {
        public static NextTween<T> To(ITween<T> previous, T target, double duration, Interpolator easing, Lerp<T> lerp) {
            return new NextTween<T>(previous, target, null, duration, easing, lerp);
        }
        public static NextTween<T> Offset(ITween<T> previous, Func<T, T> offset, double duration, Interpolator easing, Lerp<T> lerp) {
            return new NextTween<T>(previous, default, offset, duration, easing, lerp);
        }
        public static NextTween<T> Wait(ITween<T> previous, double duration) {
            return new NextTween<T>(previous, default, a => a, duration, null, null);
        }

        private NextTween(ITween<T> previous, T target, Func<T, T> offset, double duration, Interpolator easing, Lerp<T> lerp) {
            _previous = previous;
            _target = target;
            _offset = offset;
            Duration = duration;
            _easing = easing;
            _lerp = lerp;
            Clock = previous.Clock;
            StartTime = previous.StartTime;
        }

        public T A => _previous.B;
        public T B => _offset == null ? _target : _offset(_previous.B);
        public double StartTime { get; set; }
        public double Duration { get; }
        public TweenClock Clock { get; set; }

        public T Value => ValueAt(Clock.TotalMS - StartTime);
        public T ValueAt(double ms) {
            T a = A;
            if (_lerp == null || ms <= 0) return a;

            T b = _offset == null ? _target : _offset(a);
            if (ms >= Duration) return b;

            return _lerp(a, b, _easing((float)(ms / Duration)));
        }

        private readonly ITween<T> _previous;
        private readonly T _target;
        private readonly Func<T, T> _offset;
        private readonly Interpolator _easing;
        private readonly Lerp<T> _lerp;
    }
}
