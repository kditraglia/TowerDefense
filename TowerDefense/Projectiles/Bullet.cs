using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Bullet : Projectile
    {
        Enemy target;
        int damage;

        public Bullet(Point position, Texture2D tex, Enemy target, int damage) : base(tex, position)
        {
            this.target = target;
            this.damage = damage;
        }

        public override bool Move()
        {
            int rise = (Math.Abs(Position.Y - target.Position.Y));
            int run = Math.Abs(Position.X - target.Position.X);
            int x = (int)Math.Sqrt(speed * (run / rise));
            int y = (int)Math.Sqrt(speed * (rise / run));
            if (target.Position.X >= Position.X && target.Position.Y >= Position.Y)
            {
                Position = new Point(Position.X + x, Position.Y + y);
            }
            else if (target.Position.X < Position.X && target.Position.Y > Position.Y)
            {
                Position = new Point(Position.X - x, Position.Y + y);
            }
            else if (target.Position.X > Position.X && target.Position.Y < Position.Y)
            {
                Position = new Point(Position.X + x, Position.Y - y);
            }
            else if (target.Position.X <= Position.X && target.Position.Y <= Position.Y)
            {
                Position = new Point(Position.X - x, Position.Y - y);
            }
            if (-10 < (Position.X - target.Position.X) && (Position.X - target.Position.X) < 10 && -10 < (Position.Y - target.Position.Y) && (Position.Y - target.Position.Y) < 10)
            {
                Damage();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Damage()
        {
            target.damage(damage);
        }
    }
}
