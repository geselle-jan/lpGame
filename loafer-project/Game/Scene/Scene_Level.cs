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
            if (game.currentLevel.mapId == "test" && game.player.position == new Vector2(0, 240))
            {
                game.player.position = new Vector2(43 * 16, 5 * 16);
                game.levelManager.setLevel("Test2");
            }
            if (game.currentLevel.mapId == "debug_level" && game.player.position == new Vector2(704, 80))
            {
                game.player.position = new Vector2(0, 240);
                game.levelManager.setLevel("Test1");
            }
            if (game.input.menuJustPressed)
                game.scene.setScene("Title");
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp);
            game.levelManager.drawBackground(spriteBatch);
            game.player.draw(spriteBatch);
            game.levelManager.drawForeground(spriteBatch);
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
