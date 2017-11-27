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
        private Element _element;
        private Keys _key;

        public object EventSource
        {
            get
            {
                if (_element != null && _key == Keys.None)
                    return _element;
                else if (_key != Keys.None && _element == null)
                    return _key;
                else
                    throw new ArgumentException("One source is required, either an Element or a Key.");
            }
        }
        public EventType EventType { get; }

        public UserInputEventArgs(EventType eventType, Element elementSource = null,
            Keys keySource = Keys.None)
        {
            _element = elementSource;
            _key = keySource;
            EventType = eventType;
        }
    }
}