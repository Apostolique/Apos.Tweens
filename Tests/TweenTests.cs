using System;
using Microsoft.Xna.Framework;
using Xunit;

namespace Apos.Tweens.Tests {
    public class TweenTests {
        private readonly TweenClock _clock = new TweenClock();

        private static FloatTween Line(float a = 0f, float b = 100f, double duration = 1000) {
            return new FloatTween(a, b, duration, Easing.Linear);
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
            var player = Line().Play(_clock);

            _clock.TotalMS = 250;

            Assert.Equal(25f, player.Value);
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
        public void ThenCanJumpToAnyTween() {
            var tween = Line().Then(Line(500f, 600f)).Then(Line().Reverse());

            Assert.Equal(3000, tween.Duration);
            Assert.Equal(100f, tween.ValueAt(1000));
            Assert.Equal(550f, tween.ValueAt(1500));
            Assert.Equal(75f, tween.ValueAt(2250));
            Assert.Equal(0f, tween.B);
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
        public void ChainingLeavesTheOldTweenAsItWas() {
            var start = Line().Wait(500);
            var up = start.To(200f, 1000, Easing.Linear);
            var down = start.To(0f, 1000, Easing.Linear);
            var further = up.Offset(100f, 1000, Easing.Linear);

            Assert.Equal(1500, start.Duration);
            Assert.Equal(100f, start.ValueAt(5000));
            Assert.Equal(200f, up.ValueAt(5000));
            Assert.Equal(0f, down.ValueAt(5000));
            Assert.Equal(300f, further.ValueAt(5000));
            Assert.Equal(150f, up.ValueAt(2000));
            Assert.Equal(50f, down.ValueAt(2000));
        }

        [Fact]
        public void PlayersCanShareATween() {
            var tween = Line();
            var first = tween.Play(_clock);
            _clock.TotalMS = 500;
            var second = tween.Play(_clock);

            _clock.TotalMS = 750;

            Assert.Equal(75f, first.Value);
            Assert.Equal(25f, second.Value);
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
        public void VeryLongChainsCanBeReadInAnyOrder() {
            Tween<float> tween = Line(0f, 0f, 10);
            for (int i = 1; i <= 100_000; i++) {
                tween = i % 2 == 0 ? tween.Wait(10) : tween.Offset(1f, 10, Easing.Linear);
            }

            Assert.Equal(1_000_010, tween.Duration);
            Assert.Equal(50_000f, tween.B);

            var random = new Random(1);
            for (int i = 0; i < 1000; i++) {
                int step = random.Next(1, 100_001);
                float before = (step - 1) / 2 + (step % 2 == 0 ? 1 : 0);
                Assert.Equal(before + (step % 2 == 1 ? 0.5f : 0f), tween.ValueAt(step * 10 + 5));
            }
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
        public void LoopWithAFractionStopsPartWay() {
            var tween = Line().Loop(2.5);

            Assert.Equal(2500, tween.Duration);
            Assert.Equal(40f, tween.ValueAt(2400));
            Assert.Equal(50f, tween.ValueAt(2500));
            Assert.Equal(50f, tween.ValueAt(9000));
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
        public void RestartStartsFromTheClock() {
            var player = Line().To(0f, 1000, Easing.Linear).Play(_clock);

            _clock.TotalMS = 5000;
            Assert.True(player.IsDone);
            Assert.Equal(0f, player.Value);

            player.Restart();
            Assert.False(player.IsDone);
            Assert.Equal(0f, player.Elapsed);

            _clock.TotalMS = 5500;
            Assert.Equal(50f, player.Value);
        }

        [Fact]
        public void ALoopThatNeverEndsIsNeverDone() {
            var player = Line().Loop().Play(_clock);

            _clock.TotalMS = double.MaxValue;

            Assert.False(player.IsDone);
        }

        [Fact]
        public void TheStartTimeCanBeInTheFuture() {
            var player = Line().Play(500, _clock);

            _clock.TotalMS = 1000;

            Assert.Equal(50f, player.Value);
        }

        [Fact]
        public void PlayersUseTheDefaultClock() {
            var player = Line().Play();

            Assert.Same(TweenHelper.Clock, player.Clock);
        }

        [Fact]
        public void BuiltInTypesDontNeedALerp() {
            var tween = new Tween<Vector3>(Vector3.Zero, new Vector3(10f, 20f, 30f), 1000, Easing.Linear)
                .To(Vector3.Zero, 1000, Easing.Linear)
                .Offset(new Vector3(2f), 1000, Easing.Linear);

            Assert.Equal(new Vector3(5f, 10f, 15f), tween.ValueAt(500));
            Assert.Equal(new Vector3(5f, 10f, 15f), tween.ValueAt(1500));
            Assert.Equal(new Vector3(2f), tween.B);
        }

        [Fact]
        public void ToKeepsTheLerpOfTheStepBefore() {
            static float Double(float a, float b, float t) => MathHelper.Lerp(a, b, t) * 2f;

            var tween = new Tween<float>(0f, 10f, 1000, Easing.Linear, Double)
                .Wait(500)
                .To(20f, 1000, Easing.Linear);

            Assert.Equal(30f, tween.ValueAt(2000));
        }

        [Fact]
        public void ATypeWithoutALerpThrows() {
            Assert.Throws<InvalidOperationException>(() => new Tween<Point>(Point.Zero, new Point(1, 1), 1000, Easing.Linear));
            Assert.Throws<InvalidOperationException>(() => new WaitTween<Point>(Point.Zero, 1000).To(new Point(1, 1), 1000, Easing.Linear));
        }

        [Fact]
        public void ColorOffsetCanSubtract() {
            var tween = new ColorTween(new Color(200, 200, 200), new Color(200, 200, 200), 1000, Easing.Linear)
                .Offset(new Vector4(-100f / 255f, 0f, 100f / 255f, 0f), 1000, Easing.Linear);

            Assert.Equal(new Color(100, 200, 255, 255), tween.B);
        }
    }
}
