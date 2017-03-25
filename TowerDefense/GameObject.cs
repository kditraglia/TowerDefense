using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class GameObject
    {
        public Texture2D tex { get; set; }
        public Vector2 position { get; set; }
        public Color color { get; set; }

        public GameObject(Texture2D tex, Vector2 position)
        {
            this.tex = tex;
            this.position = position;
            this.color = Color.White;
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, position, null, color);
        }

        public Rectangle BoundingBox()
        {
            //Assumes the texture isn't scaled
            return new Rectangle(position.ToPoint(), new Point(tex.Width, tex.Height));
        }

        public bool ContainsPoint(Vector2 point)
        {
            return BoundingBox().Contains(point);
        }
    }
}
