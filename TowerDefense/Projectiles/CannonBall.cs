using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class CannonBall: Projectile
    {
        Point dest;
        int damage;
        int areaofeffect;
        List<Enemy> enemylist;
        public CannonBall (Point position, Texture2D tex, Point dest, List<Enemy> enemylist, int damage, int areaofeffect) : base(tex, position)
        {
            this.dest = dest;
            this.Tex = tex;
            this.damage = damage;
            this.areaofeffect = areaofeffect;
            this.enemylist = enemylist;
        }
        public override bool Move()
        {
            int rise = (Math.Abs(Position.Y - dest.Y));
            int run = Math.Abs(Position.X - dest.X);
            int x = (int)Math.Sqrt(speed * (run / rise));
            int y = (int)Math.Sqrt(speed * (rise / run));
            if (dest.X >= Position.X && dest.Y >= Position.Y)
                Position = new Point(Position.X + x, Position.Y + y);
            else if (dest.X < Position.X && dest.Y > Position.Y)
                Position = new Point(Position.X - x, Position.Y + y);
            else if (dest.X > Position.X && dest.Y < Position.Y)
                Position = new Point(Position.X + x, Position.Y - y);
            else if (dest.X <= Position.X && dest.Y <= Position.Y)
                Position = new Point(Position.X - x, Position.Y - y);
            if (-10 < (Position.X - dest.X) && (Position.X - dest.X) < 10 && -10 < (Position.Y - dest.Y) && (Position.Y - dest.Y) < 10)
            {
                Damage();
                return true;
            }
            else
                return false;
        }
        public override void Damage()
        {
            foreach (Enemy e in enemylist)
            {
                if ((int)Math.Sqrt(Math.Pow(this.Position.X - e.Position.X, 2) + Math.Pow(this.Position.Y - e.Position.Y, 2)) <= areaofeffect && e.spawned && !e.dead )
                    e.damage( damage );
            }
        }
    }
}
