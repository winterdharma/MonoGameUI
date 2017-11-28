using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Base;
using MonoGameUI.Events;
using Utilities;

namespace MonoGameUI.Elements
{
    public class Background : Image
    {

        #region Constructors
        public Background(string id, Component parent, Point position, Texture2D texture,
            Color color1, Color color2, Point size, int drawOrder)
            : base(id, parent, position, texture, color1, color2, drawOrder)
        {
            Rectangle = CreateRectangle(size);
            parent.RectangleUpdated += OnPanelRectangleUpdate;
        }

        public Background(string id, Component parent, Point position, Texture2D texture,
            Point size, int drawOrder)
            : base(id, parent, position, texture, Color.White, Color.White, drawOrder)
        {
            Rectangle = CreateRectangle(size);
            parent.RectangleUpdated += OnPanelRectangleUpdate;
        }
        #endregion

        private void OnPanelRectangleUpdate(object sender, EventData<Rectangle> eventData)
        {
            Rectangle = eventData.Data;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }

        public override void Update(GameTime gameTime)
        {

        }

        protected Rectangle CreateRectangle(Point size)
        {
            return new Rectangle((int)Position.X, (int)Position.Y, size.X, size.Y);
        }
    }
}
