namespace Apos.Tweens {
    /// <summary>Plays tweens one after the other.</summary>
    public class ThenTween<T> : ITween<T> {
        /// <summary>Plays <paramref name="tween1"/>, then <paramref name="tween2"/>.</summary>
        public ThenTween(ITween<T> tween1, ITween<T> tween2) {
            ITween<T>[] first = tween1 is ThenTween<T> then1 ? then1._tweens : [tween1];
            ITween<T>[] second = tween2 is ThenTween<T> then2 ? then2._tweens : [tween2];
            _tweens = [.. first, .. second];
        }

        public T A => _tweens[0].A;
        public T B => _tweens[^1].B;
        public double StartTime {
            get => _tweens[0].StartTime;
            set => _tweens[0].StartTime = value;
        }
        public double Duration {
            get {
                double duration = 0;
                foreach (ITween<T> tween in _tweens) {
                    duration += tween.Duration;
                }
                return duration;
            }
        }
        public TweenClock Clock {
            get => _tweens[0].Clock;
            set => _tweens[0].Clock = value;
        }

        public T Value => ValueAt(Clock.TotalMS - StartTime);
        public T ValueAt(double ms) {
            int last = _tweens.Length - 1;
            for (int i = 0; i < last; i++) {
                double duration = _tweens[i].Duration;
                if (ms <= duration) return _tweens[i].ValueAt(ms);
                ms -= duration;
            }

            return _tweens[last].ValueAt(ms);
        }

        private readonly ITween<T>[] _tweens;
    }

    public static class ThenExtensions {
        /// <summary>Plays <paramref name="tween1"/>, then <paramref name="tween2"/>.</summary>
        public static ITween<T> Then<T>(this ITween<T> tween1, ITween<T> tween2) {
            return new ThenTween<T>(tween1, tween2);
        }
    }
}
