using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense
{
    class BatteryTower: Tower
    {
        public string name = "Bug Zapper";
        public int damage = 5;
        public float attackspeed = 1.0f;
        public int range = 100;
        public string description = "Multiple targets";
        public double cooldown = 0;

        public BatteryTower(Point position, Texture2D tex) : base(tex, position)
        {
            Cost = 15;
            CommandCard = new CommandCard(name, damage.ToString(), attackspeed.ToString(), range.ToString(), Cost.ToString(), description);
        }

        public override List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, double elapsedTime, Action<int, Point> damageFunc)
        {
            if ((elapsedTime - cooldown) > attackspeed)
            {
                bool attacked = false;
                foreach (Enemy e in enemylist)
                {
                    if ((int)Math.Sqrt(Math.Pow(this.Position.X - e.Position.X, 2) + Math.Pow(this.Position.Y - e.Position.Y, 2)) <= range)
                    {
                        projectilelist.Add(new Bullet(Position, ResourceManager.LightningBolt, e, damage, damageFunc));
                        cooldown = elapsedTime;
                        attacked = true;
                    }
                }
                if (attacked)
                {
                    ResourceManager.LightningSound.Play();
                }
            }
            return projectilelist;
        }

        public override void upgrade()
        {
            damage = damage * 2;
            attackspeed = attackspeed / 1.25f;
            Cost = Cost * 2;
        }
    }
}
