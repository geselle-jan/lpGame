using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_sector01_room01 : Level
    {
        public Level_sector01_room01(lpGame lpGame) : base(lpGame)
        {
            mapId = "sector01_room01";
            backgroundId = "brown";

            doors.Add(new Door(game));
            doors[0].position = new Vector2(28 * 16, 3 * 16);
            doors[0].targetRoom = "sector01_room02";
            doors[0].targetDoor = 1;
        }
    }
}
