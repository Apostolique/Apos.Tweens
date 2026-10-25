# Custom types
This guide will show you how to tween any type.

`FloatTween`, `Vector2Tween` and `ColorTween` are a `Tween<T>` with a lerp function already picked. A lerp function takes a start value, an end value, and a progress from 0 to 1, and returns the value in between. Give `Tween<T>` one and it works with any type.

## Built-in lerps

MonoGame already has lerp functions for most of its types, so you can pass them directly:

```csharp
var scale = new Tween<Vector3>(Vector3.One, new Vector3(2f), 400, Easing.BackOut, Vector3.Lerp);
var spin = new Tween<Quaternion>(Quaternion.Identity, target, 400, Easing.SineInOut, Quaternion.Slerp);
```

Chain them with `To`, which takes the lerp too:

```csharp
var pulse = new Tween<Vector3>(Vector3.One, new Vector3(1.2f), 150, Easing.QuadOut, Vector3.Lerp)
    .To(Vector3.One, 150, Easing.QuadIn, Vector3.Lerp)
    .Loop();
```

`Wait`, `Then`, `Loop`, `Reverse` and `Yoyo` work on any type. `Offset` only exists for `float`, `Vector2` and `Color` since it needs to add two values together.

## Writing a lerp

A lerp is any method with the same shape as `Vector2.Lerp`. For example, a rectangle:

```csharp
static Rectangle LerpRectangle(Rectangle a, Rectangle b, float t) {
    return new Rectangle(
        (int)MathF.Round(MathHelper.Lerp(a.X, b.X, t)),
        (int)MathF.Round(MathHelper.Lerp(a.Y, b.Y, t)),
        (int)MathF.Round(MathHelper.Lerp(a.Width, b.Width, t)),
        (int)MathF.Round(MathHelper.Lerp(a.Height, b.Height, t)));
}

var panel = new Tween<Rectangle>(closed, open, 250, Easing.CubeOut, LerpRectangle);
```

An angle in radians is a float, but `FloatTween` would spin the long way around when going from 350° to 10°. The trick is to wrap the difference first:

```csharp
static float LerpAngle(float a, float b, float t) {
    return a + MathHelper.WrapAngle(b - a) * t;
}

var rotation = new Tween<float>(_rotation, targetRotation, 300, Easing.SineOut, LerpAngle);
```

The lerp doesn't have to be a straight line either. This one goes along a curve that bends toward a control point:

```csharp
Vector2 control = new Vector2(300, 0);

var arc = new Tween<Vector2>(new Vector2(100, 400), new Vector2(500, 400), 800, Easing.SineInOut,
    (a, b, t) => Vector2.Lerp(Vector2.Lerp(a, control, t), Vector2.Lerp(control, b, t), t));
```

## Implementing ITween

For an animation that isn't a value going from one place to another, you can implement `ITween<T>` yourself. `ValueAt` gets the milliseconds since the start and returns the value at that point. `A` and `B` are the first and last values, which `To`, `Wait`, `Reverse` and `Yoyo` read. The [source](https://github.com/Apostolique/Apos.Tweens/tree/main/Source) of `Tween<T>` and `WaitTween<T>` shows the rest.
