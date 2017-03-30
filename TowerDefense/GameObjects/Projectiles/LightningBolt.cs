using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class LightningBolt: Projectile
    {
        int damage;
        Enemy target;
        Action<int, Point> damageFunc;

        public LightningBolt(Point position, Texture2D tex, Enemy target, int damage, Action<int, Point> damageFunc) : base(tex, position)
        {
            this.target = target;
            this.damage = damage;
            this.damageFunc = damageFunc;
        }
        public override bool Move()
        {
            int rise = (Math.Abs(Position.Y - target.Position.Y));
            int run = Math.Abs(Position.X - target.Position.X);
            int x = (int)Math.Sqrt(speed * (run / rise));
            int y = (int)Math.Sqrt(speed * (rise / run));
            if (target.Position.X >= Position.X && target.Position.Y >= Position.Y)
                Position = new Point(Position.X + x, Position.Y + y);
            else if (target.Position.X < Position.X && target.Position.Y > Position.Y)
                Position = new Point(Position.X - x, Position.Y + y);
            else if (target.Position.X > Position.X && target.Position.Y < Position.Y)
                Position = new Point(Position.X + x, Position.Y - y);
            else if (target.Position.X <= Position.X && target.Position.Y <= Position.Y)
                Position = new Point(Position.X - x, Position.Y - y);
            if (-10 < (Position.X - target.Position.X) && (Position.X - target.Position.X) < 10 && -10 < (Position.Y - target.Position.Y) && (Position.Y - target.Position.Y) < 10)
            {
                Damage();
                return true;
            }
            else
                return false;
        }
        public override void Damage()
        {
            damageFunc(damage, target.Position);
            target.damage(damage);
        }



    }
}
