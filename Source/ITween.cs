namespace Apos.Tweens {
    public interface ITween<T> {
        T A { get; }
        T B { get; }
        long StartTime { get; }
        long Duration { get; }

        T Value { get; }
        T ValueAt(long ms);
    }
}
