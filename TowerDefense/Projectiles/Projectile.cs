using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class Projectile : GameObject
    {
        public int speed = 10;

        public Projectile(Texture2D tex, Vector2 position) : base(tex, position) { }

        public abstract bool Move();

        public abstract void Damage();
    }
}
