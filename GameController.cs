using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameUI.Input;
using MonoGameUI.Base;
using MonoGameUI.Events;

namespace MonoGameUI
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameController : Game
    {
        #region Fields
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;
        protected Rectangle _screenRectangle;
        protected Scene _currentScene;
        #endregion

        #region Properties
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }
        public Rectangle ScreenRectangle { get { return _screenRectangle; } }
        public InputListener InputListener { get; private set; }
        public InputResponder InputResponder { get; private set; }

        public Scene CurrentScene
        {
            get => _currentScene;
            set
            {
                if (_currentScene != null)
                    _currentScene.Hide();
                _currentScene = value;
                _currentScene.Show();
            }
        }
        #endregion




        #region Constructors
        public GameController()
        {
            _graphics = new GraphicsDeviceManager(this);
            // set your screen size
            //_screenRectangle = new Rectangle(originX, originY, width, height);
            //_graphics.PreferredBackBufferWidth = _screenRectangle.Width;
            //_graphics.PreferredBackBufferHeight = _screenRectangle.Height;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            var keyboard = new KeyboardListener();
            var mouseSettings = new MouseListenerSettings
            {
                DoubleClickMilliseconds = 250
            };
            var mouse = new MouseListener(mouseSettings);
            InputListener = new InputListener(this, keyboard, mouse);
            InputResponder = new InputResponder(keyboard, mouse);
            keyboard.KeyPressed += CheckForExit;
        }
        #endregion

        #region Event Handling
        private void CheckForExit(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.Escape) Exit();
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialize your scenes and assign the CurrentScene here
        }


        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        protected override void LoadContent()
        {
            // load game-wide content such as fonts here
        }
        #endregion

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update.</param>
        protected override void Update(GameTime gameTime)
        {
            InputListener.Update(gameTime);
            CurrentScene?.Update(gameTime);
        }



        /// <summary>This is called when the game should draw itself.</summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // clear the previous frame
            GraphicsDevice.Clear(Color.Black);
            CurrentScene?.Draw(gameTime);
        }
    }
}
