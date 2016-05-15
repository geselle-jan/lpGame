using Microsoft.Xna.Framework.Input;

namespace lp
{
    public class InputManager
    {
        public lpGame game;

        //======================================
        //
        //   key definitions
        //
        //======================================

        public Keys left;
        public Keys right;
        public Keys jump;
        //public Keys  fire;

        public Keys cameraUp;
        public Keys cameraDown;
        public Keys cameraLeft;
        public Keys cameraRight;

        public Keys zoomIn;
        public Keys zoomOut;

        public Keys toggleTracking;
        public Keys toggleFullscreen;
        public Keys toggleDebug;

        public Keys resetPlayer;

        public Keys menu;

        //======================================
        //
        //   abstracted key states
        //
        //======================================

        public bool leftPressed = false;
        public bool leftPressedBefore = false;
        public bool leftJustPressed = false;

        public bool rightPressed = false;
        public bool rightPressedBefore = false;
        public bool rightJustPressed = false;

        public bool jumpPressed = false;
        public bool jumpPressedBefore = false;
        public bool jumpJustPressed = false;

        //public bool firePressed = false;
        //public bool firePressedBefore = false;
        //public bool fireJustPressed = false;

        public bool cameraUpPressed = false;
        public bool cameraUpPressedBefore = false;
        public bool cameraUpJustPressed = false;

        public bool cameraDownPressed = false;
        public bool cameraDownPressedBefore = false;
        public bool cameraDownJustPressed = false;

        public bool cameraLeftPressed = false;
        public bool cameraLeftPressedBefore = false;
        public bool cameraLeftJustPressed = false;

        public bool cameraRightPressed = false;
        public bool cameraRightPressedBefore = false;
        public bool cameraRightJustPressed = false;

        public bool zoomInPressed = false;
        public bool zoomInPressedBefore = false;
        public bool zoomInJustPressed = false;

        public bool zoomOutPressed = false;
        public bool zoomOutPressedBefore = false;
        public bool zoomOutJustPressed = false;

        public bool toggleTrackingPressed = false;
        public bool toggleTrackingPressedBefore = false;
        public bool toggleTrackingJustPressed = false;

        public bool toggleFullscreenPressed = false;
        public bool toggleFullscreenPressedBefore = false;
        public bool toggleFullscreenJustPressed = false;

        public bool toggleDebugPressed = false;
        public bool toggleDebugPressedBefore = false;
        public bool toggleDebugJustPressed = false;

        public bool resetPlayerPressed = false;
        public bool resetPlayerPressedBefore = false;
        public bool resetPlayerJustPressed = false;

        public bool menuPressed = false;
        public bool menuPressedBefore = false;
        public bool menuJustPressed = false;


        public InputManager (lpGame lpGame)
        {
            game = lpGame;

            left = Keys.Left;
            right = Keys.Right;
            jump = Keys.Up;
            // fire;

            cameraUp = Keys.W;
            cameraDown = Keys.S;
            cameraLeft = Keys.A;
            cameraRight = Keys.D;

            zoomIn = Keys.R;
            zoomOut = Keys.F;

            toggleTracking = Keys.T;
            toggleFullscreen = Keys.F11;
            toggleDebug = Keys.I;

            resetPlayer = Keys.Z;

            menu = Keys.Escape;
        }

        public void update()
        {
            var keyboardState = Keyboard.GetState();


            //======================================
            //
            //   set previous button states
            //
            //======================================

            leftPressedBefore = leftPressed;
            rightPressedBefore = rightPressed;
            jumpPressedBefore = jumpPressed;
            //firePressedBefore = firePressed;

            cameraUpPressedBefore = cameraUpPressed;
            cameraDownPressedBefore = cameraDownPressed;
            cameraLeftPressedBefore = cameraLeftPressed;
            cameraRightPressedBefore = cameraRightPressed;

            zoomInPressedBefore = zoomInPressed;
            zoomOutPressedBefore = zoomOutPressed;

            toggleTrackingPressedBefore = toggleTrackingPressed;
            toggleFullscreenPressedBefore = toggleFullscreenPressed;
            toggleDebugPressedBefore = toggleDebugPressed;

            resetPlayerPressedBefore = resetPlayerPressed;

            menuPressedBefore = menuPressed;


            //======================================
            //
            //   set current button states
            //
            //======================================

            leftPressed = keyboardState.IsKeyDown(left);
            rightPressed = keyboardState.IsKeyDown(right);
            jumpPressed = keyboardState.IsKeyDown(jump);
            //firePressed = keyboardState.IsKeyDown(fire);

            cameraUpPressed = keyboardState.IsKeyDown(cameraUp);
            cameraDownPressed = keyboardState.IsKeyDown(cameraDown);
            cameraLeftPressed = keyboardState.IsKeyDown(cameraLeft);
            cameraRightPressed = keyboardState.IsKeyDown(cameraRight);

            zoomInPressed = keyboardState.IsKeyDown(zoomIn);
            zoomOutPressed = keyboardState.IsKeyDown(zoomOut);

            toggleTrackingPressed = keyboardState.IsKeyDown(toggleTracking);
            toggleFullscreenPressed = keyboardState.IsKeyDown(toggleFullscreen);
            toggleDebugPressed = keyboardState.IsKeyDown(toggleDebug);

            resetPlayerPressed = keyboardState.IsKeyDown(resetPlayer);

            menuPressed = keyboardState.IsKeyDown(menu);


            //======================================
            //
            //   determine just pressed buttons
            //
            //======================================

            leftJustPressed = leftPressed && !leftPressedBefore ? true : false;
            rightJustPressed = rightPressed && !rightPressedBefore ? true : false;
            jumpJustPressed = jumpPressed && !jumpPressedBefore ? true : false;
            //fireJustPressed = firePressed && !firePressedBefore ? true : false;

            cameraUpJustPressed = cameraUpPressed && !cameraUpPressedBefore ? true : false;
            cameraDownJustPressed = cameraDownPressed && !cameraDownPressedBefore ? true : false;
            cameraLeftJustPressed = cameraLeftPressed && !cameraLeftPressedBefore ? true : false;
            cameraRightJustPressed = cameraRightPressed && !cameraRightPressedBefore ? true : false;

            zoomInJustPressed = zoomInPressed && !zoomInPressedBefore ? true : false;
            zoomOutJustPressed = zoomOutPressed && !zoomOutPressedBefore ? true : false;

            toggleTrackingJustPressed = toggleTrackingPressed && !toggleTrackingPressedBefore ? true : false;
            toggleFullscreenJustPressed = toggleFullscreenPressed && !toggleFullscreenPressedBefore ? true : false;
            toggleDebugJustPressed = toggleDebugPressed && !toggleDebugPressedBefore ? true : false;

            resetPlayerJustPressed = resetPlayerPressed && !resetPlayerPressedBefore ? true : false;

            menuJustPressed = menuPressed && !menuPressedBefore ? true : false;
        }
    }
}
