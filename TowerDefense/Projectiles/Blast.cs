using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense
{
    class Blast: Projectile
    {
        Point dest;
        int damage;
        int areaofeffect;
        List<Enemy> enemylist;
        HashSet<Enemy> damaged = new HashSet<Enemy>();
        Vector2 direction;
        public Blast (Point position, Texture2D tex, Point dest, List<Enemy> enemylist, int damage, int areaofeffect) : base(tex, position)
        {
            this.dest = dest;
            this.damage = damage;
            this.areaofeffect = areaofeffect;
            this.enemylist = enemylist;

            direction = (position - dest).ToVector2();
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
        }

        public override bool Move()
        {
            Damage();
            Position -= (direction * speed).ToPoint();

            return Position.X > 704 || Position.X < 128 || Position.Y > 724 || Position.Y < 64;
        }
        public override void Damage()
        {
            foreach (Enemy e in enemylist)
            {
                if ((int)Math.Sqrt(Math.Pow(this.Position.X - e.Position.X, 2) + Math.Pow(this.Position.Y - e.Position.Y, 2)) <= areaofeffect && e.spawned && !e.dead)
                {
                    if (!damaged.Contains(e))
                    {
                        e.damage(damage);
                        damaged.Add(e);
                    }
                }
            }
        }
    }
}
