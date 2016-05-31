using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace lp
{
    public class CameraManager
    {
        public lpGame game;

        public ViewportAdapter viewportAdapter;
        public Camera2D instance;

        public float zoom = 4;
        public bool zoomLock = true;
        public Entity follow;

        public CameraManager(lpGame lpGame)
        {
            game = lpGame;
        }

        public void init()
        {
            viewportAdapter = new DefaultViewportAdapter(game.graphicsDeviceManager.GraphicsDevice);
            instance = new Camera2D(viewportAdapter);
            instance.Zoom = zoom;
        }

        public void followEntity(Entity entity)
        {
            follow = entity;
        }

        public void unfollowEntity()
        {
            follow = null;
        }

        public void update(float deltaSeconds)
        {
            if (zoomLock)
            {
                setSize();
            }
            if (follow != null)
            {
                focus(follow.position + follow.size / 2);
            }
        }

        public void move(Vector2 vector)
        {
            instance.Move(vector);
        }

        public void zoomIn(float amount)
        {
            instance.ZoomIn(amount);
            zoom = instance.Zoom;
        }

        public void zoomOut(float amount)
        {
            instance.ZoomOut(amount);
            zoom = instance.Zoom;
        }

        public Matrix getViewMatrix()
        {
            return instance.GetViewMatrix();
        }

        public void focus (Vector2 point)
        {
            instance.Position = point - new Vector2(instance.GetBoundingRectangle().Width, instance.GetBoundingRectangle().Height) / 2 * zoom;
            var bounds = instance.GetBoundingRectangle();

            if (Math.Floor(bounds.Width) <= game.currentBounds.X && Math.Floor(bounds.Height) <= game.currentBounds.Y)
            {
                var adjustment = Vector2.Zero;
                if (bounds.Top < 0)
                {
                    adjustment.Y = bounds.Top * -1;
                }
                if (bounds.Bottom > game.currentBounds.Y)
                {
                    adjustment.Y = (bounds.Bottom - game.currentBounds.Y) * -1;
                }
                if (bounds.Right > game.currentBounds.X)
                {
                    adjustment.X = (bounds.Right - game.currentBounds.X) * -1;
                }
                if (bounds.Left < 0)
                {
                    adjustment.X = bounds.Left * -1;
                }
                instance.Move(adjustment);
            }
        }

        public void setSize()
        {
            var window = new Vector2(game.graphicsDeviceManager.PreferredBackBufferWidth, game.graphicsDeviceManager.PreferredBackBufferHeight);
            var visible = new Vector2(30, 20) * 16;
            var z = window / visible;
            zoom = z.X > z.Y ? z.X: z.Y;
            instance.Zoom = zoom;
        }

        public Vector2 getFixedPositionFor(Vector2 fixedPosition)
        {
            var bounds = instance.GetBoundingRectangle();
            var realPosition = fixedPosition + new Vector2(bounds.Left, bounds.Top);
            return realPosition;
        }

        public Vector2 getRelativePositionFor(Vector2 relativePosition)
        {
            var bounds = instance.GetBoundingRectangle();
            var realPosition = relativePosition - new Vector2(bounds.Left, bounds.Top);
            return realPosition;
        }

        public Vector2 getPosition()
        {
            return instance.Position;
        }
    }
}
