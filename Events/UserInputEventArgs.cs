using Microsoft.Xna.Framework.Input;
using MonoGameUI.Base;
using System;

namespace MonoGameUI.Events
{
    public enum EventType
    {
        LeftClick,
        RightClick,
        DoublClick,
        MouseOver,
        MouseGone,
        ScrollWheel,
        Keyboard
    }

    public class UserInputEventArgs : EventArgs
    {
        public object EventSource { get; }
        public EventType EventType { get; }

        public UserInputEventArgs(EventType eventType, Element elementSource = null,
            Keys keySource = Keys.None)
        {
            if ((elementSource == null && keySource == Keys.None) || 
                (elementSource != null && keySource != Keys.None))
                throw new ArgumentException("One source is required, either an Element or a Key.");

            if (elementSource != null)
                EventSource = elementSource;
            else
                EventSource = keySource;

            EventType = eventType;
        }
    }
}