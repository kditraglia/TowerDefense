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
        public MouseState MouseState { get; set; }
        public TouchCollection TouchState { get; set; }
        public bool SelectionOccurring { get; set; }
        public GameObject SelectedObject { get; set; }
        public SelectionContext SelectionContext { get; set; }

        //This is one thing I'm not sure exactly where to store, but when you make the entrance of a portal, 
        //I need to hold a reference for when you place the other side to link them together
        public Node PortalEntrance { get; set; }

        private Vector2 _gameScale;

        public InputHandler(Vector2 gameScale)
        {
            _gameScale = gameScale;
        }
        public void Update(GameTime gameTime, InputHandler inputHandler)
        {
            MouseState = Mouse.GetState();
            TouchState = TouchPanel.GetState();

            if (!TouchState.IsConnected)
            {
                Position = (MouseState.Position.ToVector2() / _gameScale).ToPoint();
                SelectionOccurring = MouseState.LeftButton == ButtonState.Pressed;
            }
            else if (TouchState.Count == 1)
            {
                Position = (TouchState[0].Position / _gameScale).ToPoint();
                SelectionOccurring = true;
            }
            else
            {
                SelectionOccurring = false;
            }
        }

        private void HandleLeftClick(GameEngine gameEngine, GameMap gameMap)
        {
            //gameEngine.HandleLeftClick(this);



        }

        private void HandleRightClick(GameEngine gameEngine, GameMap gameMap)
        {
            gameEngine.HandleRightClick(this);

            if (SelectionContext == SelectionContext.PlacingPortalExit)
            {
                PortalEntrance.portal = false;
                PortalEntrance.defaultSet();
                PortalEntrance = null;
            }
            //else if (HoveringContext == HoveringContext.FilledNode && SelectionContext == SelectionContext.None)
            //{
            //    Node n = HoveredObject as Node;
            //    if (n.wall && gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, this, CheckForPathType.TogglingWall))
            //    {
            //        GameStats.Gold = GameStats.Gold + 1;
            //        n.wall = false;
            //        n.defaultSet();
            //        ResourceManager.SellSound.Play();
            //    }
            //    else if (n.portal && gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, this, CheckForPathType.RemovingPortal))
            //    {
            //        n.portal = false;
            //        n.defaultSet();
            //        n.portalsTo.portal = false;
            //        n.portalsTo.portalsTo = null;
            //        n.portalsTo.defaultSet();
            //        n.portalsTo = null;
            //        GameStats.Gold = GameStats.Gold + 20;
            //        ResourceManager.SellSound.Play();
            //    }
            //    else if (n.cheese && gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, this, CheckForPathType.TogglingCheese))
            //    {
            //        GameStats.Gold = GameStats.Gold + 20;
            //        n.cheese = false;
            //        n.defaultSet();
            //        ResourceManager.SellSound.Play();
            //    }
            //}
        }

        internal bool MouseInGameBounds()
        {
            int topY = Constants.MapStart.Y;
            int leftX = Constants.MapStart.X;

            return Position.Y > topY && Position.X > leftX &&
                        Position.Y < topY + (Constants.MapSize.Y * Constants.NodeSize.Y) &&
                        Position.X < leftX + (Constants.MapSize.X * Constants.NodeSize.X);
        }
    }
}
