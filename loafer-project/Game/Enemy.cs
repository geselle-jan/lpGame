using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class Enemy : Entity
    {
        public bool turning = false;
        public int direction = 1;

        public Enemy(lpGame lpGame) : base(lpGame)
        {
            spriteSheet = new SpriteSheet(this, game);
            spriteSheet.init("enemy", new Vector2(16, 16));

            spriteSheet.animations.Add(new Animation("left", new List<int> { 0 }, 1, true, Vector2.Zero, spriteSheet, game));
            spriteSheet.animations.Add(new Animation("right", new List<int> { 4 }, 1, true, Vector2.Zero, spriteSheet, game));
            spriteSheet.animations.Add(new Animation("turnLeft", new List<int> { 3, 2, 1 }, 12, false, Vector2.Zero, spriteSheet, game));
            spriteSheet.animations.Add(new Animation("turnRight", new List<int> { 1, 2, 3 }, 12, false, Vector2.Zero, spriteSheet, game));

            spriteSheet.play("right");
            gravity = new Vector2(0, 600);
        }

        public new void init()
        {
            base.init();
            size = new Vector2(1 * 16, 1 * 16);
        }

        public new void update(float deltaSeconds)
        {
            if (!game.paused)
            {
                updateMovement(deltaSeconds);
            }

            base.update(deltaSeconds);
        }

        public new void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
        }

        public void updateMovement(float deltaSeconds)
        {
            velocity.X = 0;
            if (!turning && ( blockedLeft || blockedRight || (!isCompletelyOnGround() && !isOnSlope) ))
            {
                turning = true;
                direction *= -1;
                if (direction == 1)
                {
                    spriteSheet.play("turnRight");
                }
                else
                {
                    spriteSheet.play("turnLeft");
                }
            }
            if (spriteSheet.currentAnimation.id == "turnRight" && spriteSheet.animationFinished())
            {
                spriteSheet.play("right");
                turning = false;
            }
            if (spriteSheet.currentAnimation.id == "turnLeft" && spriteSheet.animationFinished())
            {
                spriteSheet.play("left");
                turning = false;
            }
            if (!turning)
            {
                velocity.X = 50 * direction;
            }
        }

        public bool isCompletelyOnGround()
        {
            foreach (var tileLayer in game.currentLevel.map.TileLayers)
            {
                if (tileLayer.Name == "collision")
                {
                    var tileSize = 16;
                    var x1 = (int)Math.Floor(position.X / tileSize);
                    var x2 = (int)Math.Floor((position.X + size.X) / tileSize);
                    var y = (int)Math.Floor(position.Y / tileSize);
                    var tile1 = tileLayer.GetTile(x1, y + 1);
                    var tile2 = tileLayer.GetTile(x2, y + 1);
                    return (tile1.Id != 0 && tile2.Id != 0) || (game.physics.isSlope(tile1) || game.physics.isSlope(tile2));
                }
            }
            return true;
        }

    }
}
