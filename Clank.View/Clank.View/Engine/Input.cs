using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Clank.View.Engine
{
    public static class Input
    {
        static GamePadState s_lastFrameGamepadState;
        static GamePadState s_thisGamepadState;
        static KeyboardState s_lastFrameState;
        static KeyboardState s_thisState;
        static MouseState s_lastFrameMouseState;
        static MouseState s_thisMouseState;

        public static MouseState GetMouseState()
        {
            return s_thisMouseState;
        }

        /// <summary>
        /// Initialize the input.
        /// </summary>
        public static void ModuleInit()
        {
            s_lastFrameGamepadState = GamePad.GetState(PlayerIndex.One);
            s_thisGamepadState = s_lastFrameGamepadState;
            s_lastFrameState = Keyboard.GetState();
            s_thisState = s_lastFrameState;
            s_lastFrameMouseState = Mouse.GetState();
            s_thisMouseState = s_lastFrameMouseState;
        }
        /// <summary>
        /// Updates the input.
        /// </summary>
        public static void Update()
        {
            s_lastFrameState = s_thisState;
            s_thisState = Keyboard.GetState();

            s_lastFrameGamepadState = s_thisGamepadState;
            s_thisGamepadState = GamePad.GetState(PlayerIndex.One);

            s_lastFrameMouseState = s_thisMouseState;
            s_thisMouseState = Mouse.GetState();
        }
        /// <summary>
        /// Checks for a trigger.
        /// </summary>
        public static bool IsTrigger(Keys key)
        {
            return s_thisState.IsKeyDown(key) && !s_lastFrameState.IsKeyDown(key);
        }
        /// <summary>
        /// Checks if a key is pressed.
        /// </summary>
        public static bool IsPressed(Keys key)
        {
            return s_thisState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks for a trigger.
        /// </summary>
        public static bool IsGamepadTrigger(Buttons key)
        {
            return s_thisGamepadState.IsButtonDown(key) && !s_lastFrameGamepadState.IsButtonDown(key);
        }
        /// <summary>
        /// Checks if a key is pressed.
        /// </summary>
        public static bool IsGamepadPressed(Buttons key)
        {
            return s_thisGamepadState.IsButtonDown(key);
        }

        public static Vector2 GetLeftStickState()
        {
            return s_thisGamepadState.ThumbSticks.Left;
        }
        public static Vector2 GetRightStickState()
        {
            return s_thisGamepadState.ThumbSticks.Right;
        }

        public static bool IsLeftClickTrigger()
        {
            return (s_thisMouseState.LeftButton == ButtonState.Pressed) && (s_lastFrameMouseState.LeftButton == ButtonState.Released);
        }
        public static bool IsRightClickTrigger()
        {
            return (s_thisMouseState.RightButton == ButtonState.Pressed) && (s_lastFrameMouseState.RightButton == ButtonState.Released);
        }
    }
}
