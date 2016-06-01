using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_sector01_room06 : Level
    {
        public Level_sector01_room06(lpGame lpGame) : base(lpGame)
        {
            mapId = "sector01_room06";
            backgroundId = "greenpassage";
            backgroundOffset = new Vector2(0, 1.5f * 16);

            doors.Add(new Door(game));
            doors[0].position = new Vector2(-2 * 16, 3 * 16);
            doors[0].targetRoom = "sector01_room05";
            doors[0].targetDoor = 1;

            doors.Add(new Door(game));
            doors[1].position = new Vector2(28 * 16, 3 * 16);
            doors[1].targetRoom = "sector01_room07";
            doors[1].targetDoor = 0;

            doors.Add(new Door(game));
            doors[2].position = new Vector2(-2 * 16, 13 * 16);
            doors[2].targetRoom = "sector01_room08";
            doors[2].targetDoor = 0;
        }
    }
}
