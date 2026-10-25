using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public class Vector2Tween : Tween<Vector2> {
        public Vector2Tween(Vector2 a, Vector2 b, double duration, Interpolator easing, TweenClock clock = null) : base(a, b, duration, easing, Vector2.Lerp, clock) { }
        public Vector2Tween(Vector2 a, Vector2 b, double duration, Interpolator easing, double startTime, TweenClock clock = null) : base(a, b, duration, easing, Vector2.Lerp, startTime, clock) { }
    }

    public static class Vector2TweenExtensions {
        /// <summary>Plays <paramref name="tween"/>, then goes from its end value to <paramref name="target"/>.</summary>
        public static ITween<Vector2> To(this ITween<Vector2> tween, Vector2 target, double duration, Interpolator easing) {
            return tween.Then(new Vector2Tween(tween.B, target, duration, easing));
        }
        /// <summary>Plays <paramref name="tween"/>, then moves its end value by <paramref name="offset"/>.</summary>
        public static ITween<Vector2> Offset(this ITween<Vector2> tween, Vector2 offset, double duration, Interpolator easing) {
            return tween.Then(new Vector2Tween(tween.B, tween.B + offset, duration, easing));
        }
    }
}
