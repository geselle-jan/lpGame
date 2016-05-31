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
    public class Animation
    {
        public lpGame game;
        public string id;
        public List<int> frames = new List<int>();
        public int fps = 1;
        public bool loop;
        public int currentIndex = 0;
        public float timer = 0;
        public SpriteSheet spritesheet;
        public Vector2 offset = Vector2.Zero;
        public bool finished = false;

        public Animation(string id, List<int> frames, int fps, bool loop, Vector2 offset, SpriteSheet spritesheet, lpGame lpGame)
        {
            game = lpGame;
            this.spritesheet = spritesheet;
            this.offset = offset;
            this.loop = loop;
            this.id = id;
            this.frames = frames;
            this.fps = fps;

        }

        public int getFrame()
        {
            return frames[currentIndex];
        }

        public void reset()
        {
            finished = false;
            timer = 0;
            currentIndex = 0;
        }

        public void update(float deltaSeconds)
        {
            float fpsInSeconds = 1f / fps;
            timer += deltaSeconds;
            if (timer > fpsInSeconds)
            {
                currentIndex++;
                timer -= fpsInSeconds;
            }
            if (currentIndex >= frames.Count)
            {
                if (loop)
                {
                    currentIndex = 0;
                }
                else
                {
                    finished = true;
                    currentIndex = frames.Count - 1;
                }
            }
        }
    }
}
