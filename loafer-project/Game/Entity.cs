using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class Entity
    {
        public lpGame game;

        public Vector2 position = Vector2.Zero;
        public Vector2 size = Vector2.Zero;
        public Vector2 gravity = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public bool onGround = false;
        public bool wasOnSlope = false;
        public bool isOnSlope = false;
        public Texture2D texture;
        public string textureId;

        public Entity(lpGame lpGame)
        {
            game = lpGame;
        }

        public void init()
        {
            texture = game.content.Load<Texture2D>(textureId);
            size = new Vector2(texture.Width, texture.Height);
        }

        public void update(float deltaSeconds)
        {
            velocity += gravity * deltaSeconds * 1;
            position.Y = position.Y + (velocity.Y * deltaSeconds);
            game.physics.collideY(this, game.currentLevel.map);
            position.X = position.X + (velocity.X * deltaSeconds);
            game.physics.collideX(this, game.currentLevel.map);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position);
        }
    }
}
