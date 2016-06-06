using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_Debug_Combat : Level
    {
        public Level_Debug_Combat(lpGame lpGame) : base(lpGame)
        {
            mapId = "debug_combat";
            backgroundId = "debug_bg";

            doors.Add(new Door(game));
            doors[0].position = new Vector2(-2 * 16, 23 * 16);
            doors[0].targetRoom = "sector01_room02";
            doors[0].targetDoor = 3;

            enemies.Add(new Enemy(game));
            enemies[0].position = new Vector2(24 * 16, 18 * 16);

            enemies.Add(new Enemy(game));
            enemies[1].position = new Vector2(5 * 16, 13 * 16);

            enemies.Add(new Enemy(game));
            enemies[2].position = new Vector2(8 * 16, 7 * 16);

            enemies.Add(new Enemy(game));
            enemies[3].position = new Vector2(27 * 16, 4 * 16);
        }
    }
}
