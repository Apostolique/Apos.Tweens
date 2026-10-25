using System;
using Microsoft.Xna.Framework;
using Xunit;

namespace Apos.Tweens.Tests {
    public class CurveTests {
        [Fact]
        public void ABezierBendsTowardItsControlPoint() {
            var tween = new BezierTween<float>(0f, 100f, 0f, 1000, Easing.Linear);

            Assert.Equal(0f, tween.ValueAt(-500));
            Assert.Equal(50f, tween.ValueAt(500), 4);
            Assert.Equal(0f, tween.ValueAt(1500));
        }

        [Fact]
        public void ACubicBezierLeavesAndArrivesAlongItsControlPoints() {
            var tween = new BezierTween<Vector2>(Vector2.Zero, new Vector2(0f, 100f), new Vector2(100f, 100f), new Vector2(100f, 0f), 1000, Easing.Linear);

            Vector2 middle = tween.ValueAt(500);
            Assert.Equal(50f, middle.X, 4);
            Assert.Equal(75f, middle.Y, 4);
            Assert.Equal(0f, tween.VelocityAt(0, 0.01).X, 0.0001f);
        }

        [Fact]
        public void BeziersOnlyNeedALerp() {
            var tween = new BezierTween<Color>(Color.Black, Color.White, Color.Black, 1000, Easing.Linear);

            Assert.InRange(tween.ValueAt(500).R, 126, 129);
        }

        [Fact]
        public void CurveToStartsFromTheEnd() {
            var tween = new FloatTween(0f, 100f, 1000, Easing.Linear)
                .CurveTo(300f, 100f, 1000, Easing.Linear)
                .To(0f, 1000, Easing.Linear);

            Assert.Equal(3000, tween.Duration);
            Assert.Equal(200f, tween.ValueAt(1500), 4);
            Assert.Equal(50f, tween.ValueAt(2500), 4);
        }

        [Fact]
        public void AHermiteLeavesAndArrivesAtItsVelocities() {
            var tween = new HermiteTween<float>(0f, 0.5f, 100f, 0f, 1000);

            Assert.Equal(0.5f, tween.VelocityAt(0, 0.01), 2);
            Assert.Equal(0f, tween.VelocityAt(1000, 0.01), 2);
            Assert.Equal(100f, tween.ValueAt(1000));
        }

        [Fact]
        public void ASplineGoesThroughEachPoint() {
            var tween = new SplineTween<float>(new[] { 0f, 10f, 0f, 10f }, 300, Easing.Linear);

            Assert.Equal(300, tween.Duration);
            Assert.Equal(10f, tween.ValueAt(100), 4);
            Assert.Equal(0f, tween.ValueAt(200), 4);
            Assert.Equal(10f, tween.ValueAt(400));
        }

        [Fact]
        public void ASplineHasNoKinks() {
            var tween = new SplineTween<Vector2>(new[] { Vector2.Zero, new Vector2(100f, 50f), new Vector2(300f, 0f), new Vector2(200f, -100f) }, new double[] { 0, 200, 300, 800 });

            foreach (double point in new double[] { 200, 300 }) {
                Vector2 before = tween.VelocityAt(point - 0.05, 0.05);
                Vector2 after = tween.VelocityAt(point + 0.05, 0.05);
                Assert.Equal(before.X, after.X, 0.01f);
                Assert.Equal(before.Y, after.Y, 0.01f);
            }
        }

        [Fact]
        public void ASplineReachesEachPointAtItsTime() {
            var tween = new SplineTween<float>(new[] { 0f, 10f, 20f }, new double[] { 100, 200, 600 });

            Assert.Equal(600, tween.Duration);
            Assert.Equal(0f, tween.ValueAt(50));
            Assert.Equal(10f, tween.ValueAt(200), 4);
            Assert.Equal(20f, tween.ValueAt(600), 4);
        }

        [Fact]
        public void ASplineWithOnePointHoldsIt() {
            var tween = new SplineTween<float>(new[] { 5f }, 300, Easing.Linear);

            Assert.Equal(300, tween.Duration);
            Assert.Equal(5f, tween.ValueAt(150));
        }

        [Fact]
        public void SplineToStartsFromTheEnd() {
            var tween = new FloatTween(0f, 10f, 100, Easing.Linear).SplineTo(new[] { 20f, 10f }, 200, Easing.Linear);

            Assert.Equal(300, tween.Duration);
            Assert.Equal(20f, tween.ValueAt(200), 4);
            Assert.Equal(10f, tween.B);
        }

        [Fact]
        public void ASplineNeedsTimesInOrder() {
            Assert.Throws<ArgumentException>(() => new SplineTween<float>(new[] { 0f, 1f, 2f }, new double[] { 0, 200, 200 }));
            Assert.Throws<ArgumentException>(() => new SplineTween<float>(new[] { 0f, 1f }, new double[] { 0 }));
        }

        [Fact]
        public void ATypeWithoutVectorMathCantMakeASpline() {
            Assert.Throws<InvalidOperationException>(() => new SplineTween<Color>(new[] { Color.Black, Color.White }, 100, Easing.Linear));
            Assert.Throws<InvalidOperationException>(() => new HermiteTween<Color>(Color.Black, Color.Black, Color.White, Color.Black, 100));
        }
    }
}
