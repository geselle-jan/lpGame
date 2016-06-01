using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_sector01_room03 : Level
    {
        public Level_sector01_room03(lpGame lpGame) : base(lpGame)
        {
            mapId = "sector01_room03";
            backgroundId = "pipecorridor";
            backgroundOffset = new Vector2(0, -12 * 16);

            doors.Add(new Door(game));
            doors[0].position = new Vector2(-2 * 16, 8 * 16);
            doors[0].targetRoom = "sector01_room02";
            doors[0].targetDoor = 2;

            doors.Add(new Door(game));
            doors[1].position = new Vector2(43 * 16, 8 * 16);
            doors[1].targetRoom = "sector01_room04";
            doors[1].targetDoor = 0;
        }
    }
}
