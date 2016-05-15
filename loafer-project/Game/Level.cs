using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Sprites;

namespace lp
{
    public class Level
    {
        public lpGame game;

        public TiledMap map;
        public string mapId;
        public string backgroundId;
        public Texture2D backgroundImage;
        public bool showCollision = false;

        public Level(lpGame lpGame)
        {
            game = lpGame;
        }

        public void init()
        {
            loadMap();
        }

        public void loadMap()
        {
            map = game.content.Load<TiledMap>(mapId);
            backgroundImage = game.content.Load<Texture2D>(backgroundId);
            game.currentBounds = new Vector2(map.WidthInPixels, map.HeightInPixels);
        }

        public void drawBackground(SpriteBatch spriteBatch)
        {
            drawBackgroundImage(spriteBatch);

            foreach (var layer in map.Layers)
            {
                if (layer.Name == "collision" && showCollision)
                {
                    spriteBatch.Draw(layer, game.camera.instance);
                }
            }

            foreach (var layer in map.Layers)
            {
                if (layer.Name == "background2")
                {
                    spriteBatch.Draw(layer, game.camera.instance);
                }
            }

            foreach (var layer in map.Layers)
            {
                if (layer.Name == "background1")
                {
                    spriteBatch.Draw(layer, game.camera.instance);
                }
            }
        }

        public void drawForeground(SpriteBatch spriteBatch)
        {
            foreach (var layer in map.Layers)
            {
                if (layer.Name == "foreground2")
                {
                    spriteBatch.Draw(layer, game.camera.instance);
                }
            }

            foreach (var layer in map.Layers)
            {
                if (layer.Name == "foreground1")
                {
                    spriteBatch.Draw(layer, game.camera.instance);
                }
            }
        }

        private void drawBackgroundImage(SpriteBatch spriteBatch)
        {
            var height = map.HeightInPixels;
            var width = map.WidthInPixels;
            var drawPosition = Vector2.Zero;
            var offset = game.camera.getPosition() / 2;
            var xDraws = width / backgroundImage.Width + 1;
            var yDraws = height / backgroundImage.Height + 1;
            var xCounter = 0;
            var yCounter = 0;
            var xShift = Math.Ceiling(offset.X / backgroundImage.Width) * backgroundImage.Width;
            var yShift = Math.Ceiling(offset.Y / backgroundImage.Height) * backgroundImage.Height;
            var shift = new Vector2((float)xShift, (float)yShift);

            while (xCounter <= xDraws)
            {
                yCounter = 0;
                while (yCounter <= yDraws)
                {
                    drawPosition.X = xCounter * backgroundImage.Width;
                    drawPosition.Y = yCounter * backgroundImage.Height;
                    spriteBatch.Draw(backgroundImage, drawPosition + offset - shift);
                    yCounter++;
                }
                xCounter++;
            }
        }
    }
}
