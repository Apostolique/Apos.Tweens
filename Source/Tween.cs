using System;

namespace Apos.Tweens {
    /// <summary>Returns the value at <paramref name="t"/> between <paramref name="a"/> (0) and <paramref name="b"/> (1).</summary>
    public delegate T Lerp<T>(T a, T b, float t);

    /// <summary>
    /// Goes from <see cref="A"/> to <see cref="B"/> over <see cref="Duration"/>, then through each step chained
    /// after it with <c>To</c>, <c>Offset</c>, <c>Wait</c> and <c>Then</c>. A tween never changes once it's
    /// created. Chaining returns a new tween and leaves the old one as it was.
    /// </summary>
    public class Tween<T> : ITween<T> {
        /// <param name="a">The value at the start.</param>
        /// <param name="b">The value at the end.</param>
        /// <param name="duration">The length in milliseconds.</param>
        /// <param name="easing">The shape of the motion, usually one of the functions in <see cref="Easing"/>.</param>
        /// <param name="lerp">How to go between two values. Defaults to <see cref="TweenMath{T}.Lerp"/>.</param>
        public Tween(T a, T b, double duration, EasingFunction easing, Lerp<T> lerp = null)
            : this(new Step(a, b, easing ?? throw new ArgumentNullException(nameof(easing)), lerp ?? TweenMath<T>.Lerp ?? throw TweenMath<T>.NoLerp(), null), duration) { }

        internal Tween(Step step, double duration) {
            _storage = new Storage(4);
            _storage.Ends[0] = duration;
            _storage.Steps[0] = step;
            _storage.Count = 1;
            _count = 1;
        }
        private Tween(Storage storage, int count) {
            _storage = storage;
            _count = count;
        }

        /// <summary>The value at the start.</summary>
        public T A => _storage.Steps[0].From;
        /// <summary>The value at the end.</summary>
        public T B => _storage.Steps[_count - 1].To;
        public double Duration => _storage.Ends[_count - 1];

        public T ValueAt(double ms) {
            double[] ends = _storage.Ends;
            int i = StepSearch.Find(ends, _count, ref _hint, ms);
            ref Step step = ref _storage.Steps[i];

            double start = i == 0 ? 0 : ends[i - 1];
            ms -= start;
            if (step.Tween != null) return step.Tween.ValueAt(ms);
            if (step.Easing == null || ms <= 0) return step.From;

            double duration = ends[i] - start;
            if (ms >= duration) return step.To;

            return step.Lerp(step.From, step.To, step.Easing((float)(ms / duration)));
        }

        internal Tween<T> AppendTo(T target, double duration, EasingFunction easing, Lerp<T> lerp) {
            if (easing == null) throw new ArgumentNullException(nameof(easing));
            lerp ??= LastLerp ?? throw TweenMath<T>.NoLerp();
            return Append(new Step(B, target, easing, lerp, null), duration);
        }
        internal Tween<T> AppendWait(double duration) {
            T b = B;
            return Append(new Step(b, b, null, LastLerp, null), duration);
        }
        internal Tween<T> AppendTween(ITween<T> tween) {
            if (tween is Tween<T> other) return AppendAll(other);
            return Append(Step.Play(tween, LastLerp), tween.Duration);
        }
        /// <summary>The lerp that a <c>To</c> chained after this tween uses.</summary>
        internal Lerp<T> LastLerp => _storage.Steps[_count - 1].Lerp;

        internal static Tween<T> From(ITween<T> tween) {
            return tween as Tween<T> ?? new Tween<T>(Step.Play(tween, TweenMath<T>.Lerp), tween.Duration);
        }

        private Tween<T> Append(Step step, double duration) {
            Storage storage = Reserve(1);
            storage.Ends[_count] = Duration + duration;
            storage.Steps[_count] = step;
            return new Tween<T>(storage, _count + 1);
        }
        private Tween<T> AppendAll(Tween<T> other) {
            Storage storage = Reserve(other._count);
            double offset = Duration;
            for (int i = 0; i < other._count; i++) {
                storage.Ends[_count + i] = offset + other._storage.Ends[i];
            }
            Array.Copy(other._storage.Steps, 0, storage.Steps, _count, other._count);
            return new Tween<T>(storage, _count + other._count);
        }

