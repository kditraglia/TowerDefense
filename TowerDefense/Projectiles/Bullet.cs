using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Bullet: Projectile
    {
        Enemy target;
        int damage;

         public Bullet(Vector2 position, Texture2D tex, Enemy target, int damage) : base(tex, position)
        {
            this.target = target;
            this.damage = damage;
        }

         public override bool Move()
         {
             float rise = (Math.Abs(Position.Y - target.Position.Y));
             float run = Math.Abs(Position.X - target.Position.X);
             float x = (float)Math.Sqrt(speed * (run / rise));
             float y = (float)Math.Sqrt(speed * (rise / run));
             if (target.Position.X >= Position.X && target.Position.Y >= Position.Y)
                 Position = new Vector2(Position.X + x,Position.Y + y);
             else if (target.Position.X < Position.X && target.Position.Y > Position.Y)
                 Position = new Vector2(Position.X - x,Position.Y + y);
             else if (target.Position.X > Position.X && target.Position.Y < Position.Y)
                 Position = new Vector2(Position.X + x,Position.Y - y);
             else if (target.Position.X <= Position.X && target.Position.Y <= Position.Y)
                 Position = new Vector2(Position.X - x, Position.Y - y);
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
             target.damage( damage );
         }
    }
}
