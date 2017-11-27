using MonoGameUI.Base;
using System;

namespace MonoGameUI.Events
{
    public class ElementEventArgs : EventArgs
    {
        public Element Element { get; }
        public int ScrollWheelChange { get; }

        public ElementEventArgs(Element element, int scrollWheelChange = 0)
        {
            Element = element;
            ScrollWheelChange = scrollWheelChange;
        }
    }
}