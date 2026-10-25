# Getting started
This guide will show you how to get started with the Apos.Tweens library.

Before you start, make sure that you have a valid MonoGame project. You can create a new project by following this [other guide](https://learn-monogame.github.io/how-to/get-started/).

## Install

Install using the following dotnet command:

```
dotnet add package Apos.Tweens
```

For KNI, use `Apos.Tweens.KNI` instead.

You can find other ways to install it on the [NuGet page](https://www.nuget.org/packages/Apos.Tweens/).

## Setup

Import the library with:

```csharp
using Apos.Tweens;
```

At the start of your game's `Update`, give the game time to the library:

```csharp
protected override void Update(GameTime gameTime) {
    TweenHelper.UpdateSetup(gameTime);

    // Your update code.

    base.Update(gameTime);
}
```

## Your first tween

A tween goes from one value to another over some time. Create it once and play it, for example in `LoadContent`:

```csharp
protected override void LoadContent() {
    _s = new SpriteBatch(GraphicsDevice);
    _texture = Content.Load<Texture2D>("ship");

    _position = new Vector2Tween(new Vector2(100, 100), new Vector2(500, 100), 1000, Easing.SineInOut).Play();
}

SpriteBatch _s;
Texture2D _texture;
TweenPlayer<Vector2> _position;
```

The arguments are the start value, the end value, the duration in milliseconds, and an easing function that shapes the motion. [Easing](../easing/README.md) shows all of them.

`Play` starts the tween now and returns a `TweenPlayer`, which remembers when it started. Read the player's `Value` whenever you need it:

```csharp
protected override void Draw(GameTime gameTime) {
    GraphicsDevice.Clear(Color.Black);

    _s.Begin();
    _s.Draw(_texture, _position.Value, Color.White);
    _s.End();

    base.Draw(gameTime);
}
```

The ship moves from (100, 100) to (500, 100) in 1 second, speeding up and then slowing down. Before the tween starts, `Value` is the start value. After it ends, `Value` stays on the end value.

There's a tween for `float`, `Vector2` and `Color`: `FloatTween`, `Vector2Tween` and `ColorTween`. `Tween<T>` also works with `Vector3`, `Vector4` and `Quaternion`. [Custom types](../custom-types/README.md) shows how to tween anything else.

## How it works

A tween only describes the motion. It never changes once it's created and doesn't know what time it is. The player is what knows when the tween started. `Value` is computed each time you read it, from the current time and the player's start time. The only thing that needs to be updated each frame is the time, which is what `TweenHelper.UpdateSetup` does.

This means that you can keep as many players around as you want and they cost nothing until you read them. Many players can also share the same tween, for example a bounce that every coin plays when it gets picked up:

```csharp
ITween<float> bounce = new FloatTween(0f, -20f, 150, Easing.QuadOut).To(0f, 150, Easing.QuadIn);

// Each time a coin gets picked up:
coin.Bounce = bounce.Play();
```

You can also ask a tween for its value at any point, in milliseconds since the start:

```csharp
float top = bounce.ValueAt(150);
```

## Follow up

[Sequences](../sequences/README.md), a guide that shows how to chain tweens one after the other.
