using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class Door : Entity
    {
        public string targetRoom;
        public int targetDoor;
        public bool transitionFrom = false;
        public bool transitionTo = false;
        public Vector2 transitionDistance;
        public Vector2 transitionPosition;

        public Door(lpGame lpGame) : base(lpGame)
        {
            textureId = "door";
        }

        public new void update(float deltaSeconds)
        {
            if (!transitionFrom && !transitionTo && game.physics.intersect(game.player, this))
            {
                transitionFrom = true;
                game.paused = true;
                game.currentLevel.black.fadeIn();
            }
            if (transitionFrom && !game.currentLevel.black.fading)
            {
                transitionFrom = false;
                transitionPosition = game.camera.getRelativePositionFor(position);
                game.levelManager.setLevel(targetRoom);
                game.player.position = game.currentLevel.doors[targetDoor].getPlayerPosition();
                game.camera.update(0f);
                game.currentLevel.doors[targetDoor].transitionTo = true;
                game.currentLevel.doors[targetDoor].transitionPosition = game.currentLevel.doors[targetDoor].position;
                game.currentLevel.doors[targetDoor].position = game.camera.getFixedPositionFor(transitionPosition);
                game.currentLevel.doors[targetDoor].transitionDistance = new Vector2(
                    game.currentLevel.doors[targetDoor].transitionPosition.X - game.currentLevel.doors[targetDoor].position.X,
                    game.currentLevel.doors[targetDoor].transitionPosition.Y - game.currentLevel.doors[targetDoor].position.Y
                );
            }
            if (transitionTo)
            {
                var speedX = 1;
                var speedY = 4;
                if (transitionDistance.Y != 0)
                {
                    position.Y += deltaSeconds * transitionDistance.Y * speedY;
                }
                if (transitionDistance.Y < 0 && position.Y < transitionPosition.Y)
                {
                    position.Y = transitionPosition.Y;
                }
                if (transitionDistance.Y > 0 && position.Y > transitionPosition.Y)
                {
                    position.Y = transitionPosition.Y;
                }
                if (position.Y == transitionPosition.Y)
                {
                    position.X += deltaSeconds * transitionDistance.X * speedX;
                    if (transitionDistance.X < 0 && position.X < transitionPosition.X)
                    {
                        position.X = transitionPosition.X;
                    }
                    if (transitionDistance.X > 0 && position.X > transitionPosition.X)
                    {
                        position.X = transitionPosition.X;
                    }
                    if (position.X == transitionPosition.X)
                    {
                        game.currentLevel.black.fadeOut();
                    }
                }
            }
        }

        public Vector2 getPlayerPosition()
        {
            var y = position.Y + size.Y - game.player.size.Y;
            var x = 0f;
            if (position.X < 0)
            {
                x = position.X + size.X + 0.1f;
            }
            else
            {
                x = position.X - game.player.size.X - 0.1f;
            }
            return new Vector2(x, y);
        }
    }
}
