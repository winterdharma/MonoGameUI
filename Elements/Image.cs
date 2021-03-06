﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Base;

namespace MonoGameUI.Elements
{
    /// <summary>
    /// Handles the content and display logic for a discrete image drawn to the screen.
    /// </summary>
    public class Image : Element
    {
        #region Fields
        private Texture2D _texture;
        #endregion

        #region Constructors
        public Image(string id, Component parent, Vector2 position, Texture2D texture,
            Color normal, Color highlighted, int drawOrder, float scale = 1)
            : base(id, parent, position, normal, highlighted, drawOrder)
        {
            SetImageProperties(texture, scale);
        }

        public Image(string id, Component parent, Vector2 position, Texture2D texture,
            int drawOrder, float scale = 1)
            : base(id, parent, position, Color.White, Color.White, drawOrder)
        {
            SetImageProperties(texture, scale);
        }

        public Image(string id, Component parent, Point position, Texture2D texture,
            Color normal, Color highlighted, int drawOrder, float scale = 1)
            : base(id, parent, position, normal, highlighted, drawOrder)
        {
            SetImageProperties(texture, scale);
        }

        public Image(string id, Component parent, Point position, Texture2D texture,
            int drawOrder, float scale = 1)
            : base(id, parent, position, Color.White, Color.White, drawOrder)
        {
            SetImageProperties(texture, scale);
        }

        private void SetImageProperties(Texture2D texture, float scale = 1)
        {
            Texture = texture;
            Scale = scale;
            Rectangle = CreateRectangle();
        }
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get => _texture;
            set
            {
                _texture = value;
                Rectangle = CreateRectangle();
            }
        }
        public float Scale { get; set; }
        #endregion

        #region Initialization
        protected override Rectangle CreateRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y,
                (int)(Texture.Bounds.Width * Scale), (int)(Texture.Bounds.Height * Scale));
        }
        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(Texture, Position, null, Color, 0f, new Vector2(0, 0),
                    Scale, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Use this method when the Image should be drawn to a different position
        /// than is stored in its properties, such as when an offset is performed.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            if (Visible)
                spriteBatch.Draw(Texture, position, null, Color, 0f, new Vector2(0, 0),
                    Scale, SpriteEffects.None, 0f);
        }
    }
}
