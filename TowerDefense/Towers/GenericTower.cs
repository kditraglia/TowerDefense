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
        public String name = "Immunization Tower";
        public int damage = 20;
        public float attackspeed = 1.0f;
        public int range = 200;
        public String description = "Hits 1 Target";
        public double cooldown = 0;

        SoundEffect attackSound;

        public GenericTower(Vector2 position, Texture2D tex, Texture2D proj, SoundEffect attackSound) : base(tex, position)
        {
            this.tex = tex;
            this.position = position;
            this.proj = proj;
            this.attackSound = attackSound;

            cost = 12;
        }

        public override List<Projectile> Attack(List<Enemy> enemylist, List<Projectile> projectilelist, double elapsedTime)
        {
            foreach (Enemy e in enemylist)
            {
                if ((int)Math.Sqrt(Math.Pow(this.position.X - e.position.X, 2) + Math.Pow(this.position.Y - e.position.Y, 2)) <= range && (elapsedTime - cooldown) > attackspeed && e.spawned && !e.dead )
                {
                    projectilelist.Add(new Bullet(position, proj, e, damage));
                    cooldown = elapsedTime;
                    attackSound.Play();
                }
            }
            return projectilelist;

        }

        public override void upgrade()
        {
            damage = damage * 2;
            range = range + 50;
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
    }
}
