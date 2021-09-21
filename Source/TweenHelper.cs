namespace Apos.Tweens {
    public static class TweenHelper {
        public static void UpdateSetup(long totalMS) {
            TotalMS = totalMS;
        }

        public static long TotalMS { get; set; }
    }
}
