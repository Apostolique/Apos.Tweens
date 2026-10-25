namespace Apos.Tweens {
    /// <summary>Holds a value without changing it.</summary>
    public class WaitTween<T> : Tween<T> {
        /// <param name="value">The value to hold.</param>
        /// <param name="duration">The length in milliseconds.</param>
        /// <param name="lerp">The lerp that a <c>To</c> chained after it uses. Defaults to <see cref="TweenMath{T}.Lerp"/>.</param>
        public WaitTween(T value, double duration, Lerp<T> lerp = null) : base(new Step(value, value, null, lerp ?? TweenMath<T>.Lerp, null), duration) { }
    }
}
