using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace lp
{
    public class Graphics
    {
        public lpGame game;

        public BitmapFont font;

        public Graphics(lpGame lpGame)
        {
            game = lpGame;
        }

        public void init()
        {
            font = game.content.Load<BitmapFont>("emulogic-8");
        }

        public Texture2D getRectangleTexture(int width, int height, Color color)
        {
            Texture2D rect = new Texture2D(game.graphicsDeviceManager.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            rect.SetData(data);
            return rect;
        }
    }
}
