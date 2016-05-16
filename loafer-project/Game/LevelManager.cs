using System;
using System.Collections.Generic;
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
    public class LevelManager
    {
        public lpGame game;

        public List<Level> levels = new List<Level>();

        public string spawnLevel = "Test1";
        public Vector2 spawnPosition = new Vector2(6 * 16, 31 * 16);

        public LevelManager(lpGame lpGame)
        {
            game = lpGame;

            levels.Add(new Level_Test1(game));
            levels.Add(new Level_Test2(game));
            levels.Add(new Level_Test3(game));
        }

        public Level getLevel(string name)
        {
            foreach (var level in levels)
            {
                if (level.name == "Level_" + name)
                {
                    return level;
                }
            }
            return null;
        }

        public void setLevel(string name)
        {
            game.currentLevel = getLevel(name);
            game.currentLevel.init();
        }

        public void setToSpawn()
        {
            if (game.currentLevel == null || game.currentLevel.name != "Level_" + spawnLevel)
            {
                setLevel(spawnLevel);
            }
            game.player.position = spawnPosition;
        }

        public void init()
        {
            setToSpawn();
        }

        public void update(float deltaSeconds)
        {
            game.currentLevel.update(deltaSeconds);
        }

        public void drawBackground(SpriteBatch spriteBatch)
        {
            game.currentLevel.drawBackground(spriteBatch);
        }

        public void drawForeground(SpriteBatch spriteBatch)
        {
            game.currentLevel.drawForeground(spriteBatch);
        }
    }
}
