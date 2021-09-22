using System;

namespace Apos.Tweens {
    public class FloatTween : ITween<float> {
        public FloatTween(float a, float b, long duration, Interpolator interpolator) {
            A = a;
            B = b;
            StartTime = TweenHelper.TotalMS;
            Duration = duration;
            Interpolator = interpolator;
        }

        public float A { get; set; }
        public float B { get; set; }
        public long StartTime { get; set; }
        public long Duration { get; set; }
        public Interpolator Interpolator { get; set; }

        public float Value => ValueAt(TweenHelper.TotalMS - StartTime);
        public float ValueAt(long ms) {
            if (ms < 0f) return A;
            else if (ms >= Duration) return B;

            return A + (B - A) * Interpolator(ms / (float)Duration);
        }
    }

    public static class FloatTweenExtensions {
        public static ITween<float> To(this ITween<float> tween, float target, long duration, Interpolator interpolator) {
            return tween.Then(new FloatTween(tween.B, target, duration, interpolator));
        }
        public static ITween<float> Offset(this ITween<float> tween, float offset, long duration, Interpolator interpolator) {
            return tween.Then(new FloatTween(tween.B, tween.B + offset, duration, interpolator));
        }
    }
}
