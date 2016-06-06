﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class Entity
    {
        public lpGame game;

        public Vector2 position = Vector2.Zero;
        public Vector2 size = Vector2.Zero;
        public Vector2 offset = Vector2.Zero;
        public Vector2 gravity = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public bool alive = true;
        public bool physical = true;
        public bool collisionDirty = false;
        public bool blockedLeft = false;
        public bool blockedRight = false;
        public bool onGround = false;
        public bool wasOnSlope = false;
        public bool isOnSlope = false;
        public bool gravityAffected = true;
        public Texture2D texture;
        public string textureId;
        public SpriteSheet spriteSheet;

        public Entity(lpGame lpGame)
        {
            game = lpGame;
        }

        public void init()
        {
            if (spriteSheet != null)
            {
                size = spriteSheet.frameSize;
            }
            else
            {
                if (texture == null)
                {
                    texture = game.content.Load<Texture2D>(textureId);
                }
                size = new Vector2(texture.Width, texture.Height);
            }
        }

        public void update(float deltaSeconds)
        {
            if (!game.paused && alive)
            {
                if (spriteSheet != null)
                {
                    spriteSheet.update(deltaSeconds);
                }

                if (gravityAffected)
                {
                    velocity += gravity * deltaSeconds * 1;
                }
                position.Y = position.Y + (velocity.Y * deltaSeconds);
                game.physics.collideY(this, game.currentLevel.map);
                position.X = position.X + (velocity.X * deltaSeconds);
                game.physics.collideX(this, game.currentLevel.map);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                if (spriteSheet != null)
                {
                    spriteSheet.draw(spriteBatch, position - offset);
                }
                else
                {
                    spriteBatch.Draw(texture, position - offset);
                }
            }
        }

    }
}
