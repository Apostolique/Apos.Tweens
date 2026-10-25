namespace Apos.Tweens {
    /// <summary>
    /// A value that changes over time. A tween holds no playback state, so many <see cref="TweenPlayer{T}"/>
    /// can share one, each with its own start time and clock.
    /// </summary>
    public interface ITween<T> {
        /// <summary>The length in milliseconds. A tween that loops forever is <see cref="double.PositiveInfinity"/>.</summary>
        double Duration { get; }

        /// <summary>
        /// The value <paramref name="ms"/> milliseconds after the start. Before the start, it's the first
        /// value. After the end, it's the last one.
        /// </summary>
        T ValueAt(double ms);
    }
}
