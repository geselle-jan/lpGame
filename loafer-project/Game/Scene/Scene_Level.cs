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
    public class Scene_Level : Scene
    {
        public Debug debug;

        public Scene_Level(lpGame lpGame) : base(lpGame)
        {

        }

        public override void init()
        {
            debug = new Debug(game);
            game.currentLevel = new Level_Test1(game);
            game.player = new Player(game);
            game.camera.followEntity(game.player);
            game.player.init();
            debug.init();
            game.currentLevel.init();
        }

        public override void update(float deltaSeconds)
        {
            game.player.update(deltaSeconds);
            debug.update(deltaSeconds);
            //if (game.player.position == )
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp);
            game.currentLevel.drawBackground(spriteBatch);
            game.player.draw(spriteBatch);
            game.currentLevel.drawForeground(spriteBatch);
            spriteBatch.End();
            debug.draw(spriteBatch);
        }

        public override void destroy()
        {
            game.camera.unfollowEntity();
            game.currentLevel = null;
            game.player = null;
        }
    }
}
