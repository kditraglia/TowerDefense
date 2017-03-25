using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TowerDefense
{
    class Enemy : GameObject
    {
        public string name;
        public int HP;
        public int maxHP;
        public int speed;
        List<Node> bestPath = new List<Node>();
        Node temp;
        public bool spawned = false;
        Vector2 currentDest;
        public bool dead = false;
        public bool hovering = false;
        public bool lose = false;
        public float scale;
        public double spawnRate;

        public Enemy(int HP, int speed, Texture2D tex, Node[,] nodes, String name, float scale, double spawnRate) : base(tex, Vector2.Zero)
        {
            this.name = name;
            this.HP = HP;
            this.maxHP = HP;
            this.speed = speed;
            this.scale = scale;
            this.spawnRate = spawnRate;
            this.bestPath = TowerDefense.findBestPath(nodes);
            this.bestPath.Reverse();
            this.temp = bestPath[0];
            this.position = temp.actualPos;
            this.bestPath.Remove(temp);
            this.temp = bestPath[0];
            currentDest = temp.actualPos;
        }
        public void damage(int damage)
        {
            HP = HP - damage;
            ResourceManager.DamagedSound.Play();
            if ( HP <= 0 )
                dead = true;
        }
        public void spawn()
        {
            spawned = true;
        }
        public void move()
        {
            if (spawned)
            {
                if (position.Y > currentDest.Y)
                    position = new Vector2(position.X, position.Y - speed);
                else if (position.Y < currentDest.Y)
                    position = new Vector2(position.X, position.Y + speed);
                else if (position.X > currentDest.X)
                    position = new Vector2(position.X - speed, position.Y);
                else if (position.X < currentDest.X)
                    position = new Vector2(position.X + speed, position.Y);

                else
                {
                    bestPath.Remove(temp);
                    if (bestPath.Count != 0)
                    {
                        if (temp.portal)
                        {
                            position = temp.portalsTo.actualPos;
                            temp = bestPath[0];
                            bestPath.Remove(temp);
                            temp = bestPath[0];
                            ResourceManager.PortalSound.Play();
                        }
                        else
                            temp = bestPath[0];
                        currentDest = temp.actualPos;
                    }
                    else
                    {
                        currentDest = new Vector2(350, 750);
                        lose = true;
                    }
                }
            }
        }
        public void ShowStats(SpriteBatch batch, SpriteFont font, Viewport viewport)
        {
            String[] string1 = new String[3];
            int[] stringlength1 = new int[3];
            int[] stringlength2 = new int[3];
            int Y = (int)(viewport.Height * .2f);
            string1[0] = name;
            string1[1] = "HP - " + HP + "/" + maxHP;
            string1[2] = "speed - " + speed;

            for (int i = 0; i < 3; i++)
            {
                stringlength1[i] = (int)font.MeasureString(string1[i]).X + 10;
                stringlength2[i] = (int)font.MeasureString(string1[i]).Y + 10;
                Y = Y + stringlength2[i];
                batch.DrawString(font, string1[i], new Vector2(viewport.Width - stringlength1[i], Y), Color.Black,
                    0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }
        }


        public override void Draw(SpriteBatch batch)
        {
            if (spawned)
            {
                base.Draw(batch);
            }
        }
    }
}
