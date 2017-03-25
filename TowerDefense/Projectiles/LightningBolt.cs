using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class LightningBolt: Projectile
    {
        int damage;
        Enemy target;
        public LightningBolt(Vector2 position, Texture2D tex, Enemy target, int damage) : base(tex, position)
        {
            this.target = target;
            this.damage = damage;
        }
        public override bool Move()
        {
            float rise = (Math.Abs(position.Y - target.position.Y));
            float run = Math.Abs(position.X - target.position.X);
            float x = (float)Math.Sqrt(speed * (run / rise));
            float y = (float)Math.Sqrt(speed * (rise / run));
            if (target.position.X >= position.X && target.position.Y >= position.Y)
                position = new Vector2(position.X + x, position.Y + y);
            else if (target.position.X < position.X && target.position.Y > position.Y)
                position = new Vector2(position.X - x, position.Y + y);
            else if (target.position.X > position.X && target.position.Y < position.Y)
                position = new Vector2(position.X + x, position.Y - y);
            else if (target.position.X <= position.X && target.position.Y <= position.Y)
                position = new Vector2(position.X - x, position.Y - y);
            if (-10 < (position.X - target.position.X) && (position.X - target.position.X) < 10 && -10 < (position.Y - target.position.Y) && (position.Y - target.position.Y) < 10)
            {
                Damage();
                return true;
            }
            else
                return false;
        }
        public override void Damage()
        {
            target.damage(damage);
        }



    }
}
