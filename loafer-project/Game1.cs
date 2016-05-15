using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Audio;

namespace lp
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class lpGame : Game
    {
        private FramesPerSecondCounterComponent _fpsCounter;
        private BitmapFont _bitmapFont;
        private SpriteBatch _spriteBatch;
        private TiledMap _tiledMap;
        private Texture2D player;
        private Texture2D highlight;
        private Texture2D backgroundImage;
        private Texture2D debugOuter;
        private Texture2D debugInner;
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
        private bool windowDirty = false;
        private bool jumpAborted = false;
        private bool canJump = true;
        private static Vector2 screenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        private Vector2 windowSize = screenSize / 2;
        private bool changingFullscreen = false;
        private float deltaSeconds = 0f;
        private bool showDebug = false;
        //private SoundEffect song;




        public lpGame game;
        public GraphicsDeviceManager graphicsDeviceManager;
        public InputManager input;
        public CameraManager camera;

        public Vector2 currentBounds;


        public lpGame()
        {
            game = this;
            graphicsDeviceManager = new GraphicsDeviceManager(this) { SynchronizeWithVerticalRetrace = true };
            input = new InputManager(game);
            camera = new CameraManager(game);
            
            graphicsDeviceManager.IsFullScreen = false;
            graphicsDeviceManager.PreferredBackBufferWidth = (int)windowSize.X;
            graphicsDeviceManager.PreferredBackBufferHeight = (int)windowSize.Y;

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
            graphicsDeviceManager.PreferredBackBufferWidth = (int)size.X;
            graphicsDeviceManager.PreferredBackBufferHeight = (int)size.Y;

            System.Diagnostics.Debug.WriteLine($"Window.ClientBounds.Width: {Window.ClientBounds.Width}");
            System.Diagnostics.Debug.WriteLine($"Window.ClientBounds.Height: {Window.ClientBounds.Height}");

            graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth = graphicsDeviceManager.PreferredBackBufferWidth;
            graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight = graphicsDeviceManager.PreferredBackBufferHeight;
            graphicsDeviceManager.GraphicsDevice.Viewport = new Viewport(0, 0, graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight);
            graphicsDeviceManager.ApplyChanges();

            camera.init();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _bitmapFont = Content.Load<BitmapFont>("emulogic-8");

            _tiledMap = Content.Load<TiledMap>("test");

            currentBounds = new Vector2(_tiledMap.WidthInPixels, _tiledMap.HeightInPixels);

            player = Content.Load<Texture2D>("player");

            highlight = Content.Load<Texture2D>("highlight");

            backgroundImage = Content.Load<Texture2D>("sr388cave");


            //song = Content.Load<SoundEffect>("generic01");
            //var songInstance = song.CreateInstance();
            //songInstance.IsLooped = true;
            //songInstance.Play();

            debugOuter = getDrawRectangle(16 * 12, 16 + _bitmapFont.LineHeight * 5, Color.LimeGreen);
            debugInner = getDrawRectangle(16 * 12 - 2, 16 + _bitmapFont.LineHeight * 5 - 2, Color.Black);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;


            game.input.update();

            if (game.input.menuJustPressed)
                Exit();

            const float cameraSpeed = 400f;
            const float zoomSpeed = 0.8f;

            if (game.input.cameraUpPressed)
                camera.move(new Vector2(0, -cameraSpeed) * deltaSeconds);

            if (game.input.cameraLeftPressed)
                camera.move(new Vector2(-cameraSpeed, 0) * deltaSeconds);

            if (game.input.cameraDownPressed)
                camera.move(new Vector2(0, cameraSpeed) * deltaSeconds);

            if (game.input.cameraRightPressed)
                camera.move(new Vector2(cameraSpeed, 0) * deltaSeconds);

            if (game.input.zoomInPressed)
            {
                camera.zoomIn(zoomSpeed * deltaSeconds);
            }

            if (game.input.zoomOutPressed)
            {
                camera.zoomOut(zoomSpeed * deltaSeconds);
            }



            if (game.input.rightPressed)
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
            
            if (game.input.leftPressed)
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

            if (!game.input.rightPressed && !game.input.leftPressed)
            {
                velocity.X = 0;
            }

            if (game.input.rightPressed && game.input.leftPressed)
            {
                velocity.X = 0;
            }

            if (onGround)
            {
                if (game.input.jumpPressed)
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
                if (!canJump && !jumpAborted && !game.input.jumpPressed)
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




            if (game.input.toggleTrackingJustPressed)
                cameraFollow = !cameraFollow;



            if (game.input.resetPlayerJustPressed)
                playerPosition = spawnPosition;



            if (game.input.toggleFullscreenJustPressed)
            {
                changingFullscreen = true;
                if (graphicsDeviceManager.IsFullScreen)
                {
                    graphicsDeviceManager.IsFullScreen = false;
                    IsMouseVisible = true;
                    setGameSize(windowSize);
                }
                else
                {
                    graphicsDeviceManager.IsFullScreen = true;
                    IsMouseVisible = false;
                    setGameSize(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
                }
            }



            if (game.input.toggleDebugJustPressed)
                showDebug = !showDebug;



            foreach (var layer in _tiledMap.TileLayers)
            {
                if (layer.Name == "collision")
                {
                    //layer.GetTile(39, 2).Id
                }
            }

            if (cameraFollow)
            {
                camera.focus(playerPosition + new Vector2(player.Width, player.Height) / 2);
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

            var textColor = Color.LimeGreen;

            _spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp);

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
                    _spriteBatch.Draw(layer, game.camera.instance);
                }
            }

            foreach (var layer in _tiledMap.Layers)
            {
                if (layer.Name == "background1")
                {
                    _spriteBatch.Draw(layer, game.camera.instance);
                }
            }

            _spriteBatch.Draw(player, playerPosition);

            foreach (var layer in _tiledMap.Layers)
            {
                if (layer.Name == "foreground2")
                {
                    _spriteBatch.Draw(layer, game.camera.instance);
                }
            }

            foreach (var layer in _tiledMap.Layers)
            {
                if (layer.Name == "foreground1")
                {
                    _spriteBatch.Draw(layer, game.camera.instance);
                }
            }

            _spriteBatch.End();


            if (showDebug && graphicsDeviceManager.GraphicsDevice != null)
            {
                _spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
                _spriteBatch.Draw(debugOuter, game.camera.getFixedPositionFor(Vector2.Zero));
                _spriteBatch.Draw(debugInner, game.camera.getFixedPositionFor(Vector2.One));
                _spriteBatch.DrawString(_bitmapFont, $"FPS: {_fpsCounter.AverageFramesPerSecond:0}", game.camera.getFixedPositionFor(new Vector2(8, 8)), textColor);
                _spriteBatch.DrawString(_bitmapFont, $"Velocity X: {velocity.X}", game.camera.getFixedPositionFor(new Vector2(8, 8 + _bitmapFont.LineHeight * 1)), textColor);
                _spriteBatch.DrawString(_bitmapFont, $"Velocity Y: {velocity.Y}", game.camera.getFixedPositionFor(new Vector2(8, 8 + _bitmapFont.LineHeight * 2)), textColor);
                _spriteBatch.DrawString(_bitmapFont, $"onGround: {onGround}", game.camera.getFixedPositionFor(new Vector2(8, 8 + _bitmapFont.LineHeight * 3)), textColor);
                _spriteBatch.DrawString(_bitmapFont, $"isOnSlope: {isOnSlope}", game.camera.getFixedPositionFor(new Vector2(8, 8 + _bitmapFont.LineHeight * 4)), textColor);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private Texture2D getDrawRectangle (int width, int height, Color color)
        {
            Texture2D rect = new Texture2D(graphicsDeviceManager.GraphicsDevice, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            rect.SetData(data);

            return rect;
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
            var offset = game.camera.getPosition() / 2;
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