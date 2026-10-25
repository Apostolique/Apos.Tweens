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
            return tween.Then(NextTween<Color>.To(tween, target, duration, easing, Color.Lerp));
        }
        /// <summary>
        /// Plays <paramref name="tween"/>, then adds <paramref name="offset"/> to its end value. The offset is
        /// in the 0 to 1 range of <see cref="Color.ToVector4"/> and can be negative. The result is clamped.
        /// </summary>
        public static ITween<Color> Offset(this ITween<Color> tween, Vector4 offset, double duration, Interpolator easing) {
            return tween.Then(NextTween<Color>.Offset(tween, b => Add(b, offset), duration, easing, Color.Lerp));
        }

        private static Color Add(Color color, Vector4 offset) {
            Vector4 v = (color.ToVector4() + offset) * 255f;
            return new Color((int)MathF.Round(v.X), (int)MathF.Round(v.Y), (int)MathF.Round(v.Z), (int)MathF.Round(v.W));
        }
    }
}
