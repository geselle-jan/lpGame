using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_Test3 : Level
    {
        public Level_Test3(lpGame lpGame) : base(lpGame)
        {
            mapId = "sector01_room02";
            backgroundId = "browntank_animation";

            doors.Add(new Door(game));
            doors[0].position = new Vector2(-2 * 16, 3 * 16);
            doors[0].targetRoom = "Test1";
            doors[0].targetDoor = 0;
        }
    }
}
