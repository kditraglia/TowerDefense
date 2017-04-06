using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense
{
    class BlastTower: Tower
    {
        public string name = "Education Tower";
        public int damage = 10;
        public float attackspeed = 2.0f;
        public int range = 500;
        public int areaofeffect = 20;
        public string description = "Line damage";
        public double cooldown = 0;

        public BlastTower(Point position, Texture2D tex) : base(tex, position)
        {
            Cost = 15;
            CommandCard = new CommandCard(name, damage.ToString(), attackspeed.ToString(), range.ToString(), Cost.ToString(), description);
        }

        public override List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, double elapsedTime, Action<int, Point> damageFunc)
        {
            foreach (Enemy e in enemylist)
            {
                if ((int)Math.Sqrt(Math.Pow(this.Position.X - e.Position.X, 2) + Math.Pow(this.Position.Y - e.Position.Y, 2)) <= range && (elapsedTime - cooldown) > attackspeed)
                {
                    Blast tempBlast = new Blast(Position, ResourceManager.Blast, e.Position, enemylist, damage, areaofeffect, damageFunc);
                    projectilelist.Add(tempBlast);

                    cooldown = elapsedTime;
                    ResourceManager.BlastSound.Play();
                }
            }
            return projectilelist;
        }

        public override void upgrade()
        {
            damage = damage * 2;
            areaofeffect = areaofeffect + 20;
            Cost = Cost * 2;
        }
    }
}
