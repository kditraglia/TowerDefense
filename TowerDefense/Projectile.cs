using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class Projectile
    {
        public Texture2D tex;
        public Vector2 position;
        public int speed = 10;

        public Projectile(Vector2 position, Texture2D tex)
        {
            this.position = position;
            this.tex = tex;
        }
        public void Draw(SpriteBatch batch) 
        {
            batch.Draw(tex, position, null, Color.White);
        }
        public abstract bool Move();

        public abstract void Damage();
    }
}
