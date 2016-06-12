﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Audio;

namespace lp
{
    public class lpGame : Game
    {
        public bool paused = true;
        public lpGame game;
        public WindowManager window;
        public GraphicsDeviceManager graphicsDeviceManager;
        public ContentManager content;
        public InputManager input;
        public CameraManager camera;
        public Graphics graphics;
        public Debug debug;
        public UI ui;
        public Physics physics;
        public SceneManager scene;
        public Level currentLevel;
        public Player player;
        public LevelManager levelManager;

        public Vector2 currentBounds;
        private SpriteBatch spriteBatch;
        private float deltaSeconds = 0f;

        //private SoundEffect song;

        public lpGame()
        {
            game = this;
            window = new WindowManager(game);
            content = Content;
            input = new InputManager(game);
            camera = new CameraManager(game);
            graphics = new Graphics(game);
            debug = new Debug(game);
            ui = new UI(game);
            physics = new Physics(game);
            scene = new SceneManager(game);
        }

        protected override void Initialize()
        {
            window.init();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.init();
            debug.init();
            ui.init();
            scene.setScene("Title");
            base.Initialize();
        }

        protected override void LoadContent()
        {
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
            if (game.input.togglePauseJustPressed)
            {
                game.paused = !game.paused;
            }
            deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            game.input.update();
            scene.update(deltaSeconds);
            game.camera.update(deltaSeconds);
            debug.update(deltaSeconds);
            ui.update(deltaSeconds);
            window.update(deltaSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            scene.draw(spriteBatch);
            ui.drawFixed(spriteBatch);
            debug.drawFixed(spriteBatch);
            base.Draw(gameTime);
        }
        
    }
}