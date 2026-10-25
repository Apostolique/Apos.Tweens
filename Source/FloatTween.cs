using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public class FloatTween : Tween<float> {
        public FloatTween(float a, float b, double duration, Interpolator easing) : base(a, b, duration, easing, MathHelper.Lerp) { }
    }
}
