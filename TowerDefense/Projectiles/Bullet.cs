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
        Action<int, Point> damageFunc;
        Vector2 direction;

        public Bullet(Point position, Texture2D tex, Enemy target, int damage, Action<int, Point> damageFunc) : base(tex, position)
        {
            this.target = target;
            this.damage = damage;
            this.damageFunc = damageFunc;

            direction = (position - target.Position).ToVector2();
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
        }

        public override bool Move()
        {
            Position -= (direction * speed).ToPoint();

            if (Vector2.Distance(Position.ToVector2(), target.Position.ToVector2()) < 15)
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
            damageFunc(damage, target.Position);
            target.damage(damage);
        }
    }
}
