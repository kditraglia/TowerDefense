using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class Tower : GameObject
    {
        public int Cost { get; set; }
        public CommandCard CommandCard { get; set; }
        public Tower(Texture2D tex, Point position) : base(tex, position) { }

        public abstract List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, double elapsedTime, Action<int, Point> damageFunc);
        public override void HandleLeftClick(MouseHandler mouse)
        {
            switch (mouse.HoveringContext)
            {
                case HoveringContext.Tower:
                    Tower t = mouse.HoveredObject as Tower;
                    mouse.SelectedObject = t;
                    mouse.SelectionContext = SelectionContext.TowerSelected;
                    t.Selected = true;
                    break;
            }
        }

        public override void ShowStats(SpriteBatch batch)
        {
            int Y = 200;
            int X = 1020;

            CommandCard.Draw(new Point(X, Y), batch);
        }

        public abstract void upgrade();
    }
}
