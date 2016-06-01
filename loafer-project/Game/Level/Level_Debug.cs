using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_Debug : Level
    {
        public Level_Debug(lpGame lpGame) : base(lpGame)
        {
            mapId = "debug_level";
            backgroundId = "debug_bg";

            doors.Add(new Door(game));
            doors[0].position = new Vector2(43 * 16, 3 * 16);
            doors[0].targetRoom = "Test";
            doors[0].targetDoor = 1;
        }
    }
}
