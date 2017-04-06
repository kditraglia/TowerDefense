using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TowerDefense
{
    class GenericTower: Tower
    {
        public string name = "Immunization Tower";
        public int damage = 20;
        public float attackspeed = 1.0f;
        public int range = 200;
        public string description = "Hits 1 Target";
        public double cooldown = 0;

        public GenericTower(Point position, Texture2D tex) : base(tex, position)
        {
            Cost = 10;
            CommandCard = new CommandCard(name, damage.ToString(), attackspeed.ToString(), range.ToString(), Cost.ToString(), description);
        }

        public override List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, double elapsedTime, Action<int, Point> damageFunc)
        {
            foreach (Enemy e in enemylist)
            {
                if ((int)Math.Sqrt(Math.Pow(this.Position.X - e.Position.X, 2) + Math.Pow(this.Position.Y - e.Position.Y, 2)) <= range && (elapsedTime - cooldown) > attackspeed )
                {
                    projectilelist.Add(new Bullet(Position, ResourceManager.Bullet, e, damage, damageFunc));
                    cooldown = elapsedTime;
                    ResourceManager.BulletSound.Play();
                }
            }
            return projectilelist;

        }

        public override void upgrade()
        {
            damage = damage * 2;
            range = range + 50;
            Cost = Cost * 2;
        }
    }
}
