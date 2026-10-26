using System;
using System.Collections.Generic;

namespace Apos.Tweens {
    /// <summary>
    /// A value that eases toward a target that can change at any time, like a scroll offset or a hover glow.
    /// Changing the target doesn't allocate.
    /// </summary>
    public class EasedValue<T> {
        /// <param name="value">The value it starts on, which is also its first target.</param>
        /// <param name="duration">How long each move takes in milliseconds.</param>
        /// <param name="easing">The shape of each move, usually one of the functions in <see cref="Apos.Tweens.Easing"/>.</param>
        /// <param name="clock">The clock it reads its time from. Defaults to <see cref="TweenHelper.Clock"/>.</param>
        /// <param name="lerp">How to go between two values. Defaults to <see cref="TweenMath{T}.Lerp"/>.</param>
        public EasedValue(T value, double duration, EasingFunction easing, TweenClock clock = null, Lerp<T> lerp = null)
            : this(value, (_, _) => duration, easing, clock, lerp) { }
        /// <param name="value">The value it starts on, which is also its first target.</param>
        /// <param name="duration">Takes the current value and the new target, and returns how long the move takes in milliseconds.</param>
        /// <param name="easing">The shape of each move, usually one of the functions in <see cref="Apos.Tweens.Easing"/>.</param>
        /// <param name="clock">The clock it reads its time from. Defaults to <see cref="TweenHelper.Clock"/>.</param>
        /// <param name="lerp">How to go between two values. Defaults to <see cref="TweenMath{T}.Lerp"/>.</param>
        public EasedValue(T value, Func<T, T, double> duration, EasingFunction easing, TweenClock clock = null, Lerp<T> lerp = null) {
            Duration = duration ?? throw new ArgumentNullException(nameof(duration));
            Easing = easing ?? throw new ArgumentNullException(nameof(easing));
            Clock = clock ?? TweenHelper.Clock;
            _lerp = lerp ?? TweenMath<T>.Lerp ?? throw TweenMath<T>.NoLerp();
            _move = new Move(this);
            Snap(value);
        }

        /// <summary>The value at the current time of <see cref="Clock"/>.</summary>
        public T Value => ValueAt(Elapsed);
        /// <summary>
        /// Where the value is going. Setting it starts a move from <see cref="Value"/>, using <see cref="Duration"/>
        /// and <see cref="Easing"/>. Setting it to the target it already has does nothing.
        /// </summary>
        public T Target {
            get => _to;
            set {
                if (EqualityComparer<T>.Default.Equals(value, _to)) return;

                double elapsed = Elapsed;
                T from = ValueAt(elapsed);
                bool smooth = Smooth;
                T velocity = smooth ? _move.VelocityAt(elapsed) : default;

                _from = from;
                _to = value;
                _duration = Duration(from, value);
                _easing = Easing;
                _smooth = smooth;
                _kick = smooth ? RetargetTween<T>.Kick(velocity, _duration) : default;
                _start = Clock.TotalMS;
            }
        }
        /// <summary>Jumps to <paramref name="value"/> right away. <see cref="Value"/> and <see cref="Target"/> both become it.</summary>
        public void Snap(T value) {
            _from = value;
            _to = value;
            _duration = 0;
            _smooth = false;
            _kick = default;
            _start = Clock.TotalMS;
        }
        /// <summary>True once <see cref="Value"/> has reached <see cref="Target"/>.</summary>
        public bool IsDone => Elapsed >= _duration;
        /// <summary>
        /// How fast <see cref="Value"/> changes, in units per millisecond. It needs <see cref="TweenMath{T}.Subtract"/>
        /// and <see cref="TweenMath{T}.Scale"/>.
        /// </summary>
        public T Velocity => _move.VelocityAt(Elapsed);

        /// <summary>
        /// Takes the current value and the new target, and returns how long the move takes in milliseconds.
        /// Changing it applies to the next move.
        /// </summary>
        public Func<T, T, double> Duration { get; set; }
        /// <summary>The shape of each move. Changing it applies to the next move.</summary>
        public EasingFunction Easing { get; set; }
        /// <summary>
        /// When true, a move keeps the velocity the value had when the target changed and lets it fade out,
        /// so the value curves toward the new target instead of turning a sharp corner. With an easing that
        /// starts at rest, like <see cref="Apos.Tweens.Easing.SineInOut"/>, the speed doesn't jump at all. It
        /// needs <see cref="TweenMath{T}.Add"/>, <see cref="TweenMath{T}.Subtract"/> and <see cref="TweenMath{T}.Scale"/>.
        /// </summary>
        public bool Smooth {
            get => _smoothNext;
            set {
                if (value) TweenMath<T>.RequireVectorMath(nameof(Smooth));
                _smoothNext = value;
            }
        }
        /// <summary>The clock that drives <see cref="Value"/>.</summary>
        public TweenClock Clock { get; set; }

        private double Elapsed => Clock.TotalMS - _start;

        private T ValueAt(double elapsed) {
            if (elapsed >= _duration) return _to;
            if (elapsed <= 0) return _from;

            float t = (float)(elapsed / _duration);
            if (_smooth) return RetargetTween<T>.Eval(_from, _to, _kick, _easing, _lerp, t);
            return _lerp(_from, _to, _easing(t));
        }

        /// <summary>The current move seen as a tween, so velocities can be read from it.</summary>
        private sealed class Move(EasedValue<T> owner) : ITween<T> {
            public double Duration => owner._duration;
            public T ValueAt(double ms) => owner.ValueAt(ms);
        }

        private readonly Lerp<T> _lerp;
        private readonly Move _move;
        private bool _smoothNext;

        private T _from;
        private T _to;
        private double _start;
        private double _duration;
        private EasingFunction _easing;
        private bool _smooth;
        private T _kick;
    }
}
