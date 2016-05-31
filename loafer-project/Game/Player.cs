using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class Player : Entity
    {
        public bool canJump = true;
        public bool vJump = false;
        public bool hJump = false;
        public bool falling = false;
        public bool turning = false;
        public bool gravFree = false;
        public bool powerGrip = false;
        public int direction = 1;

        public Player(lpGame lpGame) : base(lpGame)
        {
            spriteSheet = new SpriteSheet(this, game);
            spriteSheet.init("samus", new Vector2(50, 50));

            spriteSheet.animations.Add(new Animation("standLeft", new List<int> { 18, 17, 16 }, 3, true, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("standRight", new List<int> { 21, 22, 23 }, 3, true, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("turnLeft", new List<int> { 20, 19 }, 20, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("turnRight", new List<int> { 19, 20 }, 20, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("walkLeft", new List<int> { 51, 50, 49, 48, 59, 58, 57, 56, 67, 66 }, 20, true, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("walkRight", new List<int> { 52, 53, 54, 55, 60, 61, 62, 63, 68, 69 }, 20, true, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("hJumpLeft", new List<int> { 259, 258, 257, 256, 267, 266, 265, 264 }, 30, true, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("hJumpRight", new List<int> { 260, 261, 262, 263, 268, 269, 270, 271 }, 30, true, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("powerGripLeft", new List<int> { 329, 330, 331, 330 }, 3, true, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("powerGripRight", new List<int> { 332, 333, 334, 333 }, 3, true, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("gripClimbLeft", new List<int> { 328, 339, 338, 337, 336, 347, 346 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("gripClimbRight", new List<int> { 335, 340, 341, 342, 343, 348, 349 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("grabEdgeLeft", new List<int> { 328 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("grabEdgeRight", new List<int> { 335 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("vJumpLeft", new List<int> { 187, 186, 185 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("vJumpRight", new List<int> { 188, 189, 190 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("fallLeft", new List<int> { 184, 195, 194 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("fallRight", new List<int> { 191, 196, 197 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("landLeft", new List<int> { 193, 192 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("landRight", new List<int> { 198, 199 }, 15, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("airTurnLeft", new List<int> { 250, 249 }, 20, false, new Vector2(17, 14), spriteSheet, game));
            spriteSheet.animations.Add(new Animation("airTurnRight", new List<int> { 249, 250 }, 20, false, new Vector2(17, 14), spriteSheet, game));
            
            spriteSheet.play("standRight");
            gravity = new Vector2(0, 600);
        }

        public new void init()
        {
            base.init();
            size = new Vector2(1 * 16, 2 * 16);
        }

        public new void update(float deltaSeconds)
        {
            if (!game.paused)
            {
                updateMovement();
            }
            
            base.update(deltaSeconds);

            if (game.input.resetPlayerJustPressed && !game.paused)
                game.levelManager.setToSpawn();

        }

        public new void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
        }

        public bool isExclusiveDirection()
        {
            return (
                (game.input.leftPressed && !game.input.rightPressed)
                ||
                (game.input.rightPressed && !game.input.leftPressed)
            );
        }
        
        public void updateMovement()
        {
            if (onGround && (vJump || hJump || falling)) {
                landing();
            }
            if (!gravFree)
            {
                if (game.input.aPressed && onGround && canJump) {
                    jump();
                }
                else if (isExclusiveDirection() && onGround)
                {
                    if (game.input.leftPressed)
                    {
                        walkLeft();
                    }
                    else
                    {
                        walkRight();
                    }
                }
                else if (onGround)
                {
                    if (direction == -1) {
                        standLeft();
                    } else {
                        standRight();
                    }
                }
                else if (!onGround && !falling && !hJump)
                {
                    if (direction == -1) {
                        airStandLeft();
                    } else {
                        airStandRight();
                    }
                }
            }
            if (!game.input.aPressed && onGround)
            {
                canJump = true;
            }
            if (!game.input.aPressed && !onGround && !canJump)
            {
                if (velocity.Y < -50)
                {
                    velocity.Y = -50;
                }
            }
            if (hJump) {
                jumpH();
            }
            if (velocity.Y > 0 && isExclusiveDirection())
            {
                //grabEdge();
            }
            if (powerGrip) {
                //powerGrip();
            } else {
                if (vJump || falling) {
                    if (isExclusiveDirection())
                    {
                        if (game.input.leftPressed)
                        {
                            airWalkLeft();
                        }
                        else
                        {
                            airWalkRight();
                        }
                    }
                    else
                    {
                        if (direction == -1) {
                            airStandLeft();
                        } else {
                            airStandRight();
                        }
                    }
                }
            }
            if (velocity.Y > 400)
            {
                velocity.Y = 400;
            }
        }


        public void landing()
        {
            if (hJump) {
                if (direction == -1) {
                    if (game.input.leftPressed)
                    {
                        spriteSheet.play("walkLeft");
                    }
                    else
                    {
                        spriteSheet.play("standLeft");
                    }
                } else {
                    if (game.input.rightPressed)
                    {
                        spriteSheet.play("walkRight");
                    }
                    else
                    {
                        spriteSheet.play("standRight");
                    }
                }
                hJump = false;
            } else if (vJump || falling) {
                vJump = false;
                falling = false;
                if (direction == -1) {
                    spriteSheet.play("landLeft");
                } else {
                    spriteSheet.play("landRight");
                }
            }
        }

        public void walkLeft()
        {
            if (direction != -1)
            {
                direction = -1;
                turning = true;
                spriteSheet.play("turnLeft");
            }
            if (turning && spriteSheet.animationFinished()) {
                turning = false;
            }
            if (!turning)
            {
                if (velocity.X > 0)
                {
                    velocity.X = 0;
                }
                if (velocity.X > -180)
                {
                    velocity.X += -15;
                }
                if (velocity.X < -180)
                {
                    velocity.X = -180;
                }
                spriteSheet.play("walkLeft");
            }
        }

        public void walkRight()
        {
            if (direction != 1)
            {
                direction = 1;
                turning = true;
                spriteSheet.play("turnRight");
            }
            if (turning && spriteSheet.animationFinished()) {
                turning = false;
            }
            if (!turning)
            {
                if (velocity.X < 0)
                {
                    velocity.X = 0;
                }
                if (velocity.X < 180)
                {
                    velocity.X += 15;
                }
                if (velocity.X > 180)
                {
                    velocity.X = 180;
                }
                spriteSheet.play("walkRight");
            }
        }

        public void standLeft()
        {
            velocity.X = 0;
            if (spriteSheet.currentAnimation.id != "standLeft" && spriteSheet.currentAnimation.id != "landLeft" && !turning)
            {
                spriteSheet.play("standLeft");
            }
            if (turning && spriteSheet.animationFinished()) {
                turning = false;
                spriteSheet.play("standLeft");

            }
            if (spriteSheet.currentAnimation.id == "landLeft" && spriteSheet.animationFinished())
            {
                spriteSheet.play("standLeft");

            }
        }

        public void standRight()
        {
            velocity.X = 0;
            if (spriteSheet.currentAnimation.id != "standRight" && spriteSheet.currentAnimation.id != "landRight" && !turning)
            {
                spriteSheet.play("standRight");
            }
            if (turning && spriteSheet.animationFinished()) {
                turning = false;
                spriteSheet.play("standRight");
            }
            if (spriteSheet.currentAnimation.id == "landRight" && spriteSheet.animationFinished())
            {
                spriteSheet.play("standRight");

            }
        }

        public void airStandLeft()
        {
            if (velocity.Y < 0 || !falling)
            {
                if (spriteSheet.currentAnimation.id != "vJumpLeft" && !turning)
                {
                    spriteSheet.play("vJumpLeft");
                }
            }
            else if (!turning)
            {
                if (velocity.Y >= 0 && spriteSheet.currentAnimation.id != "fallLeft")
                {
                    spriteSheet.play("fallLeft");
                }
            }
            if (!falling)
            {
                falling = true;
                canJump = false;
                velocity.X = -30;
            }
            else
            {
                velocity.X = 0;
            }
            if (turning && spriteSheet.animationFinished()) {
                turning = false;
                spriteSheet.play("vJumpLeft");
            }
        }

        public void airStandRight()
        {
            if (velocity.Y < 0 || !falling)
            {
                if (spriteSheet.currentAnimation.id != "vJumpRight" && !turning)
                {
                    spriteSheet.play("vJumpRight");
                }
            }
            else if (!turning)
            {
                if (velocity.Y >= 0 && spriteSheet.currentAnimation.id != "fallRight")
                {
                    spriteSheet.play("fallRight");
                }
            }
            if (!falling)
            {
                falling = true;
                canJump = false;
                velocity.X = 30;
            }
            else
            {
                velocity.X = 0;
            }
            if (turning && spriteSheet.animationFinished())
            {
                turning = false;
                spriteSheet.play("vJumpRight");
            }
        }

        public void airWalkLeft()
        {
            if (direction != -1)
            {
                direction = -1;
                turning = true;
                spriteSheet.play("airTurnLeft");
            }
            if (turning && spriteSheet.animationFinished()) {
                turning = false;
                spriteSheet.play("vJumpLeft");
            }
            if (!turning)
            {
                if (velocity.X > 0)
                {
                    velocity.X = 0;
                }
                if (velocity.X > -95)
                {
                    velocity.X += -15;
                }
                if (velocity.X < -95)
                {
                    velocity.X = -95;
                }
                if (velocity.Y < 0 && spriteSheet.currentAnimation.id != "vJumpLeft")
                {
                    spriteSheet.play("vJumpLeft");
                }
                else if (velocity.Y >= 0 && spriteSheet.currentAnimation.id != "fallLeft")
                {
                    spriteSheet.play("fallLeft");
                }
            }
        }

        public void airWalkRight()
        {
            if (direction != 1)
            {
                direction = 1;
                turning = true;
                spriteSheet.play("airTurnRight");
            }
            if (turning && spriteSheet.animationFinished())
            {
                turning = false;
                spriteSheet.play("vJumpRight");
            }
            if (!turning)
            {
                if (velocity.X < 0)
                {
                    velocity.X = 0;
                }
                if (velocity.X < 95)
                {
                    velocity.X += 15;
                }
                if (velocity.X > 95)
                {
                    velocity.X = 95;
                }
                if (velocity.Y < 0 && spriteSheet.currentAnimation.id != "vJumpRight")
                {
                    spriteSheet.play("vJumpRight");
                }
                else if (velocity.Y >= 0 && spriteSheet.currentAnimation.id != "fallRight")
                {
                    spriteSheet.play("fallRight");
                }
            }
        }

        public void jumpH()
        {
            if (direction == -1) {
                if (isExclusiveDirection() && game.input.rightPressed)
                {
                    velocity.X = velocity.X * -1;
                    direction = 1;
                    turning = false;
                    spriteSheet.play("hJumpRight");
                }
                else
                {
                    velocity.X = -90;
                }
            } else {
                if (isExclusiveDirection() && game.input.leftPressed)
                {
                    velocity.X = velocity.X * -1;
                    direction = -1;
                    turning = false;
                    spriteSheet.play("hJumpLeft");
                }
                else
                {
                    velocity.X = 90;
                }
            }
        }

        public void jump()
        {
            canJump = false;
            if (isExclusiveDirection())
            {
                hJump = true;
                velocity.Y = -290;
                if (game.input.leftPressed)
                {
                    velocity.X = -90;
                    direction = -1;
                    turning = false;
                    spriteSheet.play("hJumpLeft");
                }
                else
                {
                    velocity.X = 90;
                    direction = 1;
                    turning = false;
                    spriteSheet.play("hJumpRight");
                }
            }
            else
            {
                velocity.Y = -300;

                if (direction == -1)
                {
                    airStandLeft();
                }
                else
                {
                    airStandRight();
                }

                vJump = true;
            }
        }

        

    }
}
