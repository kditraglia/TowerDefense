using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense
{
    class CannonTower: Tower
    {
        public string name = "Malaria Net";
        public int damage = 18;
        public float attackspeed = 2.0f;
        public int areaofeffect = 50;
        public int range = 175;
        public string description = "AOE damage";
        public double cooldown = 0;

        public CannonTower(Point position, Texture2D tex) : base (tex, position)
        {
            Cost = 15;
            CommandCard = new CommandCard(name, damage.ToString(), attackspeed.ToString(), range.ToString(), Cost.ToString(), description);
        }

        public override List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, double elapsedTime, Action<int, Point> damageFunc)
        {
            foreach (Enemy e in enemylist)
            {
                if ((int)Math.Sqrt(Math.Pow(this.Position.X - e.Position.X, 2) + Math.Pow(this.Position.Y - e.Position.Y, 2)) <= range && (elapsedTime - cooldown) > attackspeed && e.spawned && !e.dead)
                {
                    projectilelist.Add(new CannonBall(Position, ResourceManager.CannonBall, e.Position, enemylist, damage, areaofeffect, damageFunc));
                    cooldown = elapsedTime;
                    ResourceManager.CannonSound.Play();
                }
            }
            return projectilelist;
        }

        public override void upgrade()
        {
            damage = damage * 2;
            areaofeffect = areaofeffect + 50;
            Cost = Cost * 2;

        }
    }
}
