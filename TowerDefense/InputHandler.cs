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

    class InputHandler : GameObject
    {
        public MouseState MouseState { get; set; }
        public TouchCollection TouchState { get; set; }
        public bool MouseClicked { get; set; }
        public GameObject SelectedObject { get; set; }
        public SelectionContext SelectionContext { get; set; }

        //This is one thing I'm not sure exactly where to store, but when you make the entrance of a portal, 
        //I need to hold a reference for when you place the other side to link them together
        public Node PortalEntrance { get; set; }

        private Vector2 _gameScale;

        public InputHandler(Vector2 gameScale) : base(ResourceManager.DefaultCursor, Point.Zero)
        {
            _gameScale = gameScale;
        }
        public void Update(GameTime gameTime, GameEngine gameEngine, GameMap gameMap)
        {
            MouseState = Mouse.GetState();
            TouchState = TouchPanel.GetState();

            if (!TouchState.IsConnected)
            {
                Position = (MouseState.Position.ToVector2() / _gameScale).ToPoint();
            }
            else if (TouchState.Count == 1)
            {
                Position = (TouchState[0].Position / _gameScale).ToPoint();
            }

            if (GameStats.PlayerLoses)
            {
                return;
            }

            if (!GameStats.AttackPhase)
            {
            //    if (MouseState.LeftButton == ButtonState.Pressed)
            //    {
            //        //I took this out of the HandleLeftClick method, since it's the only thing I want to allow holding the button down
            //        //It doesn't feel right to not be able to drag the mouse when making walls
            //        if (SelectionContext == SelectionContext.PlacingWall && HoveringContext == HoveringContext.EmptyNode)
            //        {
            //            Node n = HoveredObject as Node;
            //            if (!gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, this, CheckForPathType.TogglingWall))
            //            {
            //                MessageLog.IllegalPosition();
            //            }
            //            else if (GameStats.Gold >= 1)
            //            {
            //                n.wall = true;
            //                n.UpdateTex(Tex);
            //                GameStats.Gold = GameStats.Gold - 1;
            //                ResourceManager.WallSound.Play();
            //            }
            //            else
            //            {
            //                MessageLog.NotEnoughGold();
            //            }
            //        }
            //    }
            //    if (MouseState.LeftButton == ButtonState.Pressed && !MouseClicked)
            //    {
            //        HandleLeftClick(gameEngine, gameMap);
            //        MouseClicked = true;
            //    }
            //    if (MouseState.RightButton == ButtonState.Pressed && !MouseClicked)
            //    {
            //        HandleRightClick(gameEngine, gameMap);
            //        MouseClicked = true;
            //    }
            //    if (MouseState.LeftButton == ButtonState.Released && MouseState.RightButton == ButtonState.Released)
            //    {
            //        MouseClicked = false;
            //    }
            }

            //HoveringContext = HoveringContext.None;
            //HoveredObject = null;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {

            batch.Draw(Tex, Position.ToVector2(), new Rectangle(new Point(currentFrame * SpriteWidth, 0), new Point(SpriteWidth, SpriteHeight)), Color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        private void HandleLeftClick(GameEngine gameEngine, GameMap gameMap)
        {
            //if (HoveredObject != null)
            //{
            //    HoveredObject.HandleLeftClick(this);
            //}
            //if (HoveringContext == HoveringContext.ButtonStart && !GameStats.AttackPhase && SelectionContext == SelectionContext.None)
            //{
            //    UpdateTex(ResourceManager.DefaultCursor);
            //    gameEngine.StartLevel(gameMap.GetBestPath());
            //}
            //gameEngine.HandleLeftClick(this);

            //if (SelectionContext == SelectionContext.PlacingPortalEntrance && HoveringContext == HoveringContext.EmptyNode)
            //{
            //    Node n = HoveredObject as Node;
            //    n.portal = true;
            //    n.UpdateTex(Tex);
            //    PortalEntrance = n;
            //    SelectionContext = SelectionContext.PlacingPortalExit;
            //}
            //else if (SelectionContext == SelectionContext.PlacingPortalExit && HoveringContext == HoveringContext.EmptyNode)
            //{
            //    Node portalExit = HoveredObject as Node;
            //    Node portalEntrance = PortalEntrance;
            //    if (!gameMap.CheckForPath(portalExit.simplePos.X, portalExit.simplePos.Y, this, CheckForPathType.AddingPortal))
            //    {
            //        MessageLog.IllegalPosition();
            //    }
            //    else if (GameStats.Gold >= 20)
            //    {
            //        portalExit.portal = true;
            //        portalExit.UpdateTex(Tex);
            //        portalExit.portalsTo = portalEntrance;
            //        portalEntrance.portalsTo = portalExit;
            //        SelectionContext = SelectionContext.PlacingPortalEntrance;
            //        GameStats.Gold = GameStats.Gold - 20;
            //    }
            //    else
            //    {
            //        MessageLog.NotEnoughGold();
            //    }
            //}
            //else if (SelectionContext == SelectionContext.PlacingCheese && HoveringContext == HoveringContext.EmptyNode)
            //{
            //    Node n = HoveredObject as Node;
            //    if (!gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, this, CheckForPathType.TogglingCheese))
            //    {
            //        MessageLog.IllegalPosition();
            //    }
            //    else if (GameStats.Gold >= 20)
            //    {
            //        n.cheese = true;
            //        n.UpdateTex(Tex);
            //        GameStats.Gold = GameStats.Gold - 20;
            //        ResourceManager.WallSound.Play();
            //    }
            //    else
            //    {
            //        MessageLog.NotEnoughGold();
            //    }
            //}
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

            UpdateTex(ResourceManager.DefaultCursor);
            SelectionContext = SelectionContext.None;
        }

        internal bool MouseInGameBounds()
        {
            int topY = Constants.MapStart.Y;
            int leftX = Constants.MapStart.X;

            return Position.Y > topY && Position.X > leftX &&
                        Position.Y < topY + (Constants.MapSize.Y * Constants.NodeSize.Y) &&
                        Position.X < leftX + (Constants.MapSize.X * Constants.NodeSize.X);
        }

        public void UpdateTex(Texture2D tex)
        {
            UpdateTex(tex, tex.Bounds.Height, tex.Bounds.Width, 0);
        }

        public void UpdateTex(Texture2D tex, int spriteHeight, int spriteWidth, int frameCount)
        {
            this.Tex = tex;
            this.SpriteHeight = spriteHeight;
            this.SpriteWidth = spriteWidth;
            this.frameCount = frameCount;
            this.currentFrame = 0;
        }
    }
}
