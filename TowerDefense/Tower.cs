using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class Tower
    {
        public Texture2D tex;
        public Texture2D proj;
        public Vector2 position;
        public bool hovering;
        public Color color = Color.White;
        public int ID = 0;
        //public Tower(Vector2 position, Texture2D tex)
        //{
        //    this.tex = tex;
        //    this.position = position;
        //}
        public Tower()
        {
        }
        public void Draw(SpriteBatch batch) //Draw function, same as mousehandler one.
        {
            batch.Draw(tex, position, null, color);
        }
        public void setPos(Vector2 pos)
        {
            this.position = pos;
        }
        public abstract List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, int elapsedTime);

        public abstract void ShowStats(SpriteBatch batch, SpriteFont font, Viewport viewport);

        public abstract int getCost();

        public abstract void upgrade();


    }
}
