using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class Player : Entity
    {
        public int speed = 120;
        public int acceleration = 900;
        public int jumpSpeed = 290;
        public int jumpAbortSpeed = 70;
        public bool jumpAborted = false;
        public bool canJump = true;
        public Vector2 spawnPosition = new Vector2(2 * 16, 31 * 16);

        public Player(lpGame lpGame) : base(lpGame)
        {
            textureId = "player";
            gravity = new Vector2(0, 600);
        }

        public new void init()
        {
            position = spawnPosition;

            base.init();
        }

        public new void update(float deltaSeconds)
        {
            if (game.input.rightPressed)
            {
                if (velocity.X < 0)
                {
                    velocity.X = 0;
                }
                if (velocity.X < speed)
                {
                    velocity.X += acceleration * deltaSeconds;
                }
                if (velocity.X > speed)
                {
                    velocity.X = speed;
                }
            }

            if (game.input.leftPressed)
            {
                if (velocity.X > 0)
                {
                    velocity.X = 0;
                }
                if (velocity.X > -speed)
                {
                    velocity.X += -acceleration * deltaSeconds;
                }
                if (velocity.X < -speed)
                {
                    velocity.X = -speed;
                }
            }

            if (!game.input.rightPressed && !game.input.leftPressed)
            {
                velocity.X = 0;
            }

            if (game.input.rightPressed && game.input.leftPressed)
            {
                velocity.X = 0;
            }

            if (onGround)
            {
                if (game.input.jumpPressed)
                {
                    if (canJump)
                    {
                        canJump = false;
                        jumpAborted = false;
                        velocity.Y = jumpSpeed * -1;
                    }
                }
                else
                {
                    canJump = true;
                }
            }
            else
            {
                if (!canJump && !jumpAborted && !game.input.jumpPressed)
                {
                    jumpAborted = true;
                    if (velocity.Y < -jumpAbortSpeed)
                    {
                        velocity.Y = -jumpAbortSpeed;
                    }
                }
            }

            base.update(deltaSeconds);

            if (game.input.resetPlayerJustPressed)
                position = spawnPosition;
        }

        public new void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
        }
    }
}
