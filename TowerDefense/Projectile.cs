using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class Projectile
    {
        public Texture2D tex;
        public Vector2 position;
        public int speed = 10;
        public Vector2 dest;

        public Projectile()
        {
        }

        //public Projectile(Vector2 position, Vector2 dest, Texture2D tex)
        //{
        //    this.position = position;
        //    this.dest = dest;
        //    this.tex = tex;
        //}
        public void Draw(SpriteBatch batch) //Draw function, same as mousehandler one.
        {
            batch.Draw(tex, position, null, Color.White);
        }
        public abstract bool Move();

        public abstract void Damage();


    }
}
