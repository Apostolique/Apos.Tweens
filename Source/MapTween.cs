using System;

namespace Apos.Tweens {
    /// <summary>Turns each value of a tween into another value, which can be of another type.</summary>
    public class MapTween<T, TResult> : ITween<TResult> {
        public MapTween(ITween<T> tween, Func<T, TResult> map) {
            _tween = tween ?? throw new ArgumentNullException(nameof(tween));
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        public double Duration => _tween.Duration;

        public TResult ValueAt(double ms) {
            return _map(_tween.ValueAt(ms));
        }

        protected ITween<T> _tween;
        private readonly Func<T, TResult> _map;
    }

    /// <summary>Plays two tweens at the same time and puts their values together. It lasts as long as the longest one.</summary>
    public class ZipTween<T1, T2, TResult> : ITween<TResult> {
        public ZipTween(ITween<T1> tween1, ITween<T2> tween2, Func<T1, T2, TResult> zip) {
            _tween1 = tween1 ?? throw new ArgumentNullException(nameof(tween1));
            _tween2 = tween2 ?? throw new ArgumentNullException(nameof(tween2));
            _zip = zip ?? throw new ArgumentNullException(nameof(zip));
        }

        public double Duration => Math.Max(_tween1.Duration, _tween2.Duration);

        public TResult ValueAt(double ms) {
            return _zip(_tween1.ValueAt(ms), _tween2.ValueAt(ms));
        }

        protected ITween<T1> _tween1;
        protected ITween<T2> _tween2;
        private readonly Func<T1, T2, TResult> _zip;
    }

    public static class MapExtensions {
        /// <summary>Turns each value of <paramref name="tween"/> into another value with <paramref name="map"/>.</summary>
        public static ITween<TResult> Map<T, TResult>(this ITween<T> tween, Func<T, TResult> map) {
            return new MapTween<T, TResult>(tween, map);
        }
        /// <summary>Plays <paramref name="tween1"/> and <paramref name="tween2"/> at the same time and puts their values together with <paramref name="zip"/>.</summary>
        public static ITween<TResult> Zip<T1, T2, TResult>(this ITween<T1> tween1, ITween<T2> tween2, Func<T1, T2, TResult> zip) {
            return new ZipTween<T1, T2, TResult>(tween1, tween2, zip);
        }
    }
}
