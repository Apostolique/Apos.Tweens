using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Apos.Tweens.Tests {
    public class EasingTests {
        public static IEnumerable<object[]> All =>
            typeof(Easing).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.ReturnType == typeof(float))
                .Select(m => new object[] { m.Name });

        private static Interpolator Get(string name) {
            return typeof(Easing).GetMethod(name).CreateDelegate<Interpolator>();
        }

        [Theory]
        [MemberData(nameof(All))]
        public void StartsAt0AndEndsAt1(string name) {
            Interpolator easing = Get(name);

            Assert.Equal(0f, easing(0f), 4);
            Assert.Equal(1f, easing(1f), 4);
        }

        [Theory]
        [MemberData(nameof(All))]
        public void HasNoJumps(string name) {
            Interpolator easing = Get(name);

            const int steps = 10000;
            for (int i = 0; i < steps; i++) {
                float x0 = i / (float)steps;
                float x1 = (i + 1) / (float)steps;
                if (MathF.Abs(easing(x1) - easing(x0)) < 0.005f) continue;

                // A steep curve flattens out as the step gets smaller while a jump doesn't.
                while (x1 - x0 > 1e-6f) {
                    float mid = (x0 + x1) / 2f;
                    if (MathF.Abs(easing(mid) - easing(x0)) > MathF.Abs(easing(x1) - easing(mid))) {
                        x1 = mid;
                    } else {
                        x0 = mid;
                    }
                }
                Assert.True(MathF.Abs(easing(x1) - easing(x0)) < 0.005f, $"{name} jumps by {easing(x1) - easing(x0)} at {x0}");
            }
        }

        [Theory]
        [MemberData(nameof(All))]
        public void InOutIsHalfwayAtTheMiddle(string name) {
            if (!name.EndsWith("InOut")) return;

            Assert.Equal(0.5f, Get(name)(0.5f), 4);
        }

        [Fact]
        public void CircIsACircle() {
            Assert.Equal(MathF.Sqrt(0.75f), Easing.CircOut(0.5f), 5);
            Assert.Equal(1f - MathF.Sqrt(0.75f), Easing.CircIn(0.5f), 5);
        }
    }
}
