using Microsoft.Xna.Framework;
using Xunit;

namespace Apos.Tweens.Tests {
    public class TweenTests {
        private readonly TweenClock _clock = new TweenClock();

        private FloatTween Line(float a = 0f, float b = 100f, double duration = 1000) {
            return new FloatTween(a, b, duration, Easing.Linear, _clock);
        }

        [Fact]
        public void ATweenHoldsItsEndsOutsideItsDuration() {
            var tween = Line();

            Assert.Equal(0f, tween.ValueAt(-500));
            Assert.Equal(50f, tween.ValueAt(500));
            Assert.Equal(100f, tween.ValueAt(1500));
        }

        [Fact]
        public void ValueReadsTheClock() {
            var tween = Line();

            _clock.TotalMS = 250;

            Assert.Equal(25f, tween.Value);
        }

        [Fact]
        public void ReverseHoldsItsOwnEnds() {
            var tween = Line().Reverse();

            Assert.Equal(100f, tween.ValueAt(-500));
            Assert.Equal(75f, tween.ValueAt(250));
            Assert.Equal(0f, tween.ValueAt(1500));
        }

        [Fact]
        public void YoyoEndsWhereItStarted() {
            var tween = Line().Yoyo();

            Assert.Equal(2000, tween.Duration);
            Assert.Equal(100f, tween.ValueAt(1000));
            Assert.Equal(50f, tween.ValueAt(1500));
            Assert.Equal(0f, tween.ValueAt(2500));
            Assert.Equal(0f, tween.B);
        }

        [Fact]
        public void ThenPlaysOneAfterTheOther() {
            var tween = Line().To(300f, 1000, Easing.Linear);

            Assert.Equal(2000, tween.Duration);
            Assert.Equal(100f, tween.ValueAt(1000));
            Assert.Equal(200f, tween.ValueAt(1500));
            Assert.Equal(300f, tween.ValueAt(5000));
        }

        [Fact]
        public void OffsetMovesFromTheEnd() {
            var tween = Line().Offset(-50f, 1000, Easing.Linear);

            Assert.Equal(50f, tween.ValueAt(2000));
        }

        [Fact]
        public void WaitHoldsTheEndValue() {
            var tween = Line().Wait(500).To(0f, 1000, Easing.Linear);

            Assert.Equal(2500, tween.Duration);
            Assert.Equal(100f, tween.ValueAt(1250));
            Assert.Equal(50f, tween.ValueAt(2000));
        }

        [Fact]
        public void AChainFollowsChangesToTheTweensInIt() {
            var first = Line();
            var tween = first.Wait(500).To(0f, 1000, Easing.Linear);

            first.B = 50f;

            Assert.Equal(50f, tween.ValueAt(1000));
            Assert.Equal(50f, tween.ValueAt(1250));
            Assert.Equal(25f, tween.ValueAt(2000));
        }

        [Fact]
        public void OffsetFollowsTheEndOfThePreviousTween() {
            var first = Line();
            var tween = first.Offset(-50f, 1000, Easing.Linear);

            first.B = 200f;

            Assert.Equal(150f, tween.B);
            Assert.Equal(175f, tween.ValueAt(1500));
        }

        [Fact]
        public void LongChainsPlayEachStepInOrder() {
            ITween<float> tween = Line(0f, 1f, 100);
            for (int i = 2; i <= 50; i++) {
                tween = tween.To(i, 100, Easing.Linear);
            }

            Assert.Equal(5000, tween.Duration);
            Assert.Equal(0.5f, tween.ValueAt(50));
            Assert.Equal(23.5f, tween.ValueAt(2350));
            Assert.Equal(50f, tween.ValueAt(6000));
        }

        [Fact]
        public void LoopRunsForever() {
            var tween = Line().Loop();

            Assert.Equal(double.PositiveInfinity, tween.Duration);
            Assert.Equal(50f, tween.ValueAt(500));
            Assert.Equal(50f, tween.ValueAt(1_000_000_500));
        }

        [Fact]
        public void NothingPlaysAfterALoopThatNeverEnds() {
            var tween = Line().Loop().Then(Line(500f, 600f));

            Assert.Equal(50f, tween.ValueAt(1500));
        }

        [Fact]
        public void LoopWithACountEndsOnTheLastValue() {
            var tween = Line().Loop(3);

            Assert.Equal(3000, tween.Duration);
            Assert.Equal(50f, tween.ValueAt(2500));
            Assert.Equal(100f, tween.ValueAt(3000));
            Assert.Equal(100f, tween.ValueAt(9000));
        }

        [Fact]
        public void LoopOnAnEmptyTweenDoesNotThrow() {
            var tween = Line(duration: 0).Loop();

            Assert.Equal(0, tween.Duration);
            Assert.Equal(100f, tween.ValueAt(10));
        }

        [Fact]
        public void YoyoLoopGoesBackAndForth() {
            var tween = Line().Yoyo().Loop();

            Assert.Equal(50f, tween.ValueAt(4500));
            Assert.Equal(50f, tween.ValueAt(5500));
            Assert.Equal(100f, tween.ValueAt(5000));
        }

        [Fact]
        public void StartTimeAndClockCanBeSetOnAChain() {
            var first = Line();
            var chain = first.Wait(500).Loop();
            var other = new TweenClock();

            chain.StartTime = 2000;
            chain.Clock = other;

            Assert.Equal(2000, first.StartTime);
            Assert.Same(other, first.Clock);
        }

        [Fact]
        public void RestartStartsFromTheClock() {
            var tween = Line().To(0f, 1000, Easing.Linear);

            _clock.TotalMS = 5000;
            Assert.True(tween.IsDone());
            Assert.Equal(0f, tween.Value);

            tween.Restart();
            Assert.False(tween.IsDone());
            Assert.Equal(0f, tween.Elapsed());

            _clock.TotalMS = 5500;
            Assert.Equal(50f, tween.Value);
        }

        [Fact]
        public void ALoopThatNeverEndsIsNeverDone() {
            var tween = Line().Loop();

            _clock.TotalMS = double.MaxValue;

            Assert.False(tween.IsDone());
        }

        [Fact]
        public void TheStartTimeCanBeInTheFuture() {
            var tween = new FloatTween(0f, 100f, 1000, Easing.Linear, 500, _clock);

            _clock.TotalMS = 1000;

            Assert.Equal(50f, tween.Value);
        }

        [Fact]
        public void AnyTypeWithALerpCanTween() {
            var tween = new Tween<Vector3>(Vector3.Zero, new Vector3(10f, 20f, 30f), 1000, Easing.Linear, Vector3.Lerp, _clock)
                .To(Vector3.Zero, 1000, Easing.Linear, Vector3.Lerp);

            Assert.Equal(new Vector3(5f, 10f, 15f), tween.ValueAt(500));
            Assert.Equal(new Vector3(5f, 10f, 15f), tween.ValueAt(1500));
        }

        [Fact]
        public void ColorOffsetCanSubtract() {
            var tween = new ColorTween(new Color(200, 200, 200), new Color(200, 200, 200), 1000, Easing.Linear, _clock)
                .Offset(new Vector4(-100f / 255f, 0f, 100f / 255f, 0f), 1000, Easing.Linear);

            Assert.Equal(new Color(100, 200, 255, 255), tween.B);
        }
    }
}
