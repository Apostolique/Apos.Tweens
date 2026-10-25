# Time
This guide will show you how to control when tweens play: delays, restarts, pausing and slow motion.

Every tween reads its time from a clock. By default, that's `TweenHelper.Clock`, which `TweenHelper.UpdateSetup(gameTime)` moves forward. Times are in milliseconds.

## Start time

A tween starts at the current time on its clock when you create it. You can give it a start time instead, for example to delay it by 200 milliseconds:

```csharp
var fade = new FloatTween(0f, 1f, 500, Easing.QuadOut, TweenHelper.TotalMS + 200);
```

This also works to stagger a few tweens:

```csharp
for (int i = 0; i < _items.Length; i++) {
    _items[i] = new Vector2Tween(new Vector2(-200, 100 + i * 50), new Vector2(100, 100 + i * 50), 400, Easing.CubeOut, TweenHelper.TotalMS + i * 80);
}
```

`StartTime` can also be changed after the tween is created, even on a chain.

## Restart

`Restart` starts a tween over from the current time. For example, to play a hop each time the player jumps:

```csharp
if (jumped) {
    _hop.Restart();
}
```

## Is it done?

`IsDone()` tells you when a tween has played to the end, and `Elapsed()` tells you how long it's been playing:

```csharp
if (_fadeOut.IsDone()) {
    _screen = new MainMenu();
}
```

A tween that loops forever is never done.

## Clocks

Let's say your game has a pause menu. When the menu opens, the game world should freeze but the menu should keep animating. This is where you'd give the world its own clock:

```csharp
TweenClock _world = new TweenClock();
```

Move it forward in your update, next to the default one:

```csharp
protected override void Update(GameTime gameTime) {
    TweenHelper.UpdateSetup(gameTime);
    _world.Update(gameTime);

    // ...
}
```

Then pass it to the tweens that belong to the world:

```csharp
var position = new Vector2Tween(new Vector2(100, 100), new Vector2(500, 100), 1000, Easing.SineInOut, _world);
```

A clock's `Scale` is how fast time goes by on it. 1 is normal speed, 0.5 is half speed and 0 is paused:

```csharp
_world.Scale = _isPaused ? 0f : 1f;
```

The menu's tweens use `TweenHelper.Clock` so they keep playing.

A clock's first `Update` only remembers the game time, so a new clock starts at 0 no matter when you create it. After that, each `Update` moves it by the time that went by since the last one, so calling it twice in the same frame doesn't count the time twice.

If you'd rather move the clock yourself, for example with a fixed time step, you can pass the elapsed milliseconds:

```csharp
_world.Update(16.6667);
```

You can also set `TotalMS` directly.

## Changing the clock

Each tween's `Clock` can be changed. The start time doesn't change with it, and a start time from one clock doesn't mean much on another, so restart the tween after:

```csharp
_position.Clock = _world;
_position.Restart();
```

## Follow up

[Custom types](../custom-types/README.md), a guide that shows how to tween types other than `float`, `Vector2` and `Color`.
