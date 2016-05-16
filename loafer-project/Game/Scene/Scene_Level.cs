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
        public Scene_Level(lpGame lpGame) : base(lpGame)
        {

        }

        public override void init()
        {
            game.player = new Player(game);
            game.levelManager = new LevelManager(game);
            game.camera.followEntity(game.player);
            game.player.init();
            game.levelManager.init();
            game.debug.log($"level-name: {game.currentLevel.name}");
        }

        public override void update(float deltaSeconds)
        {
            game.levelManager.update(deltaSeconds);
            game.player.update(deltaSeconds);
            if (game.input.menuJustPressed)
                game.scene.setScene("Title");
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp);
            game.levelManager.drawBackground(spriteBatch);
            game.player.draw(spriteBatch);
            game.levelManager.drawForeground(spriteBatch);
            game.debug.drawLevel(spriteBatch);
            spriteBatch.End();
        }

        public override void destroy()
        {
            game.camera.unfollowEntity();
            game.currentLevel = null;
            game.player = null;
        }
    }
}
