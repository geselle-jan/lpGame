using System;
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
    public class Scene_Title : Scene
    {
        public Scene_Title(lpGame lpGame) : base(lpGame)
        {

        }

        public override void update(float deltaSeconds)
        {
            if (game.input.rightJustPressed)
                game.scene.setScene("Level");
            if (game.input.menuJustPressed)
                game.Exit();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (game.graphicsDeviceManager.GraphicsDevice != null)
            {
                spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
                spriteBatch.DrawString(game.graphics.font, "loafer-project", game.camera.getFixedPositionFor(new Vector2(8, 8)), Color.LimeGreen);
                spriteBatch.DrawString(game.graphics.font, "press -> to start", game.camera.getFixedPositionFor(new Vector2(8, 8 + game.graphics.font.LineHeight * 4)), Color.LimeGreen);
                spriteBatch.End();
            }
        }
    }
}
