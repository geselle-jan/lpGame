using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_Test : Level
    {
        public Level_Test(lpGame lpGame) : base(lpGame)
        {
            mapId = "test";
            backgroundId = "sr388cave";

            doors.Add(new Door(game));
            doors[0].position = new Vector2(58 * 16, 7 * 16);
            doors[0].targetRoom = "sector01_room02";
            doors[0].targetDoor = 0;

            doors.Add(new Door(game));
            doors[1].position = new Vector2(-2 * 16, 29 * 16);
            doors[1].targetRoom = "Debug";
            doors[1].targetDoor = 0;
        }
    }
}
