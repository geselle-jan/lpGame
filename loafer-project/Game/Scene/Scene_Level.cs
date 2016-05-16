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
            game.currentLevel = new Level_Test1(game);
            game.player = new Player(game);
            game.camera.followEntity(game.player);
            game.player.init();
            game.currentLevel.init();
        }

        public override void update(float deltaSeconds)
        {
            game.player.update(deltaSeconds);
            if (game.currentLevel.mapId == "test" && game.player.position == new Vector2(0, 240))
            {
                game.player.spawnPosition = new Vector2(43 * 16, 5 * 16);
                game.player.position = game.player.spawnPosition;
                game.currentLevel = new Level_Test2(game);
                game.currentLevel.init();
            }
            if (game.currentLevel.mapId == "debug_level" && game.player.position == new Vector2(704, 80))
            {
                game.player.spawnPosition = new Vector2(2 * 16, 31 * 16);
                game.player.position = new Vector2(16, 240);
                game.currentLevel = new Level_Test1(game);
                game.currentLevel.init();
            }
            if (game.input.menuJustPressed)
                game.scene.setScene("Title");
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp);
            game.currentLevel.drawBackground(spriteBatch);
            game.player.draw(spriteBatch);
            game.currentLevel.drawForeground(spriteBatch);
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
