using System;
using Microsoft.Xna.Framework;
using Xunit;

namespace Apos.Tweens.Tests {
    public class EasedValueTests {
        private readonly TweenClock _clock = new TweenClock();

        private EasedValue<float> Make(float value = 0f, double duration = 1000, EasingFunction easing = null) {
            return new EasedValue<float>(value, duration, easing ?? Easing.Linear, _clock);
        }

        [Fact]
        public void ItStartsOnItsValue() {
            var value = Make(5f);

            Assert.Equal(5f, value.Value);
            Assert.Equal(5f, value.Target);
            Assert.True(value.IsDone);
        }

        [Fact]
        public void SettingTheTargetEasesThere() {
            var value = Make();

            value.Target = 100f;
            Assert.False(value.IsDone);

            _clock.TotalMS = 500;
            Assert.Equal(50f, value.Value);
            Assert.Equal(100f, value.Target);

            _clock.TotalMS = 1000;
            Assert.Equal(100f, value.Value);
            Assert.True(value.IsDone);
        }

        [Fact]
        public void TheSameTargetDoesntStartOver() {
            var value = Make();
            value.Target = 100f;

            _clock.TotalMS = 500;
            value.Target = 100f;

            _clock.TotalMS = 1000;
            Assert.Equal(100f, value.Value);
        }

        [Fact]
        public void ANewTargetStartsFromTheCurrentValue() {
            var value = Make();
            value.Target = 100f;
            _clock.TotalMS = 500;

            value.Target = 0f;

            Assert.Equal(50f, value.Value);
            _clock.TotalMS = 1000;
            Assert.Equal(25f, value.Value);
        }

        [Fact]
        public void TargetsCanBeAddedOnTopOfAMove() {
            var value = Make();
            value.Target = 100f;
            _clock.TotalMS = 500;

            value.Target += 100f;

            _clock.TotalMS = 1500;
            Assert.Equal(200f, value.Value);
        }

        [Fact]
        public void SnapJumpsRightAway() {
            var value = Make();
            value.Target = 100f;
            _clock.TotalMS = 500;

            value.Snap(30f);

            Assert.Equal(30f, value.Value);
            Assert.Equal(30f, value.Target);
            Assert.True(value.IsDone);
        }

        [Fact]
        public void TheDurationCanDependOnTheDistance() {
            var value = new EasedValue<float>(0f, (a, b) => Math.Abs(b - a) * 10, Easing.Linear, _clock);

            value.Target = 20f;

            _clock.TotalMS = 100;
            Assert.Equal(10f, value.Value);
            _clock.TotalMS = 200;
            Assert.True(value.IsDone);
        }

        [Fact]
        public void DurationAndEasingApplyToTheNextMove() {
            var value = Make();
            value.Target = 100f;
            _clock.TotalMS = 500;

            value.Easing = Easing.QuadIn;
            value.Duration = (_, _) => 2000;
            Assert.Equal(50f, value.Value);

            value.Target = 0f;
            _clock.TotalMS = 1500;
            Assert.Equal(50f - 50f * 0.25f, value.Value, 4);
        }

        [Fact]
        public void VelocityIsInUnitsPerMillisecond() {
            var value = Make();
            value.Target = 100f;
            _clock.TotalMS = 500;

            Assert.Equal(0.1f, value.Velocity, 4);

            _clock.TotalMS = 2000;
            Assert.Equal(0f, value.Velocity);
        }

        [Fact]
        public void SmoothKeepsTheSpeedThroughATargetChange() {
            var value = new EasedValue<Vector2>(Vector2.Zero, 1000, Easing.SineInOut, _clock) { Smooth = true };
            value.Target = new Vector2(100f, 0f);
            _clock.TotalMS = 500;
            Vector2 before = value.Velocity;

            value.Target = new Vector2(0f, 100f);

            Vector2 after = value.Velocity;
            Assert.Equal(before.X, after.X, 0.01f);
            Assert.Equal(before.Y, after.Y, 0.01f);
            _clock.TotalMS = 1500;
            Assert.Equal(new Vector2(0f, 100f), value.Value);
        }

        [Fact]
        public void WithoutSmoothTheSpeedStartsOver() {
            var value = new EasedValue<float>(0f, 1000, Easing.SineInOut, _clock);
            value.Target = 100f;
            _clock.TotalMS = 500;

            value.Target = 0f;

            Assert.Equal(0f, value.Velocity, 0.01f);
        }

        [Fact]
        public void SmoothAddsTheOldSpeedToAnEasingThatStartsMoving() {
            var value = new EasedValue<float>(0f, 1000, Easing.Linear, _clock) { Smooth = true };
            value.Target = 100f;
            _clock.TotalMS = 500;

            value.Target = 150f;

            Assert.Equal(0.1f + 0.1f, value.Velocity, 0.01f);
            _clock.TotalMS = 1500;
            Assert.Equal(150f, value.Value);
        }

        [Fact]
        public void ChangingTheTargetDoesntAllocate() {
            var value = new EasedValue<Vector2>(Vector2.Zero, (a, b) => Vector2.Distance(a, b), Easing.ExpoOut, _clock) { Smooth = true };
            for (int i = 0; i < 100; i++) Step(value, i);

            long before = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < 1000; i++) Step(value, i);
            long allocated = GC.GetAllocatedBytesForCurrentThread() - before;

            Assert.Equal(0, allocated);

            void Step(EasedValue<Vector2> v, int i) {
                _clock.TotalMS += 16;
                v.Target = new Vector2(i % 7 * 10f, i % 5 * 10f);
                v.Snap(i % 50 == 0 ? Vector2.One : v.Value);
                v.Target = new Vector2(i, -i);
                _ = v.Value;
                _ = v.Velocity;
            }
        }

        [Fact]
        public void EasedValuesOnlyNeedALerp() {
            var value = new EasedValue<Color>(Color.Black, 1000, Easing.Linear, _clock);
            value.Target = Color.White;
            _clock.TotalMS = 1000;

            Assert.Equal(Color.White, value.Value);
            Assert.Throws<InvalidOperationException>(() => value.Smooth = true);
        }

        [Fact]
        public void RetargetWithAnEasingKeepsTheSpeed() {
            var player = new FloatTween(0f, 100f, 1000, Easing.SineInOut).Play(_clock);
            _clock.TotalMS = 500;
            float before = player.Velocity;

            player.Retarget(0f, 1000, Easing.SineInOut);

            Assert.Equal(before, player.Velocity, 0.01f);
            Assert.Equal(50f, player.Value, 4);
            _clock.TotalMS = 1500;
            Assert.Equal(0f, player.Value);
        }
    }
}
