namespace Apos.Tweens {
    internal static class StepSearch {
        /// <summary>
        /// Finds the step that plays at <paramref name="ms"/>, where step i ends at <paramref name="ends"/>[i].
        /// Time mostly moves forward, so the last step found and the one after it are checked before searching.
        /// </summary>
        public static int Find(double[] ends, int count, ref int hint, double ms) {
            int last = count - 1;
            int h = hint;
            if (Contains(ends, h, last, ms)) return h;
            if (h < last && Contains(ends, h + 1, last, ms)) {
                hint = h + 1;
                return h + 1;
            }

            int low = 0;
            int high = last;
            while (low < high) {
                int mid = low + (high - low) / 2;
                if (ms <= ends[mid]) high = mid;
                else low = mid + 1;
            }
            hint = low;
            return low;
        }
        private static bool Contains(double[] ends, int i, int last, double ms) {
            return (i == 0 || ms > ends[i - 1]) && (i == last || ms <= ends[i]);
        }
    }
}
