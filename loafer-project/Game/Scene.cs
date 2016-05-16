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
    public class Scene
    {
        public lpGame game;

        public Scene(lpGame lpGame)
        {
            game = lpGame;
        }

        public virtual void init()
        {

        }

        public virtual void update(float deltaSeconds)
        {

        }

        public virtual void draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void destroy()
        {

        }
    }
}
