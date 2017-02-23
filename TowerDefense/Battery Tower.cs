using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TowerDefense
{
    class Battery_Tower: Tower
    {
        public String name = "Bug Zapper";
        public int damage = 5;
        public float attackspeed = 0.75f;
        public int range = 100;
        public String description = "multiple targets";
        public double cooldown = 0;
        public int cost = 15;
        SoundEffect attackSound;

        public Battery_Tower(Vector2 position, Texture2D tex, Texture2D proj, int ID, SoundEffect attackSound)
        {
            this.tex = tex;
            this.position = position;
            this.proj = proj;
            this.ID = ID;
            this.attackSound = attackSound;
        }

        public override List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, int elapsedTime)
        {
            if ((elapsedTime - cooldown) > (attackspeed * 100))
            {
                foreach (Enemy e in enemylist)
                {
                    if ((int)Math.Sqrt(Math.Pow(this.position.X - e.pos.X, 2) + Math.Pow(this.position.Y - e.pos.Y, 2)) <= range && e.spawned && !e.dead)
                    {
                        projectilelist.Add(new Bullet(position, proj, e, damage));
                        cooldown = elapsedTime;
                        attackSound.Play();
                    }
                }
            }
            return projectilelist;
        }

        public override void upgrade()
        {
            damage = damage * 2;
            attackspeed = attackspeed / 1.25f;
            cost = cost * 2;
        }

        public override void ShowStats(SpriteBatch batch, SpriteFont font, Viewport viewport)
        {
            String[] string1 = new String[6];
            int[] stringlength1 = new int[6];
            int[] stringlength2 = new int[6];
            int Y = (int)(viewport.Height * .2f);
            string1[0] = name;
            string1[1] = "damage - " + damage;
            string1[2] = "attack speed - " + attackspeed;
            string1[3] = "range - " + range;
            string1[4] = "cost - " + cost;
            string1[5] = "description - " + description;

            for (int i = 0; i < 6; i++)
            {
                stringlength1[i] = (int)font.MeasureString(string1[i]).X + 10;
                stringlength2[i] = (int)font.MeasureString(string1[i]).Y + 10;
                Y = Y + stringlength2[i];
                batch.DrawString(font, string1[i], new Vector2(viewport.Width - stringlength1[i], Y), Color.Black,
                    0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }
        }
        public override int getCost()
        {
            return cost;
        }
 

    }
}
