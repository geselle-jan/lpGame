using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lp
{
    public class Level_Test3 : Level
    {
        public Level_Test3(lpGame lpGame) : base(lpGame)
        {
            mapId = "sector01_room02";
            backgroundSpriteSheet = new SpriteSheet(game);
            backgroundSpriteSheet.init("browntank_animation", new Vector2(32 * 16, 16 * 16));
            backgroundSpriteSheet.animations.Add(new Animation("bubbles", new List<int> { 0, 1, 2, 3 }, 4, game));
            backgroundSpriteSheet.play("bubbles");

            doors.Add(new Door(game));
            doors[0].position = new Vector2(-2 * 16, 3 * 16);
            doors[0].targetRoom = "Test1";
            doors[0].targetDoor = 0;
        }
    }
}
