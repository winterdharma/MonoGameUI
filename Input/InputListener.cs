using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameUI.Input
{
    public class InputListener : GameComponent
    {
        private readonly List<IListener> _listeners;

        public InputListener(Game game)
            : base(game)
        {
            _listeners = new List<IListener>();
        }

        public InputListener(Game game, params IListener[] listeners)
            : base(game)
        {
            _listeners = new List<IListener>(listeners);
        }

        public IList<IListener> Listeners => _listeners;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Game.IsActive)
            {
                foreach (var listener in _listeners)
                    listener.Update(gameTime);
            }

            //GamePadListener.CheckConnections();
        }
    }

    public abstract class InputListenerSettings<T>
        where T : IListener
    {
        public abstract T CreateListener();
    }
}
