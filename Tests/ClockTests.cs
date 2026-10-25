using System;
using Microsoft.Xna.Framework;
using Xunit;

namespace Apos.Tweens.Tests {
    public class ClockTests {
        private static GameTime At(double ms) {
            return new GameTime(TimeSpan.FromMilliseconds(ms), TimeSpan.FromMilliseconds(16));
        }

        [Fact]
        public void TheFirstUpdateOnlyRemembersTheGameTime() {
            var clock = new TweenClock();

            clock.Update(At(5000));

            Assert.Equal(0, clock.TotalMS);
        }

        [Fact]
        public void UpdateMovesByTheGameTimeThatWentBy() {
            var clock = new TweenClock();

            clock.Update(At(5000));
            clock.Update(At(5016));
            clock.Update(At(5032));

            Assert.Equal(32, clock.TotalMS, 6);
        }

        [Fact]
        public void UpdatingTwiceInOneFrameMovesOnce() {
            var clock = new TweenClock();

            clock.Update(At(0));
            clock.Update(At(16));
            clock.Update(At(16));

            Assert.Equal(16, clock.TotalMS, 6);
        }

        [Fact]
        public void ScaleSlowsTimeDown() {
            var clock = new TweenClock { Scale = 0.5f };

            clock.Update(At(0));
            clock.Update(At(100));

            Assert.Equal(50, clock.TotalMS, 6);
        }

        [Fact]
        public void AScaleOf0Pauses() {
            var clock = new TweenClock();
            var player = new FloatTween(0f, 100f, 1000, Easing.Linear).Play(clock);

            clock.Update(At(0));
            clock.Update(At(300));
            clock.Scale = 0f;
            clock.Update(At(5000));

            Assert.Equal(30f, player.Value, 4);
        }

        [Fact]
        public void SmallScalesDontLosePrecision() {
            var clock = new TweenClock { Scale = 0.1f };

            for (int i = 0; i <= 1000; i++) {
                clock.Update(At(i * 16.6667));
            }

            Assert.Equal(1666.67, clock.TotalMS, 0);
        }
    }
}
