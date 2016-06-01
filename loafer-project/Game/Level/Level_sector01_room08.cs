using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_sector01_room08 : Level
    {
        public Level_sector01_room08(lpGame lpGame) : base(lpGame)
        {
            mapId = "sector01_room08";
            backgroundId = "sr388cave";

            doors.Add(new Door(game));
            doors[0].position = new Vector2(73 * 16, 8 * 16);
            doors[0].targetRoom = "sector01_room06";
            doors[0].targetDoor = 2;
        }
    }
}
