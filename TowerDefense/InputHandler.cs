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
        TowerSelected,
        NodeSelected
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

        private MouseState mouseState;
        private TouchCollection touchState;

        public void Update(GameTime gameTime, Vector2 gameScale)
        {
            mouseState = Mouse.GetState();
            touchState = TouchPanel.GetState();

            if (!touchState.IsConnected)
            {
                Position = (mouseState.Position.ToVector2() / gameScale).ToPoint();
                SelectionOccurring = mouseState.LeftButton == ButtonState.Pressed;
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    SelectionHandled = false;
                }
            }
            else if (touchState.Count == 1)
            {
                Position = (touchState[0].Position / gameScale).ToPoint();
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
            if (!touchState.IsConnected)
            {
                batch.Draw(ResourceManager.DefaultCursor, Position.ToVector2(), Color.White);
            }
        }

        internal void CancelSelection()
        {
            if (SelectedObject != null)
            {
                SelectedObject.Selected = false;
            }
            if (SelectionContext == SelectionContext.PlacingPortalExit)
            {
                PortalEntrance.portal = false;
                PortalEntrance.defaultSet();
                PortalEntrance = null;
            }
            SelectedObject = null;
            SelectionContext = SelectionContext.None;
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
