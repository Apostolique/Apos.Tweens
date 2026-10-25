# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `Tween<T>` to tween any type with a lerp function
- `TweenPlayer<T>` and `Play` to play a tween on a clock, with `Restart`, `Elapsed` and `IsDone`
- `TweenClock`, a clock that can be paused and slowed down with `Scale`
- `TweenHelper.Clock`, the clock players use by default
- `TweenMath<T>` to set the lerp and add of a type once
- Lerps for `Vector3`, `Vector4` and `Quaternion` that tweens use without being given one
- `Offset` for any type with an add
- `Loop(count)` to play a tween a set number of times, fractions included
- `BezierTween<T>` and `CurveTo` to move along a curve with one or two control points
- `SplineTween<T>` and `SplineTo` to go through a list of points, evenly or at given times
- `HermiteTween<T>` to go between two values with a start and end velocity
- `Speed`, `Delay`, `Slice` and `Remap` to change how a tween plays over time
- `Map` and `Zip` to turn values into other values or put two tweens together
- `VelocityAt` on tweens and `Velocity` on players
- `Retarget` on players to change the target without losing the velocity
- `Subtract` and `Scale` in `TweenMath<T>`

### Changed

- Times and durations are `double` milliseconds instead of `long`
- `Interpolator` is now `EasingFunction`
- `TweenHelper.UpdateSetup(double)` moves the clock by the elapsed milliseconds instead of setting its time
- Tweens don't change once they're created, and chaining leaves the old tween as it was
- `To`, `Offset`, `Wait` and `Then` return a `Tween<T>`
- `To` uses the lerp of the step before it
- `FloatTween`, `Vector2Tween`, `ColorTween` and `WaitTween` extend `Tween<T>`
- `Loop()` lasts forever
- Long chains are faster to build and read
- `TweenHelper.UpdateSetup(GameTime)` moves the clock by the game time since the last call
- `Offset` on a `ColorTween` takes a `Vector4` that can be negative
- Circ easings follow a circle
- Expo easings are a bit steeper
- `BackInOut` and `ElasticInOut` have new shapes

### Removed

- Start times on tweens and the constructors that take one
- `Value`, `StartTime`, `A` and `B` on `ITween<T>`
- Setters on `A`, `B` and `Duration`
- `Interpolator` on `FloatTween`, `Vector2Tween` and `ColorTween`
- `ThenTween<T>`

### Fixed

- `Reverse` and `Yoyo` hold the right value before they start and after they end
- `Loop` on a tween with a duration of 0 doesn't throw
- `ExpoOut` and `ExpoInOut` reach their end value instead of jumping to it
- `ElasticInOut` doesn't jump in the middle

## [0.2.0] - 2026-01-07

### Added

- `Apos.Tweens.KNI` package for KNI

### Changed

- Targets .NET 8

## [0.1.4] - 2024-02-14

### Added

- `Easing.QuartInOut`

## [0.1.3] - 2022-01-27

### Changed

- Targets .NET Standard 2.0

## [0.1.2] - 2021-12-03

### Added

- `ColorTween`

## [0.1.1] - 2021-09-22

### Added

- Tween constructors that take a start time
- `TweenHelper.UpdateSetup(GameTime)`

### Changed

- Easing functions use the `Interpolator` delegate

## [0.1.0] - 2021-09-22

### Added

- `FloatTween` and `Vector2Tween`
- `Then`, `To`, `Offset`, `Wait`, `Loop`, `Reverse` and `Yoyo`
- Easing functions: Linear, Sine, Quad, Cube, Quart, Quint, Circ, Expo, Back, Elastic, Bounce

[Unreleased]: https://github.com/Apostolique/Apos.Tweens/compare/v0.2.0...HEAD
[0.2.0]: https://github.com/Apostolique/Apos.Tweens/compare/v0.1.4...v0.2.0
[0.1.4]: https://github.com/Apostolique/Apos.Tweens/compare/v0.1.3...v0.1.4
[0.1.3]: https://github.com/Apostolique/Apos.Tweens/compare/v0.1.2...v0.1.3
[0.1.2]: https://github.com/Apostolique/Apos.Tweens/compare/v0.1.1...v0.1.2
[0.1.1]: https://github.com/Apostolique/Apos.Tweens/compare/v0.1.0...v0.1.1
[0.1.0]: https://github.com/Apostolique/Apos.Tweens/releases/tag/v0.1.0
