using System;
using System.Collections.Generic;
using MonoGame.Extended.Maps.Tiled;

namespace lp
{
    public class Physics
    {
        public lpGame game;

        public Physics(lpGame lpGame)
        {
            game = lpGame;
        }

        public bool intersect(Entity a, Entity b)
        {
            return !(
                a.position.X + a.size.X < b.position.X
                ||
                a.position.Y + a.size.Y < b.position.Y
                ||
                a.position.X > b.position.X + b.size.X
                ||
                a.position.Y > b.position.Y + b.size.Y
            );
        }

        public void collideY(Entity entity, TiledMap map)
        {
            if (entity.physical)
            {
                entity.isOnSlope = false;
                var collisionY = checkCollisionY(entity, map);
                if (entity.wasOnSlope && !entity.isOnSlope)
                {
                    entity.position.Y += 1;
                    collisionY = checkCollisionY(entity, map);
                }
                entity.onGround = false;
                if (collisionY != 0)
                {
                    if (collisionY > 0)
                    {
                        entity.onGround = true;
                    }

                    entity.velocity.Y = 0;
                    entity.position.Y -= collisionY;
                }
                entity.wasOnSlope = entity.isOnSlope;
            }
            else
            {
                var collisionY = checkCollisionY(entity, map);
                if (collisionY != 0)
                {
                    game.debug.log($"{collisionY}");
                    entity.collisionDirty = true;
                }
            }
        }

        public void collideX(Entity entity, TiledMap map)
        {
            if (entity.physical)
            {
                entity.blockedLeft = false;
                entity.blockedRight = false;
                var collisionX = checkCollisionX(entity, map);
                if (collisionX != 0)
                {
                    if (collisionX > 0)
                    {
                        entity.blockedRight = true;
                    }
                    else
                    {
                        entity.blockedLeft = true;
                    }
                    entity.position.X -= collisionX;
                }
            }
            else
            {
                var collisionX = checkCollisionX(entity, map);
                if (collisionX != 0)
                {
                    entity.collisionDirty = true;
                }
            }
        }

