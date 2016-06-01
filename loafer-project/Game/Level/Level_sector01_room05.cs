using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_sector01_room05 : Level
    {
        public Level_sector01_room05(lpGame lpGame) : base(lpGame)
        {
            mapId = "sector01_room05";
            backgroundId = "sr388cave";

            doors.Add(new Door(game));
            doors[0].position = new Vector2(-2 * 16, 8 * 16);
            doors[0].targetRoom = "sector01_room04";
            doors[0].targetDoor = 1;

            doors.Add(new Door(game));
            doors[1].position = new Vector2(28 * 16, 8 * 16);
            doors[1].targetRoom = "sector01_room06";
            doors[1].targetDoor = 0;
        }
    }
}
