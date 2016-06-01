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
    public class SpriteSheet
    {
        public lpGame game;
        public Vector2 size;
        public Vector2 frameSize;
        public string textureId;
        public Texture2D texture;
        public int framesPerRow;
        public int currentIndex = 0;
        public List<Animation> animations = new List<Animation>();
        public Animation currentAnimation;
        public Vector2 offset = Vector2.Zero;
        public Entity entity;

        public SpriteSheet(Entity entity, lpGame lpGame)
        {
            game = lpGame;
            this.entity = entity;
        }

        public void init(string contentId, Vector2 size)
        {
            textureId = contentId;
            frameSize = size;
            texture = game.content.Load<Texture2D>(textureId);
            size = new Vector2(texture.Width, texture.Height);
            framesPerRow = (int)Math.Floor(size.X / frameSize.X);
        }

        public Rectangle getFrameSource(int index)
        {
            var x = (index % framesPerRow) * frameSize.X;
            var y = Math.Floor(index * frameSize.X / (framesPerRow * frameSize.X)) * frameSize.Y;
            return new Rectangle((int)x, (int)y, (int)frameSize.X, (int)frameSize.Y);
        }

        public void play(string animationName)
        {
            foreach (var animation in animations)
            {
                if (animation.id == animationName)
                {
                    if (currentAnimation != animation)
                    {
                        animation.reset();
                        currentAnimation = animation;
                        if (entity != null)
                        {
                            entity.offset = currentAnimation.offset;
                        }
                    }
                }
            }
        }

        public void setSpriteOffset(Vector2 spriteOffset)
        {
            offset = spriteOffset;
        }

        public bool animationFinished()
        {
            if (currentAnimation == null)
                return false;
            return currentAnimation.finished;
        }

        public void update(float deltaSeconds)
        {
            if (currentAnimation != null)
            {
                currentAnimation.update(deltaSeconds);
                currentIndex = currentAnimation.getFrame();
            }
        }

        public void draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position + offset, null, getFrameSource(currentIndex));
        }
    }
}
