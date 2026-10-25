namespace Apos.Tweens {
    /// <summary>
    /// A value that changes over time. A tween keeps no state between frames, its value is computed
    /// from the time on its clock.
    /// </summary>
    public interface ITween<T> {
        /// <summary>The value at the start.</summary>
        T A { get; }
        /// <summary>The value at the end.</summary>
        T B { get; }
        /// <summary>
        /// When the tween starts, in milliseconds on its <see cref="Clock"/>. On a tween made from
        /// others, this is the start time of the first one.
        /// </summary>
        double StartTime { get; set; }
        /// <summary>The length in milliseconds. A tween that loops forever is <see cref="double.PositiveInfinity"/>.</summary>
        double Duration { get; }
        /// <summary>The clock that drives <see cref="Value"/>.</summary>
        TweenClock Clock { get; set; }

        /// <summary>The value at the current time of <see cref="Clock"/>.</summary>
        T Value { get; }
        /// <summary>The value <paramref name="ms"/> milliseconds after the start.</summary>
        T ValueAt(double ms);
    }
}
