using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_crater : Level
    {
        public Level_crater(lpGame lpGame) : base(lpGame)
        {
            mapId = "maps/crater";
            backgroundId = "sr388cave";

            doors.Add(new Door(game));
            doors[0].position = new Vector2(16 * 16, 33 * 16);
            doors[0].targetRoom = "sector01_room02";
            doors[0].targetDoor = 1;
        }
    }
}