using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameUI.Events
{
    public class MouseEventArgs : EventArgs
    {
        // This is adapted from MonoGame.Extended. I've removed the unused ViewAdapter references
        // which were intended to adjust the Mouse Position according to a scaled window.
        public MouseEventArgs(TimeSpan time, MouseState previousState, MouseState currentState,
            MouseButton button = MouseButton.None)
        {
            PreviousState = previousState;
            CurrentState = currentState;
            Position = new Point(currentState.X, currentState.Y);
            Button = button;
            ScrollWheelValue = currentState.ScrollWheelValue;
            ScrollWheelDelta = currentState.ScrollWheelValue - previousState.ScrollWheelValue;
            Time = time;
        }

        public TimeSpan Time { get; private set; }

        public MouseState PreviousState { get; }
        public MouseState CurrentState { get; }
        public Point Position { get; private set; }
        public MouseButton Button { get; private set; }
        public int ScrollWheelValue { get; private set; }
        public int ScrollWheelDelta { get; private set; }

        public Vector2 DistanceMoved => CurrentState.Position.ToVector2() 
            - PreviousState.Position.ToVector2();
    }

    [Flags]
    public enum MouseButton
    {
        None,
        Left,
        Middle,
        Right,
        XButton1,
        XButton2
    }
}