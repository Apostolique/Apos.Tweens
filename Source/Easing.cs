using System;
using Microsoft.Xna.Framework;

// Based on: https://github.com/nicolausYes/easing-functions

namespace Apos.Tweens {
    public delegate float Interpolator(float x);

    public static class Easing {
        public static float Linear(float x) => x;

        public static float QuadIn(float x) => x * x;
        public static float QuadOut(float x) => x * (2 - x);
        public static float QuadInOut(float x) =>
            x < 0.5f
                ? 2f * x * x
                : x * (4f - 2f * x) - 1f;

        public static float CubeIn(float x) => x * x * x;
        public static float CubeOut(float x) => 1f + --x * x * x;
        public static float CubeInOut(float x) =>
            x < 0.5f
                ? 4f * x * x * x
                : 4f * (x - 1f) * (x - 1f) * (x - 1f) + 1f;

        public static float QuartIn(float x) {
            x *= x;
            return x * x;
        }
        public static float QuartOut(float x) {
            x = --x * x;
            return 1f - x * x;
        }

        public static float QuintIn(float x) {
            float x2 = x * x;
            return x * x2 * x2;
        }
        public static float QuintOut(float x) {
            float x2 = --x * x;
            return 1f + x * x2 * x2;
        }
        public static float QuintInOut(float x) {
            float x2;
            if (x < 0.5f) {
                x2 = x * x;
                return 16f * x * x2 * x2;
            } else {
                x2 = --x * x;
                return 1f + 16f * x * x2 * x2;
            }
        }

        public static float CircIn(float x) => 1f - (float)Math.Sqrt(1f - x);
        public static float CircOut(float x) => (float)Math.Sqrt(x);
        public static float CircInOut(float x) =>
            x < 0.5f
                ? (1f - (float)Math.Sqrt(1f - 2f * x)) * 0.5f
                : (1f + (float)Math.Sqrt(2f * x - 1f)) * 0.5f;

        public static float SineIn(float x) => 1f + (float)Math.Sin(1.5707963f * (x - 1f));
        public static float SineOut(float x) => (float)Math.Sin(1.5707963f * x);
        public static float SineInOut(float x) => 0.5f * (1f + (float)Math.Sin(3.1415926f * (x - 0.5f)));

        public static float ExpoIn(float x) => ((float)Math.Pow(2f, 8f * x) - 1f) / 255f;
        public static float ExpoOut(float x) => 1f - (float)Math.Pow(2f, -8f * x);
        public static float ExpoInOut(float x) =>
            x < 0.5f
                ? ((float)Math.Pow(2f, 16f * x) - 1f) / 510f
                : 1f - 0.5f * (float)Math.Pow(2f, -16f * (x - 0.5f));

        public static float BackIn(float x) => x * x * (2.70158f * x - 1.70158f);
        public static float BackOut(float x) => 1f + (--x) * x * (2.70158f * x + 1.70158f);
        public static float BackInOut(float x) =>
            x < 0.5f
                ? x * x * (7f * x - 2.5f) * 2f
                : 1 + (--x) * x * 2f * (7f * x + 2.5f);

        public static float ElasticIn(float x) {
            float x2 = x * x;
            return x2 * x2 * (float)Math.Sin(x * MathHelper.Pi * 4.5f);
        }
        public static float ElasticOut(float x) {
            float x2 = (x - 1f) * (x - 1f);
            return 1f - x2 * x2 * (float)Math.Cos(x * MathHelper.Pi * 4.5f);
        }
        public static float ElasticInOut(float x) {
            float x2;
            if (x < 0.45) {
                x2 = x * x;
                return 8f * x2 * x2 * (float)Math.Sin(x * 28.27433466f);
            } else if (x < 0.55) {
                return 0.5f + 0.75f * (float)Math.Sin(x * 12.56637096f);
            } else {
                x2 = (x - 1f) * (x - 1f);
                return 1f - 8f * x2 * x2 * (float)Math.Sin(x * 28.27433466f);
            }
        }

        public static float BounceIn(float x) => (float)Math.Pow(2f, 6f * (x - 1f)) * (float)Math.Abs((float)Math.Sin(x * MathHelper.Pi * 3.5f));
        public static float BounceOut(float x) => 1f - (float)Math.Pow(2f, -6f * x) * (float)Math.Abs((float)Math.Cos(x * MathHelper.Pi * 3.5f));
        public static float BounceInOut(float x) =>
            x < 0.5
                ? 8f * (float)Math.Pow(2f, 8f * (x - 1f)) * (float)Math.Abs((float)Math.Sin(x * MathHelper.Pi * 7f))
                : 1f - 8f * (float)Math.Pow(2f, -8f * x) * (float)Math.Abs((float)Math.Sin(x * MathHelper.Pi * 7f));
    }
}
