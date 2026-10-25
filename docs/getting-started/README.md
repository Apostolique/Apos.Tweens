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

A tween goes from one value to another over some time. Create it once, for example in `LoadContent`:

```csharp
protected override void LoadContent() {
    _s = new SpriteBatch(GraphicsDevice);
    _texture = Content.Load<Texture2D>("ship");

    _position = new Vector2Tween(new Vector2(100, 100), new Vector2(500, 100), 1000, Easing.SineInOut);
}

SpriteBatch _s;
Texture2D _texture;
ITween<Vector2> _position;
```

The arguments are the start value, the end value, the duration in milliseconds, and an easing function that shapes the motion. [Easing](../easing/README.md) shows all of them.

Then read the tween's `Value` whenever you need it:

```csharp
protected override void Draw(GameTime gameTime) {
    GraphicsDevice.Clear(Color.Black);

    _s.Begin();
    _s.Draw(_texture, _position.Value, Color.White);
    _s.End();

    base.Draw(gameTime);
}
```

The tween starts as soon as it's created. The ship moves from (100, 100) to (500, 100) in 1 second, speeding up and then slowing down. Before the tween starts, `Value` is the start value. After it ends, `Value` stays on the end value.

There's a tween for `float`, `Vector2` and `Color`: `FloatTween`, `Vector2Tween` and `ColorTween`. [Custom types](../custom-types/README.md) shows how to tween anything else.

## How it works

A tween doesn't keep track of its progress. `Value` is computed each time you read it, from the current time, the tween's start time and its duration. The only thing that needs to be updated each frame is the time, which is what `TweenHelper.UpdateSetup` does.

This means that you can keep as many tweens around as you want and they cost nothing until you read them. You can also ask for the value at any point, in milliseconds since the start:

```csharp
Vector2 halfway = _position.ValueAt(500);
```

## Follow up

[Sequences](../sequences/README.md), a guide that shows how to chain tweens one after the other.
