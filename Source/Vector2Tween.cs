using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public class Vector2Tween : Tween<Vector2> {
        public Vector2Tween(Vector2 a, Vector2 b, double duration, Interpolator easing) : base(a, b, duration, easing, Vector2.Lerp) { }
    }
}
