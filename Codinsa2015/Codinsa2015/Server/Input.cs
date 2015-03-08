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
namespace Codinsa2015.Server
{
    public static class Input
    {
        static GamePadState s_lastFrameGamepadState;
        static GamePadState s_thisGamepadState;
        static KeyboardState s_lastFrameState;
        static KeyboardState s_thisState;
        static MouseState s_lastFrameMouseState;
        static MouseState s_thisMouseState;
        static List<Keys> s_triggeredKeys;
        static List<Keys> s_releasedKeys;
        static bool s_clickCanceled;
        static object s_focus;


        #region Focus
        public static void TakeFocus(object owner)
        {
            s_focus = owner;
        }

        public static void ReleaseFocus(object owner)
        {
            if (HasFocus(owner))
                s_focus = null;
        }

        public static bool HasFocus(object owner)
        {
            return s_focus == owner;
        }
        #endregion
        public static MouseState GetMouseState()
        {

            return s_thisMouseState;
        }

        public static void SetMousePosition(int x, int y)
        {
            s_thisMouseState = new MouseState(x, y, s_thisMouseState.ScrollWheelValue, s_thisMouseState.LeftButton, s_thisMouseState.MiddleButton, s_thisMouseState.RightButton, s_thisMouseState.XButton1,
                s_thisMouseState.XButton2);
            Microsoft.Xna.Framework.Input.Mouse.SetPosition(x, y);
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

            s_triggeredKeys = ComputeTriggerKeys();
            s_releasedKeys = ComputeReleasedKeys();
            s_clickCanceled = false;


        }
        /// <summary>
        /// Obtient la liste des touches qui ont été appuyées durant cette frame. 
        /// </summary>
        /// <returns></returns>
        public static List<Keys> GetTriggerKeys()
        {
            return s_triggeredKeys;
        }

        /// <summary>
        /// Les prochains clicks après cet appels seront annulés.
        /// </summary>
        public static void CancelClick()
        {
            s_clickCanceled = true;   
        }

        /// <summary>
        /// Calcule et obtient la liste des touches qui ont été relâchées durant cette frame.
        /// </summary>
        /// <returns></returns>
        static List<Keys> ComputeReleasedKeys()
        {
            Keys[] thisKeys = s_thisState.GetPressedKeys();
            Keys[] oldKeys = s_lastFrameState.GetPressedKeys();
            List<Keys> released = new List<Keys>();
            foreach (Keys key in oldKeys)
            {
                if (!thisKeys.Contains(key))
                {
                    released.Add(key);
                }
            }

            return released;
        }
        /// <summary>
        /// Calcule et obtient la liste des touches qui ont été appuyées durant cette frame.
        /// </summary>
        /// <returns></returns>
        static List<Keys> ComputeTriggerKeys()
        {
            Keys[] thisKeys = s_thisState.GetPressedKeys();
            Keys[] oldKeys = s_lastFrameState.GetPressedKeys();
            List<Keys> triggerKey = new List<Keys>();
            foreach (Keys key in thisKeys)
            {
                if (!oldKeys.Contains(key))
                {
                    triggerKey.Add(key);
                }
            }

            return triggerKey;
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
        /// Checks if a key is released.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsReleased(Keys key)
        {
            return s_lastFrameState.IsKeyDown(key) && s_thisState.IsKeyUp(key);
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
        public static bool IsLeftClickReleased()
        {
            return (s_thisMouseState.LeftButton == ButtonState.Released) && (s_lastFrameMouseState.LeftButton == ButtonState.Pressed);
        }
        public static bool IsRightClickReleased()
        {
            return (s_thisMouseState.RightButton == ButtonState.Released) && (s_lastFrameMouseState.RightButton == ButtonState.Pressed);
        }
        public static bool IsLeftClickPressed()
        {
            return (s_thisMouseState.LeftButton == ButtonState.Pressed) && !s_clickCanceled;
        }
        public static bool IsRightClickPressed()
        {
            return (s_thisMouseState.RightButton == ButtonState.Pressed) && !s_clickCanceled;
        }
        public static bool IsLeftClickTrigger()
        {
            return (s_thisMouseState.LeftButton == ButtonState.Pressed) && (s_lastFrameMouseState.LeftButton == ButtonState.Released) && !s_clickCanceled;
        }
        public static bool IsRightClickTrigger()
        {
            return (s_thisMouseState.RightButton == ButtonState.Pressed) && (s_lastFrameMouseState.RightButton == ButtonState.Released) && !s_clickCanceled;
        }

        #region With focus

        public static bool IsTrigger(Keys key, object owner) { return IsTrigger(key) && HasFocus(owner); }
        public static bool IsPressed(Keys key, object owner) { return IsPressed(key) && HasFocus(owner); }

        public static bool IsLeftClickPressed(object owner) { return IsLeftClickPressed() && HasFocus(owner); }
        public static bool IsRightClickPressed(object owner) { return IsRightClickPressed() && HasFocus(owner); }
        public static bool IsLeftClickTrigger(object owner) { return IsLeftClickTrigger() && HasFocus(owner); }
        public static bool IsRightClickTrigger(object owner) { return IsRightClickTrigger() && HasFocus(owner); }
        
        #endregion
    }
}
