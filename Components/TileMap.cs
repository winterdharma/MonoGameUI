using MonoGameUI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Elements;

namespace MonoGameUI.Components
{
    /// <summary>
    /// This low-level TileMap takes a tilesheet Texture2D and converts it to a hash of
    /// Textures2Ds indexed with integers. It provides an API that loads entire layers,
    /// adds and removes individual images, updates and draws the tilemap. The tilemap
    /// doesn't support animations yet, but it does support an arbitrary number of layers.
    /// 
    /// This class is intended to be extended when actually implemented for a game to 
    /// add game-specific functions like the interface with a game's model, meaningful
    /// tile names with an enum, etc.
    /// </summary>
    public class TileMap : Component
    {
        #region Fields
        protected int _tileSize;
        protected Dictionary<int, Texture2D> _tileTextures;
        protected Rectangle _viewport;
        protected Vector2 _viewportCenter;
        protected Rectangle _mapRectangle;
        #endregion

        #region Constructor
        public TileMap(Scene parent, int drawOrder, Texture2D tileSheet, Point mapSizeInTiles, Rectangle viewPort, int layers = 1, int tileSize = 32) : base(parent, drawOrder)
        {
            _tileSize = tileSize;
            _tileTextures = InitializeTileTextures(tileSheet);
            _mapRectangle = new Rectangle(0, 0, 
                mapSizeInTiles.X * tileSize, mapSizeInTiles.Y * tileSize);
            _viewport = viewPort;
            _viewportCenter = new Vector2(_viewport.Center.X, _viewport.Center.Y);
        }
        #endregion

        #region Properties
        public Dictionary<int, Texture2D> TileTextures { get => _tileTextures; }
        public Rectangle MapRectangle { get => _mapRectangle; }
        public Rectangle Viewport { get => _viewport; }
        #endregion

        #region Initialization
        protected override Rectangle UpdatePanelRectangle()
        {
            return new Rectangle();
        }

        protected Dictionary<int, Texture2D> InitializeTileTextures(Texture2D tileSheet)
        {
            Point[] texturePositions = GetTileTexturePositions(tileSheet.Bounds);
            var textures = new Dictionary<int, Texture2D>();

            for(int i = 0; i < texturePositions.Length; i++)
            {
                textures[i] = GetTileTexture(tileSheet, texturePositions[i]);
            }

            return textures;
        }

        protected Point[] GetTileTexturePositions(Rectangle sheetSize)
        {
            if((sheetSize.Width % _tileSize != 0) ||
                (sheetSize.Height % _tileSize != 0))
            {
                throw new Exception("Tilesheet doesn't divide evenly by given tile size");
            }

            int width = sheetSize.Width / _tileSize;
            int height = sheetSize.Height / _tileSize;
            Point[] positions = new Point[width * height];

            for(int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    positions[(y * width) + x] = new Point(x * _tileSize, y * _tileSize);
                }
            }
            return positions;
        }
        #endregion

        #region Public API
        public void LoadLayers(int[,] tileLayers)
        {
            for(int i = 0; i < tileLayers.GetLength(0); i++)
            {
                for(int y = 0; y < tileLayers.GetLength(1); y++)
                {
                    int mapWidth = (int)Math.Sqrt(tileLayers.GetLength(1));
                    AddImage((y % mapWidth), (y / mapWidth), tileLayers[i , y], i);
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
                _tileTextures[textureId], layer);
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

        protected Texture2D GetTileTexture(Texture2D tileSheet, Point position)
        {
            Rectangle sourceRectangle = new Rectangle(position.X, position.Y, _tileSize, _tileSize);
            Texture2D texture = new Texture2D(Parent.Game.GraphicsDevice, sourceRectangle.Width, 
                sourceRectangle.Height);
            Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];

            tileSheet.GetData(0, sourceRectangle, data, 0, data.Length);
            texture.SetData(data);

            return texture;
        }
        #endregion
    }
}
