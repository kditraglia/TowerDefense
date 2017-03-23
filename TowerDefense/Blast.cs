using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Blast: Projectile
    {
        int damage;
        int areaofeffect;
        List<Enemy> enemylist;
        HashSet<int> damaged = new HashSet<int>();
        float rise;
        float run;
        float x;
        float y;
        public Blast (Vector2 position, Vector2 dest, List<Enemy> enemylist, Texture2D tex, int damage, int areaofeffect)
        {
            this.position = position;
            this.dest = dest;
            this.tex = tex;
            this.damage = damage;
            this.areaofeffect = areaofeffect;
            this.enemylist = enemylist;
        }
        public void findDest()
        {
            rise = (Math.Abs(position.Y - dest.Y));
            run = Math.Abs(position.X - dest.X);
            x = (float)Math.Sqrt(speed * (run / rise));
            y = (float)Math.Sqrt(speed * (rise / run));
            if (dest.X < position.X && dest.Y > position.Y)
                x = 0 - x;
            else if (dest.X > position.X && dest.Y < position.Y)
                y = 0 - y;
            else if (dest.X <= position.X && dest.Y <= position.Y)
            {
                x = 0 - x;
                y = 0 - y;
            }
        }
        public override bool Move()
        {
            Damage();
            position = new Vector2(position.X + x, position.Y + y);

            if ( position.X > 704 || position.X < 128 || position.Y > 724 || position.Y < 64 )
                return true;
            else
                return false;
        }
        public override void Damage()
        {
            foreach (Enemy e in enemylist)
            {
                if ((int)Math.Sqrt(Math.Pow(this.position.X - e.pos.X, 2) + Math.Pow(this.position.Y - e.pos.Y, 2)) <= areaofeffect && e.spawned && !e.dead)
                {
                    if (!damaged.Contains(e.ID))
                    {
                        e.damage(damage);
                        damaged.Add(e.ID);
                    }
                }
            }
        }
    }
}
