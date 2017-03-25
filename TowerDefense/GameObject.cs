using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class GameObject
    {
        public Texture2D Tex { get; set; }
        public Point Position { get; set; }
        public HoveringContext HoveringContext { get; set; }

        private bool hovering;
        public bool Hovering
        {
            get { return hovering; }
            set { hovering = value; Color = hovering || selected ? Color.Green : Color.White; }
        }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; Color = hovering || selected ? Color.Green : Color.White; }
        }
        public Color Color { get; set; }

        public GameObject(Texture2D tex, Point position, HoveringContext hoveringContext = HoveringContext.None)
        {
            Tex = tex;
            Position = position;
            HoveringContext = hoveringContext;
            Color = Color.White;
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Tex, Position.ToVector2(), null, Color);
        }

        public Rectangle BoundingBox()
        {
            //Assumes the texture isn't scaled
            return new Rectangle(Position, new Point(Tex.Width, Tex.Height));
        }

        public bool ContainsPoint(Point point)
        {
            return BoundingBox().Contains(point);
        }
    }
}
