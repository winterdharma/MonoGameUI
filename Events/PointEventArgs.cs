using Microsoft.Xna.Framework;
using System;

namespace MonoGameUI.Events
{
    public class PointEventArgs : EventArgs
    {
        public Point Point { get; }
        public PointEventArgs(Point point)
        {
            Point = point;
        }
    }
}
