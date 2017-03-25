using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense
{
    class Blast: Projectile
    {
        Vector2 dest;
        int damage;
        int areaofeffect;
        List<Enemy> enemylist;
        HashSet<Enemy> damaged = new HashSet<Enemy>();
        Vector2 direction;
        public Blast (Vector2 position, Texture2D tex, Vector2 dest, List<Enemy> enemylist, int damage, int areaofeffect) : base(tex, position)
        {
            this.dest = dest;
            this.damage = damage;
            this.areaofeffect = areaofeffect;
            this.enemylist = enemylist;

            direction = position - dest;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
        }

        public override bool Move()
        {
            Damage();
            position -= direction * speed;

            return position.X > 704 || position.X < 128 || position.Y > 724 || position.Y < 64;
        }
        public override void Damage()
        {
            foreach (Enemy e in enemylist)
            {
                if ((int)Math.Sqrt(Math.Pow(this.position.X - e.position.X, 2) + Math.Pow(this.position.Y - e.position.Y, 2)) <= areaofeffect && e.spawned && !e.dead)
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
