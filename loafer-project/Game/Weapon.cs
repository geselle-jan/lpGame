using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class Weapon
    {
        public lpGame game;
        public List<Bullet> bullets = new List<Bullet>();

        public Weapon(lpGame lpGame)
        {
            game = lpGame;
        }

        public void init()
        {
        }

        public void update(float deltaSeconds)
        {
            foreach (var bullet in bullets)
            {
                bullet.update(deltaSeconds);
            }
        }

        public void fire()
        {
            var bullet = getAvailableBullet();
            bullet.spawnedInLevel = game.currentLevel.name;
            bullet.position = game.player.position;
            bullet.velocity.X = 300 * game.player.direction;
            if (game.player.direction > 0)
            {
                bullet.position.X += 16;
            }
            bullet.position.Y += 9;
        }

        public Bullet getAvailableBullet()
        {
            foreach (var bullet in bullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    return bullet;
                }
            }
            bullets.Add(new Bullet(game));
            bullets[bullets.Count - 1].init();
            return bullets[bullets.Count - 1];
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (var bullet in bullets)
            {
                bullet.draw(spriteBatch);
            }
        }
    }
}
