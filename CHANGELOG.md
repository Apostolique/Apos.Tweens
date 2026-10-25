# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `Tween<T>` to tween any type with a lerp function, and a `To` overload that takes the lerp
- `TweenClock`, a clock that can be paused and slowed down with `Scale`
- A clock parameter on every tween constructor
- `TweenHelper.Clock`, the clock tweens use by default
- `Loop(count)` to play a tween a set number of times
- `Restart`, `Elapsed` and `IsDone`
- `StartTime` and `Clock` can be set on chains

### Changed

- Times and durations are `double` milliseconds instead of `long`
- `ITween<T>` has a `Clock` and a setter on `StartTime`
- `FloatTween`, `Vector2Tween` and `ColorTween` extend `Tween<T>`
- `Loop()` lasts forever
- Long chains are faster to read
- `TweenHelper.UpdateSetup(GameTime)` moves the clock by the game time since the last call
- `Offset` on a `ColorTween` takes a `Vector4` that can be negative
- Circ easings follow a circle
- Expo easings are a bit steeper
- `BackInOut` and `ElasticInOut` have new shapes

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
