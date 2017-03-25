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
        ButtonPortal
    }
    public enum SelectionContext
    {
        None,
        PlacingTower,
        PlacingWall,
        PlacingPortal
    }
    class MouseHandler
    {
        public Texture2D tex;
        public Vector2 pos;
        public MouseState MouseState { get; set; }
        public GameObject HoveredObject { get; set; }
        public HoveringContext HoveredContext { get; set; }
        public GameObject SelectedObject { get; set; }
        public SelectionContext SelectionContext { get; set; }

        public MouseHandler(Vector2 pos, Texture2D tex)
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
        public void UpdateTex(Texture2D tex)
        {
            this.tex = tex;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, pos, Color.White);
        }
    }
}
