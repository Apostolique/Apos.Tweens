# Apos.Tweens
Tweening library for MonoGame.

[![Discord](https://img.shields.io/discord/355231098122272778.svg)](https://discord.gg/MonoGame) [![NuGet](https://img.shields.io/nuget/v/Apos.Tweens.svg)](https://www.nuget.org/packages/Apos.Tweens/) [![NuGet](https://img.shields.io/nuget/dt/Apos.Tweens.svg)](https://www.nuget.org/packages/Apos.Tweens/)

## Description

This library moves values from one point to another over time. A tween only describes the motion and never changes once it's created. A player plays it on a clock and computes the value from the time whenever you read it. Tweens chain into longer animations, and many players can share the same tween.

This library is inspired by [MonoTween](https://github.com/njlr/MonoTween).

![Apos.Tweens Showcase](Images/Showcase.gif)

## Documentation

* [Getting started](https://apostolique.github.io/Apos.Tweens/getting-started/)
* [Sequences](https://apostolique.github.io/Apos.Tweens/sequences/)
* [Easing](https://apostolique.github.io/Apos.Tweens/easing/)
* [Time](https://apostolique.github.io/Apos.Tweens/time/)
* [Custom types](https://apostolique.github.io/Apos.Tweens/custom-types/)
* [Curves](https://apostolique.github.io/Apos.Tweens/curves/)

## Features

* Tweens for `float`, `Vector2`, `Vector3`, `Vector4`, `Color` and `Quaternion`, and `Tween<T>` for any type with a lerp function
* Chaining with `To`, `Offset`, `Wait`, `Then`, `Reverse`, `Yoyo` and `Loop`, fast to build and read even with thousands of steps
* Bezier curves and splines that go through a list of points
* `Speed`, `Delay`, `Slice` and `Remap` to change how a tween plays over time
* `Map` and `Zip` to drive many values from one tween
* Velocities, and `Retarget` to change where a tween goes without the motion kinking
* [Easing functions](https://easings.net/): Linear, Sine, Quad, Cube, Quart, Quint, Expo, Circ, Back, Elastic, Bounce
* Clocks that can be paused and slowed down on their own, for a game world that freezes while its menus keep moving
* `ValueAt` to get the value at any point in time
* Works with MonoGame and [KNI](https://github.com/kniengine/kni)

## Getting started

Install with:

```
dotnet add package Apos.Tweens
```

For KNI, use `Apos.Tweens.KNI` instead.

Create a tween and play it, give the game time to the library at the start of each update, then read the player's `Value`:

```csharp
var position = new Vector2Tween(new Vector2(50, 50), new Vector2(200, 200), 2000, Easing.SineIn)
    .Wait(1000)
    .Offset(new Vector2(-100, 0), 500, Easing.BounceOut)
    .Yoyo()
    .Loop()
    .Play();

// In Update:
TweenHelper.UpdateSetup(gameTime);

// In Draw:
_spriteBatch.Draw(_texture, position.Value, Color.White);
```

Read the [Getting started](https://apostolique.github.io/Apos.Tweens/getting-started/) guide for the full walkthrough.

## Other projects you might like

* [Apos.Gui](https://github.com/Apostolique/Apos.Gui) - UI library for MonoGame.
* [Apos.Input](https://github.com/Apostolique/Apos.Input) - Polling input library for MonoGame.
* [Apos.Camera](https://github.com/Apostolique/Apos.Camera) - Camera library for MonoGame.
* [Apos.Shapes](https://github.com/Apostolique/Apos.Shapes) - Shape rendering in MonoGame.
* [Apos.Spatial](https://github.com/Apostolique/Apos.Spatial) - Spatial partitioning library for MonoGame.
