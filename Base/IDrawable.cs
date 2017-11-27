using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameUI.Base
{
    public interface IDrawable
    {
        int DrawOrder { get; }
        bool IsMouseOver { get; }
        Rectangle Rectangle { get; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
