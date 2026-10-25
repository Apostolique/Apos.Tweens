using System;
using Microsoft.Xna.Framework;

namespace Apos.Tweens {
    /// <summary>
    /// The math tweens use for <typeparamref name="T"/> when they aren't given any. It's already set for
    /// <c>float</c>, <c>Vector2</c>, <c>Vector3</c>, <c>Vector4</c>, <c>Color</c> and <c>Quaternion</c>.
    /// For other types, you can set it once when your game starts.
    /// </summary>
    public static class TweenMath<T> {
        /// <summary>Used by tweens that aren't given a lerp.</summary>
        public static Lerp<T> Lerp { get; set; } = (Lerp<T>)TweenMathDefaults.Lerp(typeof(T));
        /// <summary>Used by <c>Offset</c> to add the offset to the end value, and by splines and velocities.</summary>
        public static Func<T, T, T> Add { get; set; } = (Func<T, T, T>)TweenMathDefaults.Add(typeof(T));
        /// <summary>Used by splines and velocities. Returns the first value minus the second.</summary>
        public static Func<T, T, T> Subtract { get; set; } = (Func<T, T, T>)TweenMathDefaults.Subtract(typeof(T));
        /// <summary>Used by splines and velocities. Multiplies a value by a number.</summary>
        public static Func<T, float, T> Scale { get; set; } = (Func<T, float, T>)TweenMathDefaults.Scale(typeof(T));

        internal static InvalidOperationException NoLerp() {
            return new InvalidOperationException($"There's no lerp for {typeof(T).Name}. Pass one to the tween or set TweenMath<{typeof(T).Name}>.Lerp.");
        }
        internal static InvalidOperationException NoAdd() {
            return new InvalidOperationException($"Offset needs TweenMath<{typeof(T).Name}>.Add to be set.");
        }
        internal static void RequireVectorMath(string what) {
            if (Add == null || Subtract == null || Scale == null) {
                throw new InvalidOperationException($"{what} needs TweenMath<{typeof(T).Name}>.Add, Subtract and Scale to be set.");
            }
        }
    }

    internal static class TweenMathDefaults {
        public static Delegate Lerp(Type type) {
            if (type == typeof(float)) return (Lerp<float>)MathHelper.Lerp;
            if (type == typeof(Vector2)) return (Lerp<Vector2>)Vector2.Lerp;
            if (type == typeof(Vector3)) return (Lerp<Vector3>)Vector3.Lerp;
            if (type == typeof(Vector4)) return (Lerp<Vector4>)Vector4.Lerp;
            if (type == typeof(Color)) return (Lerp<Color>)Color.Lerp;
            if (type == typeof(Quaternion)) return (Lerp<Quaternion>)Quaternion.Slerp;
            return null;
        }
        public static Delegate Add(Type type) {
            if (type == typeof(float)) return (Func<float, float, float>)((a, b) => a + b);
            if (type == typeof(Vector2)) return (Func<Vector2, Vector2, Vector2>)((a, b) => a + b);
            if (type == typeof(Vector3)) return (Func<Vector3, Vector3, Vector3>)((a, b) => a + b);
            if (type == typeof(Vector4)) return (Func<Vector4, Vector4, Vector4>)((a, b) => a + b);
            return null;
        }
        public static Delegate Subtract(Type type) {
            if (type == typeof(float)) return (Func<float, float, float>)((a, b) => a - b);
            if (type == typeof(Vector2)) return (Func<Vector2, Vector2, Vector2>)((a, b) => a - b);
            if (type == typeof(Vector3)) return (Func<Vector3, Vector3, Vector3>)((a, b) => a - b);
            if (type == typeof(Vector4)) return (Func<Vector4, Vector4, Vector4>)((a, b) => a - b);
            return null;
        }
        public static Delegate Scale(Type type) {
            if (type == typeof(float)) return (Func<float, float, float>)((a, s) => a * s);
            if (type == typeof(Vector2)) return (Func<Vector2, float, Vector2>)((a, s) => a * s);
            if (type == typeof(Vector3)) return (Func<Vector3, float, Vector3>)((a, s) => a * s);
            if (type == typeof(Vector4)) return (Func<Vector4, float, Vector4>)((a, s) => a * s);
            return null;
        }
    }
}
