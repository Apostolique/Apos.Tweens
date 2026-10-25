# Sequences
This guide will show you how to chain tweens to make longer animations.

Every tween can be followed by another one. The methods below each return a new tween that contains the old one, so you can keep chaining:

```csharp
var position = new Vector2Tween(new Vector2(50, 50), new Vector2(200, 200), 2000, Easing.SineIn)
    .Wait(1000)
    .Offset(new Vector2(-100, 0), 500, Easing.BounceOut)
    .Yoyo()
    .Loop();
```

The whole chain is a single tween. You read its `Value` like any other and it starts when the first tween in the chain starts.

In the images below, time goes from left to right. The bottom line is 0 and the top line is 1.

## To

`To` goes from where the previous tween ended to a new value:

```csharp
var height = new FloatTween(0f, 1f, 1000, Easing.SineInOut)
    .To(0.3f, 1000, Easing.BounceOut);
```

![A curve that rises to 1, then bounces down to 0.3](then.png)

## Offset

`Offset` moves by an amount from where the previous tween ended:

```csharp
var height = new FloatTween(0f, 0.4f, 800, Easing.CubeOut)
    .Offset(0.4f, 800, Easing.CubeOut)
    .Offset(-0.6f, 800, Easing.CubeOut);
```

![A curve that rises in two steps, then falls](offset.png)

On a `ColorTween`, the offset is a `Vector4` in the 0 to 1 range of `Color.ToVector4()`. It can be negative to take color away.

## Wait

`Wait` holds the end value of the previous tween for some time:

```csharp
var height = new FloatTween(0f, 1f, 1000, Easing.SineInOut)
    .Wait(800)
    .To(0f, 1000, Easing.SineInOut);
```

![A curve that rises, stays flat, then falls](wait.png)

## Then

`Then` plays any tween after another. `To`, `Offset` and `Wait` are built on it:

```csharp
var height = new FloatTween(0f, 1f, 1000, Easing.SineInOut)
    .Then(new FloatTween(1f, 0.3f, 1000, Easing.BounceOut));
```

The second tween starts when the first one ends, whatever its own start time is. It also doesn't need to start where the first one ended. If it doesn't, the value jumps from one to the other.

## Reverse

`Reverse` plays a tween backward:

```csharp
var height = new FloatTween(0f, 1f, 1000, Easing.QuadIn).Reverse();
```

![A curve that falls from 1 to 0, fast at first](reverse.png)

## Yoyo

`Yoyo` plays a tween forward, then backward to where it started:

```csharp
var height = new FloatTween(0f, 1f, 1000, Easing.QuadIn).Yoyo();
```

![A curve that rises to 1 and falls back to 0](yoyo.png)

## Loop

`Loop` plays a tween over and over, forever:

```csharp
var height = new FloatTween(0f, 1f, 600, Easing.QuadOut).Yoyo().Loop();
```

![A curve that keeps going up and down](loop-forever.png)

You can also give it the number of times to play. After the last time, it stays on the end value:

```csharp
var height = new FloatTween(0f, 1f, 800, Easing.SineInOut)
    .Wait(300)
    .Loop(3);
```

![A curve that rises three times, then stays at 1](loop.png)

A loop that never ends has a `Duration` of `double.PositiveInfinity`. Anything you chain after it never plays, so `Loop()` usually goes last.

## Changing a chain

Each method returns an `ITween<T>`, which doesn't let you change the start or end values. If you want to change them later, keep the tween you created yourself:

```csharp
_fade = new FloatTween(0f, 1f, 300, Easing.QuadOut);
_blink = _fade.Wait(500).To(0f, 300, Easing.QuadIn);

// Later:
_fade.B = 0.5f;
```

`To`, `Wait` and `Offset` start wherever the tween before them ends, even after a change. In the example above, `_blink` now fades in to 0.5, waits on 0.5 and fades out from 0.5. An `Offset` moves along with it too, since it's relative to where it starts.

When the target moves while a tween is playing, it's usually easier to create a new tween that starts from the current value:

```csharp
_position = new Vector2Tween(_position.Value, newTarget, 300, Easing.CubeOut);
```

## Follow up

[Easing](../easing/README.md), a page that shows every easing function.
