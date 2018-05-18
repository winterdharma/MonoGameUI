using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameUI.Elements
{
    /// <summary>
    /// Sprite extends Image to allow it to have multiple Texture2D frames that it can display. 
    /// It accepts a spritesheet and creates a Dictionary of frames that crop it into individual 
    /// images. It is not meant to be animated as-is, but could be used to create a simple animated 
    /// loop by externally updating its CurrentFrame property.
    /// 
    /// This class is intended to be extended for a game implementation.
    /// </summary>
    public class Sprite : Image
    {

        #region Fields
        protected Rectangle _frame;
        #endregion

        #region Constructors
        public Sprite(string id, Component parent, Vector2 position, Texture2D spritesheet,
            Color notHighlighted, Color highlighted, int draworder, int tilesize = 32) : 
            base(id, parent, position, spritesheet, notHighlighted, highlighted, draworder)
        {
            InitializeSprite(tilesize);
        }

        public Sprite(string id, Component parent, Point position, Texture2D spritesheet,
            Color notHighlighted, Color highlighted, int draworder, int tilesize = 32) :
            base(id, parent, position, spritesheet, notHighlighted, highlighted, draworder)
        {
            InitializeSprite(tilesize);
        }

        public Sprite(string id, Component parent, Vector2 position, Texture2D spritesheet,
            int draworder, int tilesize = 32) :
            base(id, parent, position, spritesheet, Color.White, Color.White, draworder)
        {
            InitializeSprite(tilesize);
        }

        public Sprite(string id, Component parent, Point position, Texture2D spritesheet,
            int draworder, int tilesize = 32) :
            base(id, parent, position, spritesheet, Color.White, Color.White, draworder)
        {
            InitializeSprite(tilesize);
        }
        #endregion

        #region Properties
        public Dictionary<int, Rectangle> Frames { get; set; }
        public int CurrentFrame { get; set; }
        #endregion

        #region Initialization Methods
        private void InitializeSprite(int tilesize)
        {
            _frame = new Rectangle(0, 0, tilesize, tilesize);
            Frames = CreateFrames();
            CurrentFrame = 0;
            Rectangle = CreateRectangle(); 
        }

        /// <summary>
        /// Creates a dictionary of Rectangles indexed with integer ids, counting from
        /// the upper left corner, left to right, top to bottom, with last frame in bottom
        /// right corner. Throws exceptions if texture does not divide exactly into 
        /// multiples of the _framesize dimensions.
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, Rectangle> CreateFrames()
        {
            int rows = GetNumberOfFrames(Texture.Height, _frame.Size.Y);
            int cols = GetNumberOfFrames(Texture.Width, _frame.Size.X);

            var frames = new Dictionary<int, Rectangle>();
            int index = 0;
            Point position;
            for(int y = 0; y < rows; y++)
            {
                for(int x = 0; x < cols; x++)
                {
                    position = new Point(x * _frame.Width, y * _frame.Height);
                    frames[index] = new Rectangle(position, _frame.Size);
                    index++;
                }
            }

            return frames;
        }

        private int GetNumberOfFrames(int textureSize, int frameSize)
        {
            if (textureSize % frameSize != 0)
                throw new Exception("Spritesheet does not divide into evenly.");
            return textureSize / frameSize;
        }

        /// <summary>
        /// This implementation is actually useless become _frame is not initialized until the 
        /// constructor executes, but this method executes when Image initializes. So, it's done 
        /// again in InitializeSprite().
        /// </summary>
        /// <returns></returns>
        protected override Rectangle CreateRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, _frame.Height, _frame.Width);
        }
        #endregion

        #region Public API
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(Texture, Position, Frames[CurrentFrame], Color.White);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            if (Visible)
                spriteBatch.Draw(Texture, position, Frames[CurrentFrame], Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
        #endregion
    }
}
