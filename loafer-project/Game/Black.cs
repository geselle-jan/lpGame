using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class Black
    {
        public lpGame game;

        public Vector2 position = Vector2.Zero;
        public Vector2 size = Vector2.Zero;
        public Texture2D texture;
        public float opacity = 1f;
        public float transitionRate = 4;
        public int fade = 0;
        public bool fading
        {
            get { return fade != 0; }
        }

        public Black(Vector2 size, lpGame lpGame)
        {
            game = lpGame;
            this.size = size;
        }

        public void init()
        {
            texture = game.graphics.getRectangleTexture((int)size.X, (int)size.Y, Color.Black);
        }

        public void update(float deltaSeconds)
        {
            if (fade != 0)
            {
                opacity += deltaSeconds * transitionRate * fade;
            }
            if (opacity > 1)
            {
                opacity = 1;
                fade = 0;
            }
            if (opacity < 0)
            {
                opacity = 0;
                fade = 0;
                foreach (var door in game.currentLevel.doors)
                {
                    door.transitionTo = false;
                }
                game.paused = false;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White * opacity);
        }

        public void fadeIn()
        {
            fade = 1;
        }

        public void fadeOut()
        {
            fade = -1;
        }
    }
}
