using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Sprites;

namespace lp
{
    public class Bullet : Entity
    {
        public string spawnedInLevel;

        public Bullet(lpGame lpGame) : base(lpGame)
        {
            game = lpGame;
            texture = game.graphics.getRectangleTexture(4, 4, Color.Red);
            physical = false;
            gravityAffected = false;
        }

        public new void update(float deltaSeconds)
        {
            if (collisionDirty || spawnedInLevel != game.currentLevel.name)
            {
                alive = false;
                collisionDirty = false;
            }
            base.update(deltaSeconds);
        }
    }
}
