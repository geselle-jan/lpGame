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
        public Keys up;
        public Keys down;
        public Keys a;
        public Keys b;

        public Keys cameraUp;
        public Keys cameraDown;
        public Keys cameraLeft;
        public Keys cameraRight;

        public Keys zoomIn;
        public Keys zoomOut;

        public Keys toggleTracking;
        public Keys toggleFullscreen;
        public Keys toggleDebug;
        public Keys togglePause;

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

        public bool upPressed = false;
        public bool upPressedBefore = false;
        public bool upJustPressed = false;

        public bool downPressed = false;
        public bool downPressedBefore = false;
        public bool downJustPressed = false;

        public bool aPressed = false;
        public bool aPressedBefore = false;
        public bool aJustPressed = false;

        public bool bPressed = false;
        public bool bPressedBefore = false;
        public bool bJustPressed = false;

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

        public bool togglePausePressed = false;
        public bool togglePausePressedBefore = false;
        public bool togglePauseJustPressed = false;

        public bool resetPlayerPressed = false;
        public bool resetPlayerPressedBefore = false;
        public bool resetPlayerJustPressed = false;

        public bool menuPressed = false;
        public bool menuPressedBefore = false;
        public bool menuJustPressed = false;


        public InputManager (lpGame lpGame)
        {
            game = lpGame;

            left = Keys.A;
            right = Keys.D;
            up = Keys.W;
            down = Keys.S;
            a = Keys.Up;
            b = Keys.Left;

            cameraUp = Keys.NumPad8;
            cameraDown = Keys.NumPad2;
            cameraLeft = Keys.NumPad4;
            cameraRight = Keys.NumPad6;

            zoomIn = Keys.NumPad9;
            zoomOut = Keys.NumPad3;

            toggleTracking = Keys.NumPad5;
            toggleFullscreen = Keys.F11;
            toggleDebug = Keys.NumPad0;
            togglePause = Keys.NumPad7;

            resetPlayer = Keys.NumPad1;

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
            upPressedBefore = leftPressed;
            downPressedBefore = rightPressed;
            aPressedBefore = leftPressed;
            bPressedBefore = rightPressed;

            cameraUpPressedBefore = cameraUpPressed;
            cameraDownPressedBefore = cameraDownPressed;
            cameraLeftPressedBefore = cameraLeftPressed;
            cameraRightPressedBefore = cameraRightPressed;

            zoomInPressedBefore = zoomInPressed;
            zoomOutPressedBefore = zoomOutPressed;

            toggleTrackingPressedBefore = toggleTrackingPressed;
            toggleFullscreenPressedBefore = toggleFullscreenPressed;
            toggleDebugPressedBefore = toggleDebugPressed;
            togglePausePressedBefore = togglePausePressed;

            resetPlayerPressedBefore = resetPlayerPressed;

            menuPressedBefore = menuPressed;


            //======================================
            //
            //   set current button states
            //
            //======================================

            leftPressed = keyboardState.IsKeyDown(left);
            rightPressed = keyboardState.IsKeyDown(right);
            upPressed = keyboardState.IsKeyDown(up);
            downPressed = keyboardState.IsKeyDown(down);
            aPressed = keyboardState.IsKeyDown(a);
            bPressed = keyboardState.IsKeyDown(b);

            cameraUpPressed = keyboardState.IsKeyDown(cameraUp);
            cameraDownPressed = keyboardState.IsKeyDown(cameraDown);
            cameraLeftPressed = keyboardState.IsKeyDown(cameraLeft);
            cameraRightPressed = keyboardState.IsKeyDown(cameraRight);

            zoomInPressed = keyboardState.IsKeyDown(zoomIn);
            zoomOutPressed = keyboardState.IsKeyDown(zoomOut);

            toggleTrackingPressed = keyboardState.IsKeyDown(toggleTracking);
            toggleFullscreenPressed = keyboardState.IsKeyDown(toggleFullscreen);
            toggleDebugPressed = keyboardState.IsKeyDown(toggleDebug);
            togglePausePressed = keyboardState.IsKeyDown(togglePause);

            resetPlayerPressed = keyboardState.IsKeyDown(resetPlayer);

            menuPressed = keyboardState.IsKeyDown(menu);


            //======================================
            //
            //   determine just pressed buttons
            //
            //======================================

            leftJustPressed = leftPressed && !leftPressedBefore ? true : false;
            rightJustPressed = rightPressed && !rightPressedBefore ? true : false;
            upJustPressed = upPressed && !upPressedBefore ? true : false;
            downJustPressed = downPressed && !downPressedBefore ? true : false;
            aJustPressed = aPressed && !aPressedBefore ? true : false;
            bJustPressed = bPressed && !bPressedBefore ? true : false;

            cameraUpJustPressed = cameraUpPressed && !cameraUpPressedBefore ? true : false;
            cameraDownJustPressed = cameraDownPressed && !cameraDownPressedBefore ? true : false;
            cameraLeftJustPressed = cameraLeftPressed && !cameraLeftPressedBefore ? true : false;
            cameraRightJustPressed = cameraRightPressed && !cameraRightPressedBefore ? true : false;

            zoomInJustPressed = zoomInPressed && !zoomInPressedBefore ? true : false;
            zoomOutJustPressed = zoomOutPressed && !zoomOutPressedBefore ? true : false;

            toggleTrackingJustPressed = toggleTrackingPressed && !toggleTrackingPressedBefore ? true : false;
            toggleFullscreenJustPressed = toggleFullscreenPressed && !toggleFullscreenPressedBefore ? true : false;
            toggleDebugJustPressed = toggleDebugPressed && !toggleDebugPressedBefore ? true : false;
            togglePauseJustPressed = togglePausePressed && !togglePausePressedBefore ? true : false;

            resetPlayerJustPressed = resetPlayerPressed && !resetPlayerPressedBefore ? true : false;

            menuJustPressed = menuPressed && !menuPressedBefore ? true : false;
        }
    }
}
