using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Lightning_Bolt: Projectile
    {
        int damage;
        Enemy target;
        public Lightning_Bolt(Vector2 position, Texture2D tex, Enemy target, int damage)
        {
            this.position = position;
            this.tex = tex;
            this.target = target;
            this.damage = damage;
        }
        public override bool Move()
        {
            float rise = (Math.Abs(position.Y - target.pos.Y));
            float run = Math.Abs(position.X - target.pos.X);
            float x = (float)Math.Sqrt(speed * (run / rise));
            float y = (float)Math.Sqrt(speed * (rise / run));
            if (target.pos.X >= position.X && target.pos.Y >= position.Y)
                position = new Vector2(position.X + x, position.Y + y);
            else if (target.pos.X < position.X && target.pos.Y > position.Y)
                position = new Vector2(position.X - x, position.Y + y);
            else if (target.pos.X > position.X && target.pos.Y < position.Y)
                position = new Vector2(position.X + x, position.Y - y);
            else if (target.pos.X <= position.X && target.pos.Y <= position.Y)
                position = new Vector2(position.X - x, position.Y - y);
            if (-10 < (position.X - target.pos.X) && (position.X - target.pos.X) < 10 && -10 < (position.Y - target.pos.Y) && (position.Y - target.pos.Y) < 10)
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
