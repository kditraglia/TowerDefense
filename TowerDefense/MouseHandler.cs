using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense
{
    public enum HoveringContext
    {
        None,
        ButtonStart,
        ButtonUpgrade,
        ButtonGenericTower,
        ButtonCannonTower,
        ButtonBatteryTower,
        ButtonBlastTower,
        ButtonWall,
        ButtonPortal,
        ButtonCheese,
        EmptyNode,
        FilledNode,
        Tower,
        Enemy
    }
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
    class MouseHandler
    {
        public Texture2D tex;
        public Point pos;
        public MouseState MouseState { get; set; }
        public bool MouseClicked { get; set; }
        public GameObject HoveredObject { get; set; }
        public HoveringContext HoveringContext { get; set; }
        public GameObject SelectedObject { get; set; }
        public SelectionContext SelectionContext { get; set; }

        //This is one thing I'm not sure exactly where to store, but when you make the entrance of a portal, 
        //I need to hold a reference for when you place the other side to link them together
        public Node PortalEntrance { get; set; }

        public MouseHandler(Point pos, Texture2D tex)
        {
            this.pos = pos;
            this.tex = tex;
        }
        public void Update()
        {
            MouseState = Mouse.GetState();
            pos.X = MouseState.X;
            pos.Y = MouseState.Y;
        }

        internal bool MouseInGameBounds()
        {
            int topY = Constants.MapStart.Y;
            int leftX = Constants.MapStart.X;

            return pos.Y > topY && pos.X > leftX &&
                        pos.Y < topY + (Constants.MapSize.Y * Constants.NodeSize.Y) &&
                        pos.X < leftX + (Constants.MapSize.X * Constants.NodeSize.X);
        }

        public void UpdateTex(Texture2D tex)
        {
            this.tex = tex;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, pos.ToVector2(), Color.White);
        }
    }
}
