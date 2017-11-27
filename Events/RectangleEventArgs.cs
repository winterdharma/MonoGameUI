using Microsoft.Xna.Framework;
using System;

namespace MonoGameUI.Events
{
    public class RectangleEventArgs : EventArgs
    {
        public Rectangle Rectangle { get; }
        public RectangleEventArgs(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }
    }
}
