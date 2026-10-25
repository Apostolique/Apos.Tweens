using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public class FloatTween : Tween<float> {
        public FloatTween(float a, float b, double duration, Interpolator easing, TweenClock clock = null) : base(a, b, duration, easing, MathHelper.Lerp, clock) { }
        public FloatTween(float a, float b, double duration, Interpolator easing, double startTime, TweenClock clock = null) : base(a, b, duration, easing, MathHelper.Lerp, startTime, clock) { }
    }

    public static class FloatTweenExtensions {
        /// <summary>Plays <paramref name="tween"/>, then goes from its end value to <paramref name="target"/>.</summary>
        public static ITween<float> To(this ITween<float> tween, float target, double duration, Interpolator easing) {
            return tween.Then(new FloatTween(tween.B, target, duration, easing));
        }
        /// <summary>Plays <paramref name="tween"/>, then moves its end value by <paramref name="offset"/>.</summary>
        public static ITween<float> Offset(this ITween<float> tween, float offset, double duration, Interpolator easing) {
            return tween.Then(new FloatTween(tween.B, tween.B + offset, duration, easing));
        }
    }
}
