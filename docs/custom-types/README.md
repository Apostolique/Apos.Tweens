# Custom types
This guide will show you how to tween any type.

`FloatTween`, `Vector2Tween` and `ColorTween` are a `Tween<T>` with a lerp function already picked. A lerp function takes a start value, an end value, and a progress from 0 to 1, and returns the value in between. `Tween<T>` works with any type that has one.

## Built-in types

`Tween<T>` already knows the lerp for `float`, `Vector2`, `Vector3`, `Vector4`, `Color` and `Quaternion`, so you don't need to pass one:

```csharp
var scale = new Tween<Vector3>(Vector3.One, new Vector3(2f), 400, Easing.BackOut);
var spin = new Tween<Quaternion>(Quaternion.Identity, target, 400, Easing.SineInOut);
```

They chain like the others:

```csharp
var pulse = new Tween<Vector3>(Vector3.One, new Vector3(1.2f), 150, Easing.QuadOut)
    .To(Vector3.One, 150, Easing.QuadIn)
    .Loop();
```

`Wait`, `Then`, `Loop`, `Reverse` and `Yoyo` work on any type. `Offset` needs to add two values together, so it works on `float`, `Vector2`, `Vector3` and `Vector4`. On a `Color`, the offset is a `Vector4`.

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

If you tween rectangles in a lot of places, you can set the lerp once when your game starts instead. Every `Tween<Rectangle>` that isn't given one uses it:

```csharp
TweenMath<Rectangle>.Lerp = LerpRectangle;
TweenMath<Rectangle>.Add = (a, b) => new Rectangle(a.X + b.X, a.Y + b.Y, a.Width + b.Width, a.Height + b.Height);
```

`Add` is what `Offset` uses. [Splines](../curves/README.md) and velocities also need `Subtract` and `Scale`:

```csharp
TweenMath<Rectangle>.Subtract = (a, b) => new Rectangle(a.X - b.X, a.Y - b.Y, a.Width - b.Width, a.Height - b.Height);
TweenMath<Rectangle>.Scale = (a, s) => new Rectangle((int)(a.X * s), (int)(a.Y * s), (int)(a.Width * s), (int)(a.Height * s));
```

An angle in radians is a float, but `FloatTween` would spin the long way around when going from 350° to 10°. The trick is to wrap the difference first:

```csharp
static float LerpAngle(float a, float b, float t) {
    return a + MathHelper.WrapAngle(b - a) * t;
}

var rotation = new Tween<float>(_rotation, targetRotation, 300, Easing.SineOut, LerpAngle)
    .To(_rotation, 300, Easing.SineIn);
```

A `To` uses the same lerp as the step before it, so the way back also takes the short way around.

## One tween for many values

`Map` turns each value of a tween into another value, which can be of another type. Let's say a power up should grow, spin and fade in together. One tween from 0 to 1 can drive all of it:

```csharp
var appear = new FloatTween(0f, 1f, 400, Easing.BackOut)
    .Map(t => new Appearance(
        Scale: t,
        Rotation: (1f - t) * MathHelper.Pi,
        Color: Color.White * MathHelper.Clamp(t, 0f, 1f)));

_powerUp = appear.Play();
```

`Appearance` here is a record of your own. Everything in it stays in sync since it all comes from the same value.

`Zip` plays two tweens at the same time and puts their values together. It lasts as long as the longest one:

```csharp
var across = new FloatTween(0f, 400f, 1000, Easing.Linear);
var down = new FloatTween(0f, 300f, 1000, Easing.BounceOut);

var fall = across.Zip(down, (x, y) => new Vector2(x, y));
```

## Implementing ITween

For an animation that isn't a value going from one place to another, you can implement `ITween<T>` yourself. It has a `Duration` and a `ValueAt`, which gets the milliseconds since the start and returns the value at that point. For example, a shake that fades out:

```csharp
class Shake : ITween<Vector2> {
    public double Duration => 300;

    public Vector2 ValueAt(double ms) {
        float t = (float)(Math.Clamp(ms, 0, Duration) / Duration);
        return new Vector2(MathF.Sin(t * 60f), MathF.Sin(t * 45f)) * 8f * (1f - t);
    }
}
```

It plays like any other tween and works with `Then`, `To`, `Loop` and the rest:

```csharp
_offset = new Shake().Play();
```

Many players can read the same tween at different times, so `ValueAt` should only depend on the time it's given.

## Follow up

[Curves](../curves/README.md), a guide that shows how to move along curves and change targets smoothly.
