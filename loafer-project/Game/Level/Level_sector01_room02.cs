using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_sector01_room02 : Level
    {
        public Level_sector01_room02(lpGame lpGame) : base(lpGame)
        {
            mapId = "sector01_room02";
            backgroundSpriteSheet = new SpriteSheet(null, game);
            backgroundSpriteSheet.init("browntank_animation", new Vector2(32 * 16, 16 * 16));
            backgroundSpriteSheet.animations.Add(new Animation("bubbles", new List<int> { 0, 1, 2, 3 }, 4, true, Vector2.Zero, backgroundSpriteSheet, game));
            backgroundSpriteSheet.play("bubbles");

            doors.Add(new Door(game));
            doors[0].position = new Vector2(-2 * 16, 13 * 16);
            doors[0].targetRoom = "Test";
            doors[0].targetDoor = 0;

            doors.Add(new Door(game));
            doors[1].position = new Vector2(-2 * 16, 3 * 16);
            doors[1].targetRoom = "sector01_room01";
            doors[1].targetDoor = 0;

            doors.Add(new Door(game));
            doors[2].position = new Vector2(28 * 16, 13 * 16);
            doors[2].targetRoom = "sector01_room03";
            doors[2].targetDoor = 0;

            doors.Add(new Door(game));
            doors[3].position = new Vector2(28 * 16, 3 * 16);
            doors[3].targetRoom = "Debug_Combat";
            doors[3].targetDoor = 0;
        }
    }
}
