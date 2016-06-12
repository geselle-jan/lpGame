using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace lp
{
    public class UI
    {
        public lpGame game;
        
        public Texture2D healthBackground;
        public Texture2D healthForeground;
        public Color foregroundColor = Color.LimeGreen;
        public Color backgroundColor = Color.Red;

        public UI(lpGame lpGame)
        {
            game = lpGame;
        }

        public void init()
        {
            healthBackground = game.graphics.getRectangleTexture(100, 8, backgroundColor);
            healthForeground = game.graphics.getRectangleTexture(100, 8, foregroundColor);
        }

        public void update(float deltaSeconds)
        {
        }

        public void drawLevel(SpriteBatch spriteBatch)
        {
        }

        public void drawFixed(SpriteBatch spriteBatch)
        {
            if (game.scene.current is Scene_Level)
            {
                spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
                spriteBatch.Draw(healthBackground, position: game.camera.getFixedPositionFor(new Vector2(8, 8)));
                spriteBatch.Draw(healthForeground, position: game.camera.getFixedPositionFor(new Vector2(8, 8)), scale: new Vector2(game.player.health / 100f, 1));
                spriteBatch.End();
            }
        }

        public void log(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
    }
}
