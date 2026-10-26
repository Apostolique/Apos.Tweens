# Curves
This guide will show you how to move along curves instead of straight lines.

## Bezier curves

A `BezierTween` goes from one value to another along a curve that bends toward a control point:

```csharp
var arc = new BezierTween<Vector2>(new Vector2(100, 400), new Vector2(300, 0), new Vector2(500, 400), 800, Easing.SineInOut);
```

![An arch between two points, under a control point above them](bezier.png)

The curve doesn't go through the control point. It starts by heading toward it, then turns to reach the end. With two control points, the curve leaves toward the first one and arrives from the second one:

```csharp
var swoop = new BezierTween<Vector2>(start, start + new Vector2(0, -200), end + new Vector2(0, -200), end, 800, Easing.CubeInOut);
```

![A wide arch that leaves straight up from the start and comes straight down to the end](bezier-cubic.png)

The easing is how fast it moves along the curve, not the shape of the curve.

`CurveTo` chains a curve from where the previous tween ended:

```csharp
var position = new Vector2Tween(new Vector2(100, 400), new Vector2(200, 400), 300, Easing.Linear)
    .CurveTo(new Vector2(400, 100), new Vector2(500, 400), 800, Easing.SineOut)
    .To(new Vector2(600, 400), 300, Easing.Linear);
```

A Bezier curve only needs a lerp, so it works with every type that `Tween<T>` works with, including `Color` and your own types.

## Splines

A `SplineTween` goes through each point along a smooth curve:

```csharp
var points = new[] {
    new Vector2(100, 300),
    new Vector2(250, 150),
    new Vector2(400, 300),
    new Vector2(550, 150),
};

var path = new SplineTween<Vector2>(points, 2000, Easing.Linear);
```

![A smooth wave that goes through 4 points](spline.png)

Each point to the next takes the same time, so the tween goes faster where the points are far apart. The easing applies to the whole spline, so with `Easing.SineInOut` it starts and stops slowly but doesn't stop at each point.

You can also say when to reach each point, in milliseconds from the start:

```csharp
var path = new SplineTween<Vector2>(points, new double[] { 0, 300, 1200, 1500 });
```

Each time comes after the one before. If the first time isn't 0, the tween holds the first point until then.

`SplineTo` chains a spline from where the previous tween ended:

```csharp
var flight = new Vector2Tween(new Vector2(0, 300), new Vector2(100, 300), 200, Easing.Linear)
    .SplineTo(points, 2000, Easing.Linear);
```

The spline goes through the points, so it needs to add, subtract and scale values. It works on `float`, `Vector2`, `Vector3` and `Vector4`. [Custom types](../custom-types/README.md) shows how to add that math for your own types.

## Follow up

[Moving targets](../targets/README.md), a guide that shows how to ease toward a target that keeps changing.
