using System;
using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public class ColorTween : Tween<Color> {
        public ColorTween(Color a, Color b, double duration, Interpolator easing, TweenClock clock = null) : base(a, b, duration, easing, Color.Lerp, clock) { }
        public ColorTween(Color a, Color b, double duration, Interpolator easing, double startTime, TweenClock clock = null) : base(a, b, duration, easing, Color.Lerp, startTime, clock) { }
    }

    public static class ColorTweenExtensions {
        /// <summary>Plays <paramref name="tween"/>, then goes from its end value to <paramref name="target"/>.</summary>
        public static ITween<Color> To(this ITween<Color> tween, Color target, double duration, Interpolator easing) {
            return tween.Then(new ColorTween(tween.B, target, duration, easing));
        }
        /// <summary>
        /// Plays <paramref name="tween"/>, then adds <paramref name="offset"/> to its end value. The offset is
        /// in the 0 to 1 range of <see cref="Color.ToVector4"/> and can be negative. The result is clamped.
        /// </summary>
        public static ITween<Color> Offset(this ITween<Color> tween, Vector4 offset, double duration, Interpolator easing) {
            Vector4 b = (tween.B.ToVector4() + offset) * 255f;
            return tween.Then(new ColorTween(tween.B, new Color((int)MathF.Round(b.X), (int)MathF.Round(b.Y), (int)MathF.Round(b.Z), (int)MathF.Round(b.W)), duration, easing));
        }
    }
}
