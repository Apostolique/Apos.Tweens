using System;
using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public class Vector2Tween : ITween<Vector2> {
        public Vector2Tween(Vector2 a, Vector2 b, long duration, Interpolator interpolator) {
            A = a;
            B = b;
            StartTime = TweenHelper.TotalMS;
            Duration = duration;
            Interpolator = interpolator;
        }

        public Vector2 A { get; set; }
        public Vector2 B { get; set; }
        public long StartTime { get; set; }
        public long Duration { get; set; }
        public Interpolator Interpolator { get; set; }

        public Vector2 Value => ValueAt(TweenHelper.TotalMS - StartTime);
        public Vector2 ValueAt(long ms) {
            if (ms < 0f) return A;
            else if (ms >= Duration) return B;

            return A + (B - A) * Interpolator(ms / (float)Duration);
        }
    }

    public static class Vector2TweenExtensions {
        public static ITween<Vector2> To(this ITween<Vector2> tween, Vector2 target, long duration, Interpolator interpolator) {
            return tween.Then(new Vector2Tween(tween.B, target, duration, interpolator));
        }
        public static ITween<Vector2> Offset(this ITween<Vector2> tween, Vector2 offset, long duration, Interpolator interpolator) {
            return tween.Then(new Vector2Tween(tween.B, tween.B + offset, duration, interpolator));
        }
    }
}
