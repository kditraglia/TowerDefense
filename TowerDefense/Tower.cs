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
        protected Texture2D proj;
        public int cost;
        public Vector2 position { get; set; }
        public bool hovering;
        public Color color = Color.White;

        public Tower(Vector2 position, Texture2D tex)
        {
            this.tex = tex;
            this.position = position;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, position, null, color);
        }

        public abstract List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, int elapsedTime);

        public abstract void ShowStats(SpriteBatch batch, SpriteFont font, Viewport viewport);

        public abstract void upgrade();
    }
}
