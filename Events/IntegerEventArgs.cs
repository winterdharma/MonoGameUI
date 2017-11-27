using System;

namespace MonoGameUI.Events
{
    public class IntegerEventArgs : EventArgs
    {
        public int Value { get; }

        public IntegerEventArgs(int value)
        {
            Value = value;
        }
    }
}
