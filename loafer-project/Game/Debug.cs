using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace lp
{
    public class Debug
    {
        public lpGame game;

        public bool show = false;
        public Texture2D outerRectangle;
        public Texture2D innerRectangle;
        public Color textColor = Color.LimeGreen;
        public Color backgroundColor = Color.Black;
        public FramesPerSecondCounterComponent fpsCounter;

        public Debug(lpGame lpGame)
        {
            game = lpGame;
        }

        public void init()
        {
            outerRectangle = game.graphics.getRectangleTexture(16 * 12, 16 + game.graphics.font.LineHeight * 5, textColor);
            innerRectangle = game.graphics.getRectangleTexture(16 * 12 - 2, 16 + game.graphics.font.LineHeight * 5 - 2, backgroundColor);
            game.Components.Add(fpsCounter = new FramesPerSecondCounterComponent(game));
        }

        public void update(float deltaSeconds)
        {
            if (game.input.toggleDebugJustPressed)
                show = !show;

            const float cameraSpeed = 400f;
            const float zoomSpeed = 0.8f;

            if (game.input.cameraUpPressed)
                game.camera.move(new Vector2(0, -cameraSpeed) * deltaSeconds);

            if (game.input.cameraLeftPressed)
                game.camera.move(new Vector2(-cameraSpeed, 0) * deltaSeconds);

            if (game.input.cameraDownPressed)
                game.camera.move(new Vector2(0, cameraSpeed) * deltaSeconds);

            if (game.input.cameraRightPressed)
                game.camera.move(new Vector2(cameraSpeed, 0) * deltaSeconds);

            if (game.input.zoomInPressed)
            {
                game.camera.zoomIn(zoomSpeed * deltaSeconds);
            }

            if (game.input.zoomOutPressed)
            {
                game.camera.zoomOut(zoomSpeed * deltaSeconds);
            }

            if (game.input.toggleTrackingJustPressed)
            {
                if (game.camera.follow == null)
                {
                    game.camera.followEntity(game.player);
                }
                else
                {
                    game.camera.unfollowEntity();
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (show && game.graphicsDeviceManager.GraphicsDevice != null)
            {
                spriteBatch.Begin(transformMatrix: game.camera.getViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
                spriteBatch.Draw(outerRectangle, game.camera.getFixedPositionFor(Vector2.Zero));
                spriteBatch.Draw(innerRectangle, game.camera.getFixedPositionFor(Vector2.One));
                spriteBatch.DrawString(game.graphics.font, $"FPS: {fpsCounter.AverageFramesPerSecond:0}", game.camera.getFixedPositionFor(new Vector2(8, 8)), textColor);
                if (game.scene.current is Scene_Level)
                {
                    spriteBatch.DrawString(game.graphics.font, $"Position X: {game.player.position.X}", game.camera.getFixedPositionFor(new Vector2(8, 8 + game.graphics.font.LineHeight * 1)), textColor);
                    spriteBatch.DrawString(game.graphics.font, $"Position Y: {game.player.position.Y}", game.camera.getFixedPositionFor(new Vector2(8, 8 + game.graphics.font.LineHeight * 2)), textColor);
                    spriteBatch.DrawString(game.graphics.font, $"onGround: {game.player.onGround}", game.camera.getFixedPositionFor(new Vector2(8, 8 + game.graphics.font.LineHeight * 3)), textColor);
                    spriteBatch.DrawString(game.graphics.font, $"isOnSlope: {game.player.isOnSlope}", game.camera.getFixedPositionFor(new Vector2(8, 8 + game.graphics.font.LineHeight * 4)), textColor);
                }
                spriteBatch.End();
            }
        }

        public void log(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
    }
}
