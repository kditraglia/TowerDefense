using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Cannon_Ball: Projectile
    {
        Vector2 dest;
        int damage;
        int areaofeffect;
        List<Enemy> enemylist;
        public Cannon_Ball (Vector2 position, Texture2D tex, Vector2 dest, List<Enemy> enemylist, int damage, int areaofeffect) : base(position, tex)
        {
            this.position = position;
            this.dest = dest;
            this.tex = tex;
            this.damage = damage;
            this.areaofeffect = areaofeffect;
            this.enemylist = enemylist;
        }
        public override bool Move()
        {
            float rise = (Math.Abs(position.Y - dest.Y));
            float run = Math.Abs(position.X - dest.X);
            float x = (float)Math.Sqrt(speed * (run / rise));
            float y = (float)Math.Sqrt(speed * (rise / run));
            if (dest.X >= position.X && dest.Y >= position.Y)
                position = new Vector2(position.X + x, position.Y + y);
            else if (dest.X < position.X && dest.Y > position.Y)
                position = new Vector2(position.X - x, position.Y + y);
            else if (dest.X > position.X && dest.Y < position.Y)
                position = new Vector2(position.X + x, position.Y - y);
            else if (dest.X <= position.X && dest.Y <= position.Y)
                position = new Vector2(position.X - x, position.Y - y);
            if (-10 < (position.X - dest.X) && (position.X - dest.X) < 10 && -10 < (position.Y - dest.Y) && (position.Y - dest.Y) < 10)
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
                if ((int)Math.Sqrt(Math.Pow(this.position.X - e.pos.X, 2) + Math.Pow(this.position.Y - e.pos.Y, 2)) <= areaofeffect && e.spawned && !e.dead )
                    e.damage( damage );
            }
        }
    }
}
