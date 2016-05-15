//using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using MonoGame.Extended;
//using MonoGame.Extended.BitmapFonts;
//using MonoGame.Extended.Maps.Tiled;
//using MonoGame.Extended.Sprites;
//using Microsoft.Xna.Framework.Audio;

namespace lp
{
    public class lpGame : Game
    {
        public lpGame game;
        public WindowManager window;
        public GraphicsDeviceManager graphicsDeviceManager;
        public ContentManager content;
        public InputManager input;
        public CameraManager camera;
        public Physics physics;
        public Graphics graphics;
        public Debug debug;

        public Vector2 currentBounds;
        private SpriteBatch spriteBatch;
        private float deltaSeconds = 0f;

        public Level currentLevel;
        public Player player;

        //private SoundEffect song;
        
        public lpGame()
        {
            game = this;
            window = new WindowManager(game);
            content = Content;
            input = new InputManager(game);
            camera = new CameraManager(game);
            physics = new Physics(game);
            graphics = new Graphics(game);
            debug = new Debug(game);
            currentLevel = new Level_Test(game);
            player = new Player(game);
            camera.followEntity(player);
        }

        protected override void Initialize()
        {
            window.init();
            player.init();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.init();
            debug.init();
            currentLevel.init();

            //song = Content.Load<SoundEffect>("generic01");
            //var songInstance = song.CreateInstance();
            //songInstance.IsLooped = true;
            //songInstance.Play();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            game.input.update();
            player.update(deltaSeconds);
            debug.update(deltaSeconds);
            game.camera.update(deltaSeconds);
            window.update(deltaSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp);
            currentLevel.drawBackground(spriteBatch);
            player.draw(spriteBatch);
            currentLevel.drawForeground(spriteBatch);
            spriteBatch.End();
            debug.draw(spriteBatch);
            base.Draw(gameTime);
        }
        
    }
}