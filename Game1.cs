using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PerceptionWorld.Utils;

namespace PerceptionWorld
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private State _state;

        private Vector2 _playerPosition = new Vector2(100, 100);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _state = new State(Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // Движение спрайта стрелками
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A))
                _playerPosition.X -= 2;
            if (keyboardState.IsKeyDown(Keys.D))
                _playerPosition.X += 2;
            if (keyboardState.IsKeyDown(Keys.W))
                _playerPosition.Y -= 2;
            if (keyboardState.IsKeyDown(Keys.S))
                _playerPosition.Y += 2;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Получаем текстуру через Resource
            var playerTexture = Resource.Texture("stone");

            // Отрисовываем спрайт
            _spriteBatch.Draw(
                playerTexture,
                _playerPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
            );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
