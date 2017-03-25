using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class Tower : GameObject
    {
        public int cost;
        public bool hovering;

        public Tower(Texture2D tex, Vector2 position) : base(tex, position) { }

        public abstract List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, double elapsedTime);

        public abstract void ShowStats(SpriteBatch batch, SpriteFont font, Viewport viewport);

        public abstract void upgrade();
    }
}
