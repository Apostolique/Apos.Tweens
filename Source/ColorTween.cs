using System;
using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public class ColorTween : Tween<Color> {
        public ColorTween(Color a, Color b, double duration, EasingFunction easing) : base(a, b, duration, easing, Color.Lerp) { }
    }

    public static class ColorTweenExtensions {
        /// <summary>
        /// Plays <paramref name="tween"/>, then adds <paramref name="offset"/> to its end value. The offset is
        /// in the 0 to 1 range of <see cref="Color.ToVector4"/> and can be negative. The result is clamped.
        /// </summary>
        public static Tween<Color> Offset(this ITween<Color> tween, Vector4 offset, double duration, EasingFunction easing) {
            Tween<Color> head = Tween<Color>.From(tween);
            return head.AppendTo(Add(head.B, offset), duration, easing, null);
        }

        private static Color Add(Color color, Vector4 offset) {
            Vector4 v = (color.ToVector4() + offset) * 255f;
            return new Color((int)MathF.Round(v.X), (int)MathF.Round(v.Y), (int)MathF.Round(v.Z), (int)MathF.Round(v.W));
        }
    }
}
