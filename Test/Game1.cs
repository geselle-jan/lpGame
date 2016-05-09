using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.ViewportAdapters;

namespace Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private FramesPerSecondCounterComponent _fpsCounter;
        private BitmapFont _bitmapFont;
        private Camera2D _camera;
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private TiledMap _tiledMap;
        private ViewportAdapter _viewportAdapter;
        private Texture2D player;
        private Texture2D highlight;
        private Texture2D backgroundImage;
        private Vector2 playerPosition;
        private int speed = 100;
        private int gravity = 5;
        private Vector2 velocity = Vector2.Zero;
        private bool isJumping = false;
        private bool cameraFollow = true;
        private float zoom = 4;
        private bool windowDirty = false;
        private bool wasDownF11 = false;
        private static Vector2 screenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        private Vector2 windowSize = screenSize / 2;
        private bool changingFullscreen = false;

        public Game1()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this) { SynchronizeWithVerticalRetrace = false };
            _graphicsDeviceManager.IsFullScreen = false;
            _graphicsDeviceManager.PreferredBackBufferWidth = (int)windowSize.X;
            _graphicsDeviceManager.PreferredBackBufferHeight = (int)windowSize.Y;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 120.0f);
        }

        protected override void Initialize()
        {

            setGameSize(windowSize);
            
            Window.Position = Point.Zero;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            Components.Add(_fpsCounter = new FramesPerSecondCounterComponent(this));

            playerPosition = new Vector2(2 * 16, 31 * 16);

            System.Diagnostics.Debug.WriteLine("Initialized.");

            base.Initialize();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            windowDirty = true;
            System.Diagnostics.Debug.WriteLine($"Window.ClientBounds.Width: {Window.ClientBounds.Width}");
            System.Diagnostics.Debug.WriteLine($"Window.ClientBounds.Height: {Window.ClientBounds.Height}");
        }

        void Window_HandleSizeChange()
        {
            if (!changingFullscreen)
            {
                windowSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
                setGameSize(windowSize);
            }
            else
            {
                setGameSize(new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height));
            }
            
            windowDirty = false;
        }

        void setGameSize(Vector2 size)
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = (int)size.X;
            _graphicsDeviceManager.PreferredBackBufferHeight = (int)size.Y;

            System.Diagnostics.Debug.WriteLine($"Window.ClientBounds.Width: {Window.ClientBounds.Width}");
            System.Diagnostics.Debug.WriteLine($"Window.ClientBounds.Height: {Window.ClientBounds.Height}");

            _graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth = _graphicsDeviceManager.PreferredBackBufferWidth;
            _graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight = _graphicsDeviceManager.PreferredBackBufferHeight;
            _graphicsDeviceManager.GraphicsDevice.Viewport = new Viewport(0, 0, _graphicsDeviceManager.PreferredBackBufferWidth, _graphicsDeviceManager.PreferredBackBufferHeight);
            _graphicsDeviceManager.ApplyChanges();

            _viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
            _camera = new Camera2D(_viewportAdapter);
            _camera.Zoom = zoom;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _bitmapFont = Content.Load<BitmapFont>("montserrat-32");

            _tiledMap = Content.Load<TiledMap>("test");

            player = Content.Load<Texture2D>("player");

            highlight = Content.Load<Texture2D>("highlight");

            backgroundImage = Content.Load<Texture2D>("sr388cave");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            const float cameraSpeed = 400f;
            const float zoomSpeed = 0.8f;

            if (keyboardState.IsKeyDown(Keys.W))
                _camera.Move(new Vector2(0, -cameraSpeed) * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.A))
                _camera.Move(new Vector2(-cameraSpeed, 0) * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.S))
                _camera.Move(new Vector2(0, cameraSpeed) * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.D))
                _camera.Move(new Vector2(cameraSpeed, 0) * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.R))
            {
                _camera.ZoomIn(zoomSpeed * deltaSeconds);
                zoom = _camera.Zoom;
            }

            if (keyboardState.IsKeyDown(Keys.F))
            {
                _camera.ZoomOut(zoomSpeed * deltaSeconds);
                zoom = _camera.Zoom;
            }


            velocity.X = 0;

            if (keyboardState.IsKeyDown(Keys.Right))
                velocity.X = speed * 1;

            if (keyboardState.IsKeyDown(Keys.Left))
                velocity.X = speed * -1;

            if (keyboardState.IsKeyDown(Keys.Up) && !isJumping)
            {
                isJumping = true;
                velocity.Y = 300 * -1;
            }




            velocity += new Vector2(0, gravity * 125 * deltaSeconds * 1);


            playerPosition = playerPosition + (velocity * deltaSeconds);
            collide(deltaSeconds);




            if (keyboardState.IsKeyDown(Keys.Q))
                speed = 10;

            if (keyboardState.IsKeyDown(Keys.E))
                speed = 100;



            if (keyboardState.IsKeyDown(Keys.T))
                cameraFollow = true;

            if (keyboardState.IsKeyDown(Keys.Y))
                cameraFollow = false;



            if (keyboardState.IsKeyDown(Keys.Z))
                playerPosition = new Vector2(2 * 16, 31 * 16);



            if (keyboardState.IsKeyDown(Keys.F11))
            {
                if (!wasDownF11)
                {
                    changingFullscreen = true;
                    if (_graphicsDeviceManager.IsFullScreen)
                    {
                        _graphicsDeviceManager.IsFullScreen = false;
                        IsMouseVisible = true;
                        setGameSize(windowSize);
                    }
                    else
                    {
                        _graphicsDeviceManager.IsFullScreen = true;
                        IsMouseVisible = false;
                        setGameSize(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
                    }
                }
                wasDownF11 = true;
            }
            else
            {
                wasDownF11 = false;
            }



            foreach (var layer in _tiledMap.TileLayers)
            {
                if (layer.Name == "collision")
                {
                    //layer.GetTile(39, 2).Id
                }
            }

            if (cameraFollow)
            {
                _camera.Position = playerPosition + new Vector2(player.Width, player.Height) / 2 - new Vector2(_camera.GetBoundingRectangle().Width, _camera.GetBoundingRectangle().Height) / 2 * zoom;
                var bounds = _camera.GetBoundingRectangle();

                if (Math.Floor(bounds.Width) <= _tiledMap.WidthInPixels && Math.Floor(bounds.Height) <= _tiledMap.HeightInPixels)
                {
                    var adjustment = Vector2.Zero;
                    if (bounds.Top < 0)
                    {
                        adjustment.Y = bounds.Top * -1;
                    }
                    if (bounds.Bottom > _tiledMap.HeightInPixels)
                    {
                        adjustment.Y = (bounds.Bottom - _tiledMap.HeightInPixels) * -1;
                    }
                    if (bounds.Right > _tiledMap.WidthInPixels)
                    {
                        adjustment.X = (bounds.Right - _tiledMap.WidthInPixels) * -1;
                    }
                    if (bounds.Left < 0)
                    {
                        adjustment.X = bounds.Left * -1;
                    }
                    _camera.Move(adjustment);
                }
            }

            if (windowDirty)
            {
                Window_HandleSizeChange();
                changingFullscreen = false;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_tiledMap.BackgroundColor ?? Color.Black);

            var textColor = Color.White;

            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);

            drawBackground(_spriteBatch);

            foreach (var layer in _tiledMap.Layers)
            {
                if (layer.Name == "collision")
                {
                    //_spriteBatch.Draw(layer, _camera);
                }
            }

            foreach (var layer in _tiledMap.Layers)
            {
                if (layer.Name == "deco2")
                {
                    _spriteBatch.Draw(layer, _camera);
                }
            }

            _spriteBatch.Draw(player, playerPosition);

            foreach (var layer in _tiledMap.Layers)
            {
                if (layer.Name == "deco1")
                {
                    _spriteBatch.Draw(layer, _camera);
                }
            }

            _spriteBatch.End();


            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            _spriteBatch.DrawString(_bitmapFont, "debug1", new Vector2(5, 32), textColor);
            _spriteBatch.DrawString(_bitmapFont, "debug2", new Vector2(5, 32 + _bitmapFont.LineHeight), textColor);
            _spriteBatch.DrawString(_bitmapFont, $"FPS: {_fpsCounter.AverageFramesPerSecond:0}", Vector2.One, Color.AliceBlue);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void collide(float deltaSeconds)
        {
            var velocityX = new Vector2(velocity.X, 0);
            var velocityY = new Vector2(0, velocity.Y);
            if (checkCollision())
            {
                playerPosition -= velocityX * deltaSeconds;
                if (checkCollision())
                {
                    playerPosition += velocityX * deltaSeconds;
                    playerPosition -= velocityY * deltaSeconds;

                    if (checkCollision())
                    {
                        playerPosition -= velocityX * deltaSeconds;
                        velocity.X = 0;
                        velocity.Y = 0;
                    }
                    else
                    {
                        if (velocity.Y > 0)
                        {
                            isJumping = false;
                        }
                        velocity.Y = 0;
                    }
                }
                else
                {
                    velocity.X = 0;
                }
            }
        }

        private bool checkCollision()
        {
            var collisions = 0;
            foreach (var tileLayer in _tiledMap.TileLayers)
            {
                if (tileLayer.Name == "collision")
                {
                    var tileSize = 16;
                    var playerHeight = 32;
                    var playerWidth = 16;
                    var currentX = 0;
                    var currentY = 0;


                    while (currentX <= playerWidth)
                    {
                        while (currentY <= playerHeight)
                        {
                            var playerX = (int)Math.Floor((playerPosition.X + currentX) / tileSize);
                            var playerY = (int)Math.Floor((playerPosition.Y + currentY) / tileSize);

                            if (currentX == playerWidth && playerPosition.X % tileSize == 0)
                            {
                                break;
                            }

                            if (currentY == playerHeight && playerPosition.Y % tileSize == 0)
                            {
                                break;
                            }

                            var tileID = tileLayer.GetTile(
                                playerX,
                                playerY
                            ).Id;
                            if (tileID != 0)
                            {
                                //_spriteBatch.Draw(highlight, new Vector2(playerX * tileSize, playerY * tileSize));
                                collisions++;
                            }

                            currentY = currentY + tileSize;
                        }

                        currentY = 0;
                        currentX = currentX + tileSize;
                    }
                }
            }

            return collisions != 0;
        }

        private Vector2 roundPosition (Vector2 position)
        {
            position.X = (float)Math.Round(position.X);
            position.Y = (float)Math.Round(position.Y);
            return position;
        }

        private void drawBackground (SpriteBatch _spriteBatch)
        {
            var height = _tiledMap.HeightInPixels;
            var width = _tiledMap.WidthInPixels;
            var drawPosition = Vector2.Zero;
            var offset = _camera.Position / 2;
            var xDraws = width / backgroundImage.Width + 1;
            var yDraws = height / backgroundImage.Height + 1;
            var xCounter = 0;
            var yCounter = 0;
            var xShift = Math.Ceiling(offset.X / backgroundImage.Width) * backgroundImage.Width;
            var yShift = Math.Ceiling(offset.Y / backgroundImage.Height) * backgroundImage.Height;
            var shift = new Vector2((float)xShift, (float)yShift);

            while (xCounter <= xDraws)
            {
                yCounter = 0;
                while (yCounter <= yDraws)
                {
                    drawPosition.X = xCounter * backgroundImage.Width;
                    drawPosition.Y = yCounter * backgroundImage.Height;
                    _spriteBatch.Draw(backgroundImage, drawPosition + offset - shift);
                    yCounter++;
                }
                xCounter++;
            }
        }
    }
}