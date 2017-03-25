using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class GameObject
    {
        public Texture2D Tex { get; set; }
        public Vector2 Position { get; set; }
        public HoveringContext HoveringContext { get; set; }

        private bool _hovering;
        public bool hovering
        {
            get { return _hovering; }
            set { _hovering = value; Color = _hovering ? Color.Green : Color.White; }
        }
        public Color Color { get; set; }

        public GameObject(Texture2D tex, Vector2 position, HoveringContext hoveringContext = HoveringContext.None)
        {
            Tex = tex;
            Position = position;
            HoveringContext = hoveringContext;
            Color = Color.White;
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Tex, Position, null, Color);
        }

        public Rectangle BoundingBox()
        {
            //Assumes the texture isn't scaled
            return new Rectangle(Position.ToPoint(), new Point(Tex.Width, Tex.Height));
        }

        public bool ContainsPoint(Vector2 point)
        {
            return BoundingBox().Contains(point);
        }
    }
}
