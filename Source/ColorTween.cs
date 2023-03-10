using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    public class ColorTween : ITween<Color> {
        public ColorTween(Color a, Color b, long duration, Interpolator interpolator) {
            A = a;
            B = b;
            StartTime = TweenHelper.TotalMS;
            Duration = duration;
            Interpolator = interpolator;
        }
        public ColorTween(Color a, Color b, long duration, Interpolator interpolator, long startTime) {
            A = a;
            B = b;
            StartTime = startTime;
            Duration = duration;
            Interpolator = interpolator;
        }

        public Color A { get; set; }
        public Color B { get; set; }
        public long StartTime { get; set; }
        public long Duration { get; set; }
        public Interpolator Interpolator { get; set; }

        public Color Value => ValueAt(TweenHelper.TotalMS - StartTime);
        public Color ValueAt(long ms) {
            if (ms <= 0f) return A;
            else if (ms >= Duration) return B;

            return Color.Lerp(A, B, Interpolator(ms / (float)Duration));
        }
    }

    public static class ColorTweenExtensions {
        public static ITween<Color> To(this ITween<Color> tween, Color target, long duration, Interpolator interpolator) {
            return tween.Then(new ColorTween(tween.B, target, duration, interpolator));
        }
        public static ITween<Color> Offset(this ITween<Color> tween, Color offset, long duration, Interpolator interpolator) {
            return tween.Then(new ColorTween(tween.B, new Color(tween.B.R + offset.R, tween.B.G + offset.G, tween.B.B + offset.B, tween.B.A + offset.A), duration, interpolator));
        }
    }
}