        /// <summary>
        /// Makes room for <paramref name="extra"/> steps after this tween's last one. Only the newest tween on a
        /// storage can grow it, so an older one copies its steps instead. That way no tween ever sees a step
        /// change.
        /// </summary>
        private Storage Reserve(int extra) {
            int count = _count + extra;
            lock (_storage) {
                if (_storage.Count == _count) {
                    if (count > _storage.Ends.Length) {
                        int capacity = Math.Max(count, _storage.Ends.Length * 2);
                        Array.Resize(ref _storage.Ends, capacity);
                        Array.Resize(ref _storage.Steps, capacity);
                    }
                    _storage.Count = count;
                    return _storage;
                }
            }

            Storage copy = new Storage(Math.Max(4, count * 2));
            Array.Copy(_storage.Ends, copy.Ends, _count);
            Array.Copy(_storage.Steps, copy.Steps, _count);
            copy.Count = count;
            return copy;
        }

        /// <summary>
        /// Goes from <see cref="From"/> to <see cref="To"/>. Holds <see cref="From"/> when there's no easing.
        /// Plays <see cref="Tween"/> instead when there's one. <see cref="Lerp"/> is also what the next <c>To</c> uses.
        /// </summary>
        internal readonly struct Step(T from, T to, EasingFunction easing, Lerp<T> lerp, ITween<T> tween) {
            public readonly T From = from;
            public readonly T To = to;
            public readonly EasingFunction Easing = easing;
            public readonly Lerp<T> Lerp = lerp;
            public readonly ITween<T> Tween = tween;

            public static Step Play(ITween<T> tween, Lerp<T> lerp) {
                return new Step(tween.ValueAt(0), tween.ValueAt(tween.Duration), null, lerp, tween);
            }
        }

        /// <summary>Steps shared by a tween and the tweens chained from it. Ends are in milliseconds from the start.</summary>
        private sealed class Storage(int capacity) {
            public double[] Ends = new double[capacity];
            public Step[] Steps = new Step[capacity];
            public int Count;
        }

        private readonly Storage _storage;
        private readonly int _count;
        private int _hint;
    }

    public static class TweenExtensions {
        /// <summary>
        /// Plays <paramref name="tween"/>, then goes from its end value to <paramref name="target"/>. Without a
        /// <paramref name="lerp"/>, it uses the same one as the step before.
        /// </summary>
        public static Tween<T> To<T>(this ITween<T> tween, T target, double duration, EasingFunction easing, Lerp<T> lerp = null) {
            return Tween<T>.From(tween).AppendTo(target, duration, easing, lerp);
        }
        /// <summary>Plays <paramref name="tween"/>, then moves its end value by <paramref name="offset"/> with <see cref="TweenMath{T}.Add"/>.</summary>
        public static Tween<T> Offset<T>(this ITween<T> tween, T offset, double duration, EasingFunction easing) {
            Func<T, T, T> add = TweenMath<T>.Add ?? throw TweenMath<T>.NoAdd();
            Tween<T> head = Tween<T>.From(tween);
            return head.AppendTo(add(head.B, offset), duration, easing, null);
        }
        /// <summary>Plays <paramref name="tween"/>, then holds its end value for <paramref name="duration"/> milliseconds.</summary>
        public static Tween<T> Wait<T>(this ITween<T> tween, double duration) {
            return Tween<T>.From(tween).AppendWait(duration);
        }
        /// <summary>Plays <paramref name="tween1"/>, then <paramref name="tween2"/>.</summary>
        public static Tween<T> Then<T>(this ITween<T> tween1, ITween<T> tween2) {
            return Tween<T>.From(tween1).AppendTween(tween2);
        }
    }
}