        public float checkCollisionY(Entity entity, TiledMap map)
        {
            var collisionY = 0f;
            var collisionMapBottom = (entity.position.Y + entity.size.Y) - (map.HeightInPixels);
            var collisionMapTop = entity.position.Y;
            if (collisionMapBottom > 0 && collisionMapBottom > collisionY)
            {
                collisionY = collisionMapBottom;
            }
            if (collisionMapTop < 0 && collisionMapTop < collisionY)
            {
                collisionY = collisionMapTop;
            }
            foreach (var tileLayer in map.TileLayers)
            {
                if (tileLayer.Name == "collision")
                {
                    var tileSize = map.TileHeight;
                    var currentX = 0;
                    var currentY = 0;
                    var highHalfSlopeCollided = false;

                    while (currentX <= entity.size.X)
                    {
                        while (currentY <= entity.size.Y)
                        {
                            var entityX = (int)Math.Floor((entity.position.X + currentX) / tileSize);
                            var entityY = (int)Math.Floor((entity.position.Y + currentY) / tileSize);

                            if (currentX == entity.size.X && entity.position.X % tileSize == 0)
                            {
                                break;
                            }

                            if (currentY == entity.size.Y && entity.position.Y % tileSize == 0)
                            {
                                break;
                            }

                            var tileID = tileLayer.GetTile(
                                entityX,
                                entityY
                            ).Id;

                            if (tileID == 1)
                            {
                                if (entity.velocity.Y > 0) // falling down
                                {
                                    var intersection = (entity.position.Y + entity.size.Y) - (entityY * tileSize);
                                    if (intersection > collisionY)
                                    {
                                        collisionY = intersection;
                                    }
                                }
                                else // jumping
                                {
                                    var intersection = (entity.position.Y) - ((entityY * tileSize) + tileSize);
                                    if (intersection < collisionY)
                                    {
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 2)
                            {
                                if (entity.velocity.Y > 0 || !entity.gravityAffected) // falling down
                                {
                                    var highestPointUnderEntity = tileSize - (entity.position.X + entity.size.X - entityX * tileSize);
                                    if (highestPointUnderEntity < 0)
                                    {
                                        highestPointUnderEntity = 0;
                                    }
                                    if (highestPointUnderEntity > tileSize)
                                    {
                                        highestPointUnderEntity = tileSize;
                                    }
                                    var intersection = (entity.position.Y + entity.size.Y) - (entityY * tileSize) - highestPointUnderEntity;

                                    if (entity.onGround && highestPointUnderEntity != 0)
                                    {
                                        entity.position.Y = entity.position.Y - intersection + 1;
                                        entity.isOnSlope = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        entity.isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 3)
                            {
                                if (entity.velocity.Y > 0 || !entity.gravityAffected) // falling down
                                {
                                    var highestPointUnderEntity = tileSize - (entityX * tileSize + tileSize - entity.position.X);
                                    if (highestPointUnderEntity < 0)
                                    {
                                        highestPointUnderEntity = 0;
                                    }
                                    if (highestPointUnderEntity > tileSize)
                                    {
                                        highestPointUnderEntity = tileSize;
                                    }
                                    var intersection = (entity.position.Y + entity.size.Y) - (entityY * tileSize) - highestPointUnderEntity;

                                    if (entity.onGround && highestPointUnderEntity != 0)
                                    {
                                        entity.position.Y = entity.position.Y - intersection + 1;
                                        entity.isOnSlope = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        entity.isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 4)
                            {
                                if ((entity.velocity.Y > 0 || !entity.gravityAffected) && !highHalfSlopeCollided) // falling down
                                {
                                    var highestPointUnderEntity = tileSize - (entity.position.X + entity.size.X - entityX * tileSize);
                                    if (highestPointUnderEntity < 0)
                                    {
                                        highestPointUnderEntity = 0;
                                    }
                                    if (highestPointUnderEntity > tileSize)
                                    {
                                        highestPointUnderEntity = tileSize;
                                    }
                                    highestPointUnderEntity = highestPointUnderEntity / 2 + tileSize / 2;
                                    var intersection = (entity.position.Y + entity.size.Y) - (entityY * tileSize) - highestPointUnderEntity;

                                    if (entity.onGround && highestPointUnderEntity != 0)
                                    {
                                        entity.position.Y = entity.position.Y - intersection + 1;
                                        entity.isOnSlope = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        entity.isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 5)
                            {
                                if (entity.velocity.Y > 0 || !entity.gravityAffected) // falling down
                                {
                                    var highestPointUnderEntity = tileSize - (entity.position.X + entity.size.X - entityX * tileSize);
                                    if (highestPointUnderEntity < 0)
                                    {
                                        highestPointUnderEntity = 0;
                                    }
                                    if (highestPointUnderEntity > tileSize)
                                    {
                                        highestPointUnderEntity = tileSize;
                                    }
                                    highestPointUnderEntity = highestPointUnderEntity / 2;
                                    var intersection = (entity.position.Y + entity.size.Y) - (entityY * tileSize) - highestPointUnderEntity;

                                    if (entity.onGround && highestPointUnderEntity != 0)
                                    {
                                        entity.position.Y = entity.position.Y - intersection + 1;
                                        entity.isOnSlope = true;
                                        highHalfSlopeCollided = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        entity.isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 6)
                            {
                                if (entity.velocity.Y > 0 || !entity.gravityAffected) // falling down
                                {
                                    var highestPointUnderEntity = tileSize - (entityX * tileSize + tileSize - entity.position.X);
                                    if (highestPointUnderEntity < 0)
                                    {
                                        highestPointUnderEntity = 0;
                                    }
                                    if (highestPointUnderEntity > tileSize)
                                    {
                                        highestPointUnderEntity = tileSize;
                                    }
                                    highestPointUnderEntity = highestPointUnderEntity / 2;
                                    var intersection = (entity.position.Y + entity.size.Y) - (entityY * tileSize) - highestPointUnderEntity;

                                    if (entity.onGround && highestPointUnderEntity != 0)
                                    {
                                        entity.position.Y = entity.position.Y - intersection + 1;
                                        entity.isOnSlope = true;
                                        highHalfSlopeCollided = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        entity.isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }
                            else if (tileID == 7)
                            {
                                if ((entity.velocity.Y > 0 || !entity.gravityAffected) && !highHalfSlopeCollided) // falling down
                                {
                                    var highestPointUnderEntity = tileSize - (entityX * tileSize + tileSize - entity.position.X);
                                    if (highestPointUnderEntity < 0)
                                    {
                                        highestPointUnderEntity = 0;
                                    }
                                    if (highestPointUnderEntity > tileSize)
                                    {
                                        highestPointUnderEntity = tileSize;
                                    }
                                    highestPointUnderEntity = highestPointUnderEntity / 2 + tileSize / 2;
                                    var intersection = (entity.position.Y + entity.size.Y) - (entityY * tileSize) - highestPointUnderEntity;

                                    if (entity.onGround && highestPointUnderEntity != 0)
                                    {
                                        entity.position.Y = entity.position.Y - intersection + 1;
                                        entity.isOnSlope = true;
                                        collisionY = 1f;
                                    }
                                    else if (intersection > collisionY)
                                    {
                                        entity.isOnSlope = true;
                                        collisionY = intersection;
                                    }
                                }
                            }

                            if (entity.size.Y < tileSize)
                            {
                                currentY = currentY + (int)entity.size.Y;
                            }
                            else
                            {
                                currentY = currentY + tileSize;
                            }
                        }

                        currentY = 0;
                        if (entity.size.X < tileSize)
                        {
                            currentX = currentX + (int)entity.size.X;
                        }
                        else
                        {
                            currentX = currentX + tileSize;
                        }
                    }
                }
            }

            return collisionY;
        }

        public float checkCollisionX(Entity entity, TiledMap map)
        {
            var collisionX = 0f;
            var collisionMapRight = (entity.position.X + entity.size.X) - (map.WidthInPixels);
            var collisionMapLeft = entity.position.X;
            if (collisionMapRight > 0 && collisionMapRight > collisionX)
            {
                collisionX = collisionMapRight;
            }
            if (collisionMapLeft < 0 && collisionMapLeft < collisionX)
            {
                collisionX = collisionMapLeft;
            }
            foreach (var tileLayer in map.TileLayers)
            {
                if (tileLayer.Name == "collision")
                {
                    var tileSize = map.TileHeight;
                    var currentX = 0;
                    var currentY = 0;


                    while (currentX <= entity.size.X)
                    {
                        while (currentY <= entity.size.Y)
                        {
                            var entityX = (int)Math.Floor((entity.position.X + currentX) / tileSize);
                            var entityY = (int)Math.Floor((entity.position.Y + currentY) / tileSize);

                            if (currentX == entity.size.X && entity.position.X % tileSize == 0)
                            {
                                break;
                            }

                            if (currentY == entity.size.Y && entity.position.Y % tileSize == 0)
                            {
                                break;
                            }

                            var tileID = tileLayer.GetTile(
                                entityX,
                                entityY
                            ).Id;
                            if (tileID == 1)
                            {
                                if (isSlopeAt(entityX, entityY - 1, tileLayer)) {
                                    break;
                                }

                                if (entity.velocity.X > 0)
                                {
                                    if (isSlopeAt(entityX - 1, entityY, tileLayer))
                                    {
                                        break;
                                    }
                                }

                                if (entity.velocity.X < 0)
                                {
                                    if (isSlopeAt(entityX + 1, entityY, tileLayer))
                                    {
                                        break;
                                    }
                                }

                                if (entity.velocity.X > 0) // going right
                                {
                                    var intersection = (entity.position.X + entity.size.X) - (entityX * tileSize);
                                    if (intersection > collisionX)
                                    {
                                        collisionX = intersection;
                                    }
                                }
                                else // going left
                                {
                                    var intersection = (entity.position.X) - ((entityX * tileSize) + tileSize);
                                    if (intersection < collisionX)
                                    {
                                        collisionX = intersection;
                                    }
                                }
                            }

                            if (entity.size.Y < tileSize)
                            {
                                currentY = currentY + (int)entity.size.Y;
                            }
                            else
                            {
                                currentY = currentY + tileSize;
                            }
                        }

                        currentY = 0;
                        if (entity.size.X < tileSize)
                        {
                            currentX = currentX + (int)entity.size.X;
                        }
                        else
                        {
                            currentX = currentX + tileSize;
                        }
                    }
                }
            }

            return collisionX;
        }

        public bool isSlope(TiledTile tile)
        {
            var slopeIds = new List<int> { 2, 3, 4, 5, 6, 7 };
            return slopeIds.Contains(tile.Id);
        }

        public bool isSlopeAt(int x, int y, TiledTileLayer tileLayer)
        {
            var tile = tileLayer.GetTile(x, y);
            return isSlope(tile);
        }

    }
}
