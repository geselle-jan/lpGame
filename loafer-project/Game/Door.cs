using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lp
{
    public class Door : Entity
    {
        public string targetRoom;
        public int targetDoor;

        public Door(lpGame lpGame) : base(lpGame)
        {
            textureId = "door";
        }

        public new void update(float deltaSeconds)
        {
            if (game.physics.intersect(game.player, this))
            {
                game.levelManager.setLevel(targetRoom);
                game.player.position = game.currentLevel.doors[targetDoor].getPlayerPosition();
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
