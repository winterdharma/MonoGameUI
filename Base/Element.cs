﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Events;
using Utilities;

namespace MonoGameUI.Base
{
    /// <summary>
    /// An Element handles the logic and content for Update() and Draw() on a discrete part of a 
    /// UI Component.
    /// </summary>
    public abstract class Element : IDrawable
    {
        #region Fields
        protected Rectangle _rectangle;
        private Vector2 _position;
        private bool _enabled;
        private bool _visible;
        #endregion

        #region Events
        public event EventHandler EnabledChanged;
        public event EventHandler VisibleChanged;
        public event EventHandler LeftClick;
        public event EventHandler RightClick;
        public event EventHandler DoubleClick;
        public event EventHandler MouseOver;
        public event EventHandler MouseGone;
        public event EventHandler<EventData<int>> ScrollWheelChange;
        #endregion

        #region Constructors
        public Element(string id, Component parent, Vector2 position, Color unhighlighted,
            Color highlighted, int drawOrder) : this(id, parent, unhighlighted, highlighted, drawOrder)
        {
            Position = position;
        }

        public Element(string id, Component parent, Point position, Color unhighlighted,
            Color highlighted, int drawOrder) : this(id, parent, unhighlighted, highlighted, drawOrder)
        {
            Position = new Vector2(position.X, position.Y);
        }

        private Element(string id, Component parent, Color unhighlighted, Color highlighted, int drawOrder)
        {
            Id = id;
            Parent = parent;
            Color = unhighlighted;
            UnhighlightedColor = unhighlighted;
            HighlightedColor = highlighted;
            DrawOrder = drawOrder;

            Visible = false;
            Enabled = false;
            IsMouseOver = false;
        }
        #endregion

        #region Properties
        public string Id { get; set; }
        public bool Visible
        {
            get => _visible;
            set
            {
                if (value != _visible)
                {
                    _visible = value;
                    VisibleChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;

                    if (_enabled) SubscribeToEvents();
                    else UnsubscribeFromEvents();

                    EnabledChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public int DrawOrder { get; set; }
        public bool IsMouseOver { get; private set; }
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (value != _position)
                {
                    _position = value;
                    _rectangle.Location = new Point((int)_position.X, (int)_position.Y);
                }
            }
        }
        public Color Color { get; set; }
        public Color HighlightedColor { get; set; }
        public Color UnhighlightedColor { get; set; }
        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }
        public Component Parent { get; set; }
        #endregion

        #region Initialization
        protected abstract Rectangle CreateRectangle();

        protected virtual void SubscribeToEvents()
        {
            Parent.Input.LeftMouseClick += OnLeftClick;
            Parent.Input.NewMousePosition += OnMouseMoved;
            Parent.Input.RightMouseClick += OnRightClick;
            Parent.Input.DoubleLeftMouseClick += OnDoubleClick;
            Parent.Input.ScrollWheelMoved += OnScrollWheelMove;
        }

        protected virtual void UnsubscribeFromEvents()
        {
            Parent.Input.NewMousePosition -= OnMouseMoved;
            Parent.Input.LeftMouseClick -= OnLeftClick;
            Parent.Input.RightMouseClick -= OnRightClick;
            Parent.Input.DoubleLeftMouseClick -= OnDoubleClick;
            Parent.Input.ScrollWheelMoved -= OnScrollWheelMove;
        }
        #endregion

        #region Event Handling
        private void OnMouseMoved(object sender, EventData<Point> eventData)
        {
            var mousePosition = eventData.Data;

            if (!IsMouseOver && Rectangle.Contains(mousePosition))
            {
                IsMouseOver = true;
                MouseOver?.Invoke(this, new EventArgs());
            }
            else if (IsMouseOver && !Rectangle.Contains(mousePosition))
            {
                IsMouseOver = false;
                MouseGone?.Invoke(this, new EventArgs());
            }
        }

        private void OnLeftClick(object sender, EventArgs e)
        {
            if (IsMouseOver)
                LeftClick?.Invoke(this, e);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (IsMouseOver)
                DoubleClick?.Invoke(this, e);
        }

        private void OnRightClick(object sender, EventArgs e)
        {
            if (IsMouseOver)
                RightClick?.Invoke(this, e);
        }

        private void OnScrollWheelMove(object sender, EventData<int> eventData)
        {
            if (IsMouseOver)
                ScrollWheelChange?.Invoke(this, eventData);
        }
        #endregion

        public virtual void Update(GameTime gameTime)
        {

        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position);

        public virtual void Show()
        {
            Visible = true;
            Enabled = true;
        }

        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
        }

        public virtual void Highlight()
        {
            Color = HighlightedColor;
        }

        public virtual void Unhighlight()
        {
            Color = UnhighlightedColor;
        }
    }
}
