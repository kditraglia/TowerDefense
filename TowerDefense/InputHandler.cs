using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace TowerDefense
{
    public enum SelectionContext
    {
        None,
        PlacingTower,
        PlacingWall,
        PlacingPortalEntrance,
        PlacingPortalExit,
        PlacingCheese,
        TowerSelected
    }

    class InputHandler
    {
        public Point Position { get; set; }
        public bool SelectionOccurring { get; set; }
        public bool SelectionHandled { get; set; }
        public GameObject SelectedObject { get; set; }
        public SelectionContext SelectionContext { get; set; }

        //This is one thing I'm not sure exactly where to store, but when you make the entrance of a portal, 
        //I need to hold a reference for when you place the other side to link them together
        public Node PortalEntrance { get; set; }

        private Vector2 _gameScale;
        private MouseState _mouseState;
        private TouchCollection _touchState;

        public InputHandler(Vector2 gameScale)
        {
            _gameScale = gameScale;
        }
        public void Update(GameTime gameTime)
        {
            _mouseState = Mouse.GetState();
            _touchState = TouchPanel.GetState();

            if (!_touchState.IsConnected)
            {
                Position = (_mouseState.Position.ToVector2() / _gameScale).ToPoint();
                SelectionOccurring = _mouseState.LeftButton == ButtonState.Pressed;
            }
            else if (_touchState.Count == 1)
            {
                Position = (_touchState[0].Position / _gameScale).ToPoint();
                SelectionOccurring = true;
            }
            else
            {
                SelectionOccurring = false;
                SelectionHandled = false;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            SelectedObject?.ShowStats(batch);
        }

        internal void CancelSelection()
        {
            if (SelectedObject != null)
            {
                SelectedObject.Selected = false;
            }
            SelectedObject = null;
            SelectionContext = SelectionContext.None;
        }

        private void HandleRightClick(GameEngine gameEngine, GameMap gameMap)
        {
            //gameEngine.HandleRightClick(this);

            //if (SelectionContext == SelectionContext.PlacingPortalExit)
            //{
            //    PortalEntrance.portal = false;
            //    PortalEntrance.defaultSet();
            //    PortalEntrance = null;
            //}
        }

        internal bool SelectionInGameBounds()
        {
            int topY = Constants.MapStart.Y;
            int leftX = Constants.MapStart.X;

            return Position.Y > topY && Position.X > leftX &&
                        Position.Y < topY + (Constants.MapSize.Y * Constants.NodeSize.Y) &&
                        Position.X < leftX + (Constants.MapSize.X * Constants.NodeSize.X);
        }
    }
}
