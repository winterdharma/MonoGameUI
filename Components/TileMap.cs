using MonoGameUI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Elements;
using MonoGameUI.Assets;

namespace MonoGameUI.Components
{
    /// <summary>
    /// This low-level TileMap handles Image elements and viewport positioning using
    /// integers ids only. It provides an API that loads multiple layers,
    /// adds and removes individual images, and updates and draws the tilemap. The tilemap
    /// doesn't support animations yet, but it does support an arbitrary number of layers.
    /// It interacts with the Textures class directly to fetch Texture2D assets.
    /// 
    /// This class is intended to be extended when actually implemented for a game to 
    /// add game-specific functions like an interface with a game's model and meaningful
    /// tile names using enums. Note that if enums are used, they must map to unique integer 
    /// values.
    /// </summary>
    public class TileMap : Component
    {
        #region Fields
        protected int _tileSize;
        protected Rectangle _viewport;
        protected Vector2 _viewportCenter;
        protected Rectangle _mapRectangle;
        #endregion

        #region Constructor
        public TileMap(Scene parent, int drawOrder, Point mapSizeInTiles, Rectangle viewPort, 
            int layers = 1, int tileSize = 32) : base(parent, drawOrder)
        {
            _tileSize = tileSize;
            _mapRectangle = new Rectangle(0, 0, 
                mapSizeInTiles.X * tileSize, mapSizeInTiles.Y * tileSize);
            _viewport = viewPort;
            _viewportCenter = new Vector2(_viewport.Center.X, _viewport.Center.Y);
        }
        #endregion

        #region Properties
        public Rectangle MapRectangle { get => _mapRectangle; }
        public Rectangle Viewport { get => _viewport; }
        #endregion

        #region Initialization
        protected override Rectangle UpdatePanelRectangle()
        {
            return new Rectangle();
        }
        #endregion

        #region Public API
        /// <summary>
        /// @tileLayers consists of 1 or more tileId layers. ints in the layer data must
        /// correspond to keys in Textures.Textures2D to fetch assets properly.
        /// </summary>
        /// <param name="tileLayers"></param>
        public void LoadLayers(int[,] tileLayers)
        {
            for(int layer = 0; layer < tileLayers.GetLength(0); layer++)
            {
                //This is a one dimensional array of a two dimensional Cartesian map.
                //First element is the bottom-left corner, going left to right, bottom to top.
                for(int index = 0; index < tileLayers.GetLength(1); index++)
                {
                    int mapWidth = (int)Math.Sqrt(tileLayers.GetLength(1));
                    AddImage((index % mapWidth), (index / mapWidth), tileLayers[layer , index], layer);
                }
            }
            var els = Elements;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateViewportPosition(_viewportCenter, _mapRectangle);
            StopViewportAtTilemapEdges(_viewport, _mapRectangle);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var element in _visibleElements)
            {
                if (_viewport.Intersects(element.Rectangle))
                {
                    Image image = element as Image;
                    image.Draw(gameTime, spriteBatch,
                        image.Position - new Vector2(_viewport.Location.X, _viewport.Location.Y));
                }
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Adds an Image element to the TileMap at the Map coordinates specified,
        /// with the texture that matches the textureId, and with a drawOrder equal
        /// to its layer number.
        /// </summary>
        /// <param name="mapTileX"></param>
        /// <param name="mapTileY"></param>
        /// <param name="textureId"></param>
        /// <param name="layer"></param>
        public void AddImage(int mapTileX, int mapTileY, int textureId, int layer)
        {
            string imageId = GetImageId(mapTileX, mapTileY);
            var elements = new Dictionary<string, Element>(Elements);
            elements[imageId] = new Image(imageId, this,
                new Vector2(mapTileX * _tileSize, mapTileY * _tileSize), 
                Textures.Textures2D[textureId], layer);
            Elements = elements;
            Elements[imageId].Show();
        }

        public void RemoveImage(int mapTileX, int mapTileY)
        {
            var elements = new Dictionary<string, Element>(Elements);
            String imageId = GetImageId(mapTileX, mapTileY);
            elements.Remove(imageId);
            Elements[imageId].Hide();
            Elements = elements;
        }
        #endregion

        #region Helper Methods
        private void StopViewportAtTilemapEdges(Rectangle viewport, Rectangle mapRectangle)
        {
            var position = new Vector2(
                MathHelper.Clamp(viewport.Location.X, 0, mapRectangle.Width - viewport.Width),
                MathHelper.Clamp(viewport.Location.Y, 0, mapRectangle.Width - viewport.Height));
            _viewport.Location = new Point((int)position.X, (int)position.Y);
        }

        private void UpdateViewportPosition(Vector2 viewportCenter, Rectangle mapRectangle)
        {
            var position = new Vector2(
                (viewportCenter.X + 16) - (_viewport.Width / 2),
                (viewportCenter.Y + 16) - (_viewport.Height / 2));
            _viewport.Location = new Point((int)position.X, (int)position.Y);
        }

        protected virtual string GetImageId(int x, int y)
        {
            return "image@[" + x + ", " + y + "]";
        }
        #endregion
    }
}
