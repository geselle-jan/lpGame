using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.ViewportAdapters;

namespace lp
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class lpGame : Game
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
        private Vector2 spawnPosition = new Vector2(2 * 16, 31 * 16);
        private int speed = 120;
        private int acceleration = 900;
        private int jumpSpeed = 290;
        private int jumpAbortSpeed = 70;
        private int gravity = 600;
        private Vector2 velocity = Vector2.Zero;
        private bool onGround = false;
        private bool wasOnSlope = false;
        private bool isOnSlope = false;
        private bool cameraFollow = true;
        private float zoom = 4;
        private bool windowDirty = false;
        private bool wasDownF11 = false;
        private bool jumpAborted = false;
        private bool canJump = true;
        private static Vector2 screenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        private Vector2 windowSize = screenSize / 2;
        private bool changingFullscreen = false;
        private float deltaSeconds = 0f;

        public lpGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this) { SynchronizeWithVerticalRetrace = true };
            _graphicsDeviceManager.IsFullScreen = false;
            _graphicsDeviceManager.PreferredBackBufferWidth = (int)windowSize.X;
            _graphicsDeviceManager.PreferredBackBufferHeight = (int)windowSize.Y;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 240.0f);
        }

        protected override void Initialize()
        {

            setGameSize(windowSize);
            
            Window.Position = Point.Zero;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            Components.Add(_fpsCounter = new FramesPerSecondCounterComponent(this));

            playerPosition = spawnPosition;

            System.Diagnostics.Debug.WriteLine("Initialized.");

            base.Initialize();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            windowDirty = true;
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
            deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
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



            if (keyboardState.IsKeyDown(Keys.Right))
            {
                if (velocity.X < 0)
                {
                    velocity.X = 0;
                }
                if (velocity.X < speed)
                {
                    velocity.X += acceleration * deltaSeconds;
                }
                if (velocity.X > speed)
                {
                    velocity.X = speed;
                }
            }
            
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                if (velocity.X > 0)
                {
                    velocity.X = 0;
                }
                if (velocity.X > -speed)
                {
                    velocity.X += -acceleration * deltaSeconds;
                }
                if (velocity.X < -speed)
                {
                    velocity.X = -speed;
                }
            }

            if (!keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
            {
                velocity.X = 0;
            }

            if (onGround)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    if (canJump)
                    {
                        canJump = false;
                        jumpAborted = false;
                        velocity.Y = jumpSpeed * -1;
                    }
                } else
                {
                    canJump = true;
                }
            } else
            {
                if (!canJump && !jumpAborted && !keyboardState.IsKeyDown(Keys.Up))
                {
                    jumpAborted = true;
                    if (velocity.Y < -jumpAbortSpeed)
                    {
                        velocity.Y = -jumpAbortSpeed;
                    }
                }
            }


            velocity += new Vector2(0, gravity * deltaSeconds * 1);

            playerPosition.Y = playerPosition.Y + (velocity.Y * deltaSeconds);
            
            collideY();

            playerPosition.X = playerPosition.X + (velocity.X * deltaSeconds);

            collideX();


            if (keyboardState.IsKeyDown(Keys.Q))
                speed = 10;

            if (keyboardState.IsKeyDown(Keys.E))
                speed = 100;



            if (keyboardState.IsKeyDown(Keys.T))
                cameraFollow = true;

            if (keyboardState.IsKeyDown(Keys.Y))
                cameraFollow = false;



            if (keyboardState.IsKeyDown(Keys.Z))
                playerPosition = spawnPosition;



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
                if (layer.Name == "background2")
                {
                    _spriteBatch.Draw(layer, _camera);
                }
            }

            foreach (var layer in _tiledMap.Layers)
            {
                if (layer.Name == "background1")
                {
                    _spriteBatch.Draw(layer, _camera);
                }
            }

            _spriteBatch.Draw(player, playerPosition);

            foreach (var layer in _tiledMap.Layers)
            {
                if (layer.Name == "foreground2")
                {
                    _spriteBatch.Draw(layer, _camera);
                }
            }

            foreach (var layer in _tiledMap.Layers)
            {
                if (layer.Name == "foreground1")
                {
                    _spriteBatch.Draw(layer, _camera);
                }
            }

            _spriteBatch.End();


            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            _spriteBatch.DrawString(_bitmapFont, $"Velocity X: {velocity.X}", new Vector2(5, 32), textColor);
            _spriteBatch.DrawString(_bitmapFont, $"Velocity Y: {velocity.Y}", new Vector2(5, 32 + _bitmapFont.LineHeight), textColor);
            _spriteBatch.DrawString(_bitmapFont, $"onGround: {onGround}", new Vector2(5, 32 + _bitmapFont.LineHeight * 2), textColor);
            _spriteBatch.DrawString(_bitmapFont, $"isOnSlope: {isOnSlope}", new Vector2(5, 32 + _bitmapFont.LineHeight * 3), textColor);
            _spriteBatch.DrawString(_bitmapFont, $"FPS: {_fpsCounter.AverageFramesPerSecond:0}", Vector2.One, Color.AliceBlue);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void collideY()
        {
            var velocityY = new Vector2(0, velocity.Y);
            isOnSlope = false;
            var collisionY = checkCollisionY();
            if (wasOnSlope && !isOnSlope)
            {
                playerPosition.Y += 1;
                collisionY = checkCollisionY();
            }
            onGround = false;
            if (collisionY != 0)
            {
                if (collisionY > 0)
                {
                    onGround = true;
                }

                velocity.Y = 0;
                playerPosition.Y -= collisionY;
            }
            wasOnSlope = isOnSlope;
        }

        private void collideX()
        {
            var velocityX = new Vector2(0, velocity.X);
            var collisionX = checkCollisionX();
            if (collisionX != 0)
            {
                playerPosition.X -= collisionX;
            }
        }

        private float checkCollisionY()
        {
            var collisionY = 0f;
            foreach (var tileLayer in _tiledMap.TileLayers)
            {
                if (tileLayer.Name == "collision")
                {
                    var tileSize = _tiledMap.TileHeight;
                    var playerHeight = player.Height;
                    var playerWidth = player.Width;
                    var currentX = 0;
                    var currentY = 0;
                    var highHalfSlopeCollided = false;


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
                            if (tileID == 1)
                            {
                                //_spriteBatch.Draw(highlight, new Vector2(playerX * tileSize, playerY * tileSize));
                                if (velocity.Y > 0) // falling down
                                {
                                    var intersection = (playerPosition.Y + playerHeight) - (playerY * tileSize);
                                    if (intersection > collisionY)
                                    {
                                        collisionY = intersection;
                                    }
                                }
                                else // jumping
                                {
                                    var intersection = (playerPosition.Y) - ((playerY * tileSize) + tileSize);
                                    if (intersection < collisionY)
                                    {
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 2)
                            {
                                if (velocity.Y > 0) // falling down
                                {
                                    var highestPointUnderPlayer = tileSize - (playerPosition.X + playerWidth - playerX * tileSize);
                                    if (highestPointUnderPlayer < 0)
                                    {
                                        highestPointUnderPlayer = 0;
                                    }
                                    if (highestPointUnderPlayer > tileSize)
                                    {
                                        highestPointUnderPlayer = tileSize;
                                    }
                                    var intersection = (playerPosition.Y + playerHeight) - (playerY * tileSize) - highestPointUnderPlayer;

                                    if (onGround && highestPointUnderPlayer != 0)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"intersection: {intersection}");
                                        playerPosition.Y = playerPosition.Y - intersection + 1;
                                        isOnSlope = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 3)
                            {
                                if (velocity.Y > 0) // falling down
                                {
                                    var highestPointUnderPlayer = tileSize - (playerX * tileSize + tileSize - playerPosition.X);
                                    if (highestPointUnderPlayer < 0)
                                    {
                                        highestPointUnderPlayer = 0;
                                    }
                                    if (highestPointUnderPlayer > tileSize)
                                    {
                                        highestPointUnderPlayer = tileSize;
                                    }
                                    var intersection = (playerPosition.Y + playerHeight) - (playerY * tileSize) - highestPointUnderPlayer;

                                    if (onGround && highestPointUnderPlayer != 0)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"intersection: {intersection}");
                                        playerPosition.Y = playerPosition.Y - intersection + 1;
                                        isOnSlope = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 4)
                            {
                                if (velocity.Y > 0 && !highHalfSlopeCollided) // falling down
                                {
                                    var highestPointUnderPlayer = tileSize - (playerPosition.X + playerWidth - playerX * tileSize);
                                    if (highestPointUnderPlayer < 0)
                                    {
                                        highestPointUnderPlayer = 0;
                                    }
                                    if (highestPointUnderPlayer > tileSize)
                                    {
                                        highestPointUnderPlayer = tileSize;
                                    }
                                    highestPointUnderPlayer = highestPointUnderPlayer / 2 + tileSize / 2;
                                    var intersection = (playerPosition.Y + playerHeight) - (playerY * tileSize) - highestPointUnderPlayer;

                                    if (onGround && highestPointUnderPlayer != 0)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"intersection: {intersection}");
                                        playerPosition.Y = playerPosition.Y - intersection + 1;
                                        isOnSlope = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 5)
                            {
                                if (velocity.Y > 0) // falling down
                                {
                                    var highestPointUnderPlayer = tileSize - (playerPosition.X + playerWidth - playerX * tileSize);
                                    if (highestPointUnderPlayer < 0)
                                    {
                                        highestPointUnderPlayer = 0;
                                    }
                                    if (highestPointUnderPlayer > tileSize)
                                    {
                                        highestPointUnderPlayer = tileSize;
                                    }
                                    highestPointUnderPlayer = highestPointUnderPlayer / 2;
                                    var intersection = (playerPosition.Y + playerHeight) - (playerY * tileSize) - highestPointUnderPlayer;

                                    if (onGround && highestPointUnderPlayer != 0)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"intersection: {intersection}");
                                        playerPosition.Y = playerPosition.Y - intersection + 1;
                                        isOnSlope = true;
                                        highHalfSlopeCollided = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 6)
                            {
                                if (velocity.Y > 0) // falling down
                                {
                                    var highestPointUnderPlayer = tileSize - (playerX * tileSize + tileSize - playerPosition.X);
                                    if (highestPointUnderPlayer < 0)
                                    {
                                        highestPointUnderPlayer = 0;
                                    }
                                    if (highestPointUnderPlayer > tileSize)
                                    {
                                        highestPointUnderPlayer = tileSize;
                                    }
                                    highestPointUnderPlayer = highestPointUnderPlayer / 2;
                                    var intersection = (playerPosition.Y + playerHeight) - (playerY * tileSize) - highestPointUnderPlayer;

                                    if (onGround && highestPointUnderPlayer != 0)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"intersection: {intersection}");
                                        playerPosition.Y = playerPosition.Y - intersection + 1;
                                        isOnSlope = true;
                                        highHalfSlopeCollided = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 7)
                            {
                                if (velocity.Y > 0 && !highHalfSlopeCollided) // falling down
                                {
                                    var highestPointUnderPlayer = tileSize - (playerX * tileSize + tileSize - playerPosition.X);
                                    if (highestPointUnderPlayer < 0)
                                    {
                                        highestPointUnderPlayer = 0;
                                    }
                                    if (highestPointUnderPlayer > tileSize)
                                    {
                                        highestPointUnderPlayer = tileSize;
                                    }
                                    highestPointUnderPlayer = highestPointUnderPlayer / 2 + tileSize / 2;
                                    var intersection = (playerPosition.Y + playerHeight) - (playerY * tileSize) - highestPointUnderPlayer;

                                    if (onGround && highestPointUnderPlayer != 0)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"intersection: {intersection}");
                                        playerPosition.Y = playerPosition.Y - intersection + 1;
                                        isOnSlope = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }

                            currentY = currentY + tileSize;
                        }

                        currentY = 0;
                        currentX = currentX + tileSize;
                    }
                }
            }

            return collisionY;
        }

        private float checkCollisionX()
        {
            var collisionX = 0f;
            foreach (var tileLayer in _tiledMap.TileLayers)
            {
                if (tileLayer.Name == "collision")
                {
                    var tileSize = _tiledMap.TileHeight;
                    var playerHeight = player.Height;
                    var playerWidth = player.Width;
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
                            if (tileID == 1)
                            {
                                //_spriteBatch.Draw(highlight, new Vector2(playerX * tileSize, playerY * tileSize));
                                if (velocity.X > 0) // going right
                                {
                                    var intersection = (playerPosition.X + playerWidth) - (playerX * tileSize);
                                    if (intersection > collisionX)
                                    {
                                        collisionX = intersection;
                                    }
                                }
                                else // going left
                                {
                                    var intersection = (playerPosition.X) - ((playerX * tileSize) + tileSize);
                                    if (intersection < collisionX)
                                    {
                                        collisionX = intersection;
                                    }
                                }
                            }

                            currentY = currentY + tileSize;
                        }

                        currentY = 0;
                        currentX = currentX + tileSize;
                    }
                }
            }

            return collisionX;
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