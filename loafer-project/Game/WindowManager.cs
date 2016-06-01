using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class WindowManager
    {
        public lpGame game;

        public bool windowDirty = false;
        public static Vector2 screenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        public Vector2 size = new Vector2(16 * 30, 16* 20);
        public bool changingFullscreen = false;

        public WindowManager(lpGame lpGame)
        {
            game = lpGame;
            game.graphicsDeviceManager = new GraphicsDeviceManager(game) { SynchronizeWithVerticalRetrace = true };
            game.graphicsDeviceManager.IsFullScreen = false;
            game.graphicsDeviceManager.PreferredBackBufferWidth = (int)size.X;
            game.graphicsDeviceManager.PreferredBackBufferHeight = (int)size.Y;
            game.Content.RootDirectory = "Content";
            game.IsMouseVisible = true;
            game.IsFixedTimeStep = true;
            game.TargetElapsedTime = TimeSpan.FromSeconds(1 / 240.0f);
        }

        public void init()
        {
            setGameSize(size);

            game.Window.Position = Point.Zero;
            game.Window.AllowUserResizing = true;
            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(clientSizeChanged);
        }

        public void update(float deltaSeconds)
        {
            if (game.input.toggleFullscreenJustPressed)
            {
                changingFullscreen = true;
                if (game.graphicsDeviceManager.IsFullScreen)
                {
                    game.graphicsDeviceManager.IsFullScreen = false;
                    game.IsMouseVisible = true;
                    setGameSize(size);
                }
                else
                {
                    game.graphicsDeviceManager.IsFullScreen = true;
                    game.IsMouseVisible = false;
                    setGameSize(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
                }
            }
            if (windowDirty)
            {
                handleSizeChange();
                changingFullscreen = false;
            }
        }

        public void clientSizeChanged(object sender, EventArgs e)
        {
            windowDirty = true;
        }

        public void handleSizeChange()
        {
            if (!changingFullscreen)
            {
                size = new Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
                setGameSize(size);
            }
            else
            {
                setGameSize(new Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height));
            }

            windowDirty = false;
        }

        public void setGameSize(Vector2 size)
        {
            game.graphicsDeviceManager.PreferredBackBufferWidth = (int)size.X;
            game.graphicsDeviceManager.PreferredBackBufferHeight = (int)size.Y;

            game.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth = game.graphicsDeviceManager.PreferredBackBufferWidth;
            game.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight = game.graphicsDeviceManager.PreferredBackBufferHeight;
            game.graphicsDeviceManager.GraphicsDevice.Viewport = new Viewport(0, 0, game.graphicsDeviceManager.PreferredBackBufferWidth, game.graphicsDeviceManager.PreferredBackBufferHeight);
            game.graphicsDeviceManager.ApplyChanges();

            game.camera.init();
        }
    }
}
