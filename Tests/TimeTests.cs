using Microsoft.Xna.Framework;
using Xunit;

namespace Apos.Tweens.Tests {
    public class TimeTests {
        private readonly TweenClock _clock = new TweenClock();

        private static FloatTween Line(float a = 0f, float b = 100f, double duration = 1000) {
            return new FloatTween(a, b, duration, Easing.Linear);
        }

        [Fact]
        public void SlicePlaysPartOfATween() {
            var tween = Line().Slice(250, 750);

            Assert.Equal(500, tween.Duration);
            Assert.Equal(25f, tween.ValueAt(-100));
            Assert.Equal(50f, tween.ValueAt(250));
            Assert.Equal(75f, tween.ValueAt(900));
        }

        [Fact]
        public void SliceEndsALoopThatNeverEnds() {
            var tween = Line().Loop().Slice(0, 2500);

            Assert.Equal(2500, tween.Duration);
            Assert.Equal(50f, tween.ValueAt(5000));
        }

        [Fact]
        public void SpeedChangesTheDuration() {
            var fast = Line().Speed(2);
            var slow = Line().Speed(0.5);

            Assert.Equal(500, fast.Duration);
            Assert.Equal(50f, fast.ValueAt(250));
            Assert.Equal(2000, slow.Duration);
            Assert.Equal(50f, slow.ValueAt(1000));
        }

        [Fact]
        public void DelayHoldsTheStartValue() {
            var tween = Line().Delay(500);

            Assert.Equal(1500, tween.Duration);
            Assert.Equal(0f, tween.ValueAt(250));
            Assert.Equal(50f, tween.ValueAt(1000));
        }

        [Fact]
        public void RemapReadsAtAnyTime() {
            var tween = Line().Remap(2000, ms => 1000 - ms / 2);

            Assert.Equal(2000, tween.Duration);
            Assert.Equal(100f, tween.ValueAt(0));
            Assert.Equal(75f, tween.ValueAt(500));
            Assert.Equal(0f, tween.ValueAt(5000));
        }

        [Fact]
        public void MapTurnsValuesIntoAnotherType() {
            var tween = Line().Map(x => new Vector2(x, -x));

            Assert.Equal(1000, tween.Duration);
            Assert.Equal(new Vector2(50f, -50f), tween.ValueAt(500));
        }

        [Fact]
        public void ZipLastsAsLongAsTheLongestTween() {
            var tween = Line().Zip(Line(duration: 2000), (x, y) => new Vector2(x, y));

            Assert.Equal(2000, tween.Duration);
            Assert.Equal(new Vector2(100f, 75f), tween.ValueAt(1500));
        }

        [Fact]
        public void VelocityIsInUnitsPerMillisecond() {
            var tween = Line();

            Assert.Equal(0.1f, tween.VelocityAt(500), 4);
            Assert.Equal(0.1f, tween.VelocityAt(0), 4);
            Assert.Equal(0f, tween.VelocityAt(-10));
            Assert.Equal(0f, tween.VelocityAt(1010));
        }

        [Fact]
        public void RetargetKeepsTheValueAndVelocity() {
            var player = Line().Play(_clock);
            _clock.TotalMS = 500;

            player.Retarget(0f, 1000);

            Assert.Equal(50f, player.Value, 4);
            Assert.Equal(0.1f, player.Velocity, 2);

            _clock.TotalMS = 1500;
            Assert.True(player.IsDone);
            Assert.Equal(0f, player.Value);
        }

        [Fact]
        public void UpdateSetupMovesByTheElapsedTime() {
            double before = TweenHelper.TotalMS;

            TweenHelper.UpdateSetup(100);

            Assert.Equal(before + 100 * TweenHelper.Clock.Scale, TweenHelper.TotalMS, 6);
        }
    }
}
