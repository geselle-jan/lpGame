using System;
using System.Collections.Generic;
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
        public SpriteSheet backgroundSpriteSheet;
        public bool showCollision = false;
        public List<Door> doors = new List<Door>();
        public Black black;
        public string name
        {
            get { return GetType().Name; }
        }

        public Level(lpGame lpGame)
        {
            game = lpGame;
        }

        public void init()
        {
            game.debug.entities = new List<Entity>();
            loadMap();
            foreach (var door in doors)
            {
                door.init();
            }
            black = new Black(new Vector2(map.WidthInPixels, map.HeightInPixels), game);
            black.init();
        }

        public void update(float deltaSeconds)
        {
            if (backgroundSpriteSheet != null && !game.paused)
            {
                backgroundSpriteSheet.update(deltaSeconds);
            }
            foreach (var door in doors)
            {
                door.update(deltaSeconds);
            }
            black.update(deltaSeconds);
        }

        public void loadMap()
        {
            map = game.content.Load<TiledMap>(mapId);
            if (backgroundSpriteSheet == null)
            {
                backgroundImage = game.content.Load<Texture2D>(backgroundId);
            }
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

            foreach (var door in doors)
            {
                door.draw(spriteBatch);
            }

            black.draw(spriteBatch);

            foreach (var door in doors)
            {
                if (door.transitionFrom || door.transitionTo)
                {
                    door.draw(spriteBatch);
                }
            }
        }

        private void drawBackgroundImage(SpriteBatch spriteBatch)
        {
            var imageWidth = 0;
            var imageHeight = 0;
            if (backgroundSpriteSheet != null)
            {
                imageWidth = (int)backgroundSpriteSheet.frameSize.X;
                imageHeight = (int)backgroundSpriteSheet.frameSize.Y;
            }
            else
            {
                imageWidth = backgroundImage.Width;
                imageHeight = backgroundImage.Height;
            } 
            var height = map.HeightInPixels;
            var width = map.WidthInPixels;
            var drawPosition = Vector2.Zero;
            var offset = game.camera.getPosition() / 2;
            var xDraws = width / imageWidth + 1;
            var yDraws = height / imageHeight + 1;
            var xCounter = 0;
            var yCounter = 0;
            var xShift = Math.Ceiling(offset.X / imageWidth) * imageWidth;
            var yShift = Math.Ceiling(offset.Y / imageHeight) * imageHeight;
            var shift = new Vector2((float)xShift, (float)yShift);

            while (xCounter <= xDraws)
            {
                yCounter = 0;
                while (yCounter <= yDraws)
                {
                    drawPosition.X = xCounter * imageWidth;
                    drawPosition.Y = yCounter * imageHeight;
                    if (backgroundSpriteSheet != null)
                    {
                        backgroundSpriteSheet.draw(spriteBatch, drawPosition + offset - shift);
                    }
                    else
                    {
                        spriteBatch.Draw(backgroundImage, drawPosition + offset - shift);
                    }

                    yCounter++;
                }
                xCounter++;
            }
        }
    }
}
