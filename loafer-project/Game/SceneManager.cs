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
    public class SceneManager
    {
        public lpGame game;

        public Scene current;

        public SceneManager(lpGame lpGame)
        {
            game = lpGame;
        }
        
        public void setScene(string sceneId)
        {
            if (current != null)
            {
                current.destroy();
            }
            Type sceneType = Type.GetType("lp.Scene_" + sceneId, true);
            current = (Scene)(Activator.CreateInstance(sceneType, game));
            if (current != null)
            {
                current.init();
            }
        }

        public void update(float deltaSeconds)
        {
            if (current != null)
            {
                current.update(deltaSeconds);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (current != null)
            {
                current.draw(spriteBatch);
            }
        }
    }
}
