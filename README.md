# Apos.Tweens
Tweening library for MonoGame.

[![Discord](https://img.shields.io/discord/355231098122272778.svg)](https://discord.gg/MonoGame)

This library is inspired by [MonoTween](https://github.com/njlr/MonoTween).

## Documentation

* Coming soon!

## Build

[![NuGet](https://img.shields.io/nuget/v/Apos.Tweens.svg)](https://www.nuget.org/packages/Apos.Tweens/) [![NuGet](https://img.shields.io/nuget/dt/Apos.Tweens.svg)](https://www.nuget.org/packages/Apos.Tweens/)

## Features

* Interpolate float values
* Interpolate Vector2 values
* [Easing functions](https://easings.net/):
  * Linear
  * Sine
  * Quad
  * Cube
  * Quart
  * Quint
  * Circ
  * Expo
  * Back
  * Elastic
  * Bounce

## Showcase

![Apos.Tweens Showcase](Images/Showcase.gif)

## Usage samples

```csharp
var position = new Vector2Tween(new Vector2(50, 50), new Vector2(200, 200), 2000, Easing.SineIn)
    .Wait(1000)
    .Offset(new Vector2(-100, 0), 500, Easing.BounceOut)
    .Yoyo()
    .Loop();

// ...

TweenHelper.UpdateSetup(gameTime);

// ...

shapeBatch.DrawCircle(position.Value, 10f, Color.White, Color.Black, 2f);
```

## Other projects you might like

* [Apos.Gui](https://github.com/Apostolique/Apos.Gui) - UI library for MonoGame.
* [Apos.Input](https://github.com/Apostolique/Apos.Input) - Polling input library for MonoGame.
* [Apos.Camera](https://github.com/Apostolique/Apos.Camera) - Camera library for MonoGame.
* [Apos.Shapes](https://github.com/Apostolique/Apos.Shapes) - Shape rendering in MonoGame.
* [Apos.Spatial](https://github.com/Apostolique/Apos.Spatial) - Spatial partitioning library for MonoGame.
