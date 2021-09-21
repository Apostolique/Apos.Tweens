using System;
using Apos.Input;
using Apos.Tweens;
using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject {
    public class GameRoot : Game {
        public GameRoot() {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            Window.AllowUserResizing = true;

            base.Initialize();
        }

        protected override void LoadContent() {
            _s = new SpriteBatch(GraphicsDevice);
            _sb = new ShapeBatch(GraphicsDevice, Content);

            InputHelper.Setup(this);

            float left = 100f;
            float right = 700f;

            float top = 50f;
            long duration = 2000;
            long wait = 500;

            _tweens = new ITween<Vector2>[] {
                CreateTween(ref top, left, right, duration, wait, Easing.Linear),

                CreateTween(ref top, left, right, duration, wait, Easing.CircIn),
                CreateTween(ref top, left, right, duration, wait, Easing.SineIn),
                CreateTween(ref top, left, right, duration, wait, Easing.QuadIn),
                CreateTween(ref top, left, right, duration, wait, Easing.CubeIn),
                CreateTween(ref top, left, right, duration, wait, Easing.ExpoIn),
                CreateTween(ref top, left, right, duration, wait, Easing.QuintIn),
                CreateTween(ref top, left, right, duration, wait, Easing.BackIn),
                CreateTween(ref top, left, right, duration, wait, Easing.ElasticIn),
                CreateTween(ref top, left, right, duration, wait, Easing.BounceIn),

                CreateTween(ref top, left, right, duration, wait, Easing.CircInOut),
                CreateTween(ref top, left, right, duration, wait, Easing.SineInOut),
                CreateTween(ref top, left, right, duration, wait, Easing.QuadInOut),
                CreateTween(ref top, left, right, duration, wait, Easing.CubeInOut),
                CreateTween(ref top, left, right, duration, wait, Easing.ExpoInOut),
                CreateTween(ref top, left, right, duration, wait, Easing.QuintInOut),
                CreateTween(ref top, left, right, duration, wait, Easing.BackInOut),
                CreateTween(ref top, left, right, duration, wait, Easing.ElasticInOut),
                CreateTween(ref top, left, right, duration, wait, Easing.BounceInOut),

                CreateTween(ref top, left, right, duration, wait, Easing.CircOut),
                CreateTween(ref top, left, right, duration, wait, Easing.SineOut),
                CreateTween(ref top, left, right, duration, wait, Easing.QuadOut),
                CreateTween(ref top, left, right, duration, wait, Easing.CubeOut),
                CreateTween(ref top, left, right, duration, wait, Easing.ExpoOut),
                CreateTween(ref top, left, right, duration, wait, Easing.QuintOut),
                CreateTween(ref top, left, right, duration, wait, Easing.BackOut),
                CreateTween(ref top, left, right, duration, wait, Easing.ElasticOut),
                CreateTween(ref top, left, right, duration, wait, Easing.BounceOut),
            };
        }

        private ITween<Vector2> CreateTween(ref float top, float left, float right, long duration, long wait, Func<float, float> interpolator) {
            float t = top;
            top += 30;
            return
                new Vector2Tween(new Vector2(left, t), new Vector2(right, t), duration, interpolator)
                    .Wait(wait)
                    .To(new Vector2(left, t), duration, interpolator)
                    .Wait(wait)
                    .Loop();
        }

        protected override void Update(GameTime gameTime) {
            InputHelper.UpdateSetup();
            TweenHelper.UpdateSetup(gameTime.TotalGameTime.Ticks / TimeSpan.TicksPerMillisecond);

            if (_quit.Pressed())
                Exit();

            // TODO: Add your update logic here

            InputHelper.UpdateCleanup();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _sb.Begin();
            foreach (var f in _tweens) {
                _sb.DrawCircle(f.Value, 10f, Color.White, Color.Black, 2f);
            }
            _sb.End();

            base.Draw(gameTime);
        }

        GraphicsDeviceManager _graphics;
        SpriteBatch _s;
        ShapeBatch _sb;

        ICondition _quit =
            new AnyCondition(
                new KeyboardCondition(Keys.Escape),
                new GamePadCondition(GamePadButton.Back, 0)
            );

        ITween<Vector2>[] _tweens;
    }
}
