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
        Point currentDest;
        public bool dead = false;
        public bool lose = false;
        public float scale;
        public double spawnRate;

        public Enemy(int HP, int speed, Texture2D tex, Node[,] nodes, String name, float scale, double spawnRate) : base(tex, Point.Zero)
        {
            this.name = name;
            this.HP = HP;
            this.maxHP = HP;
            this.speed = speed;
            this.scale = scale;
            this.spawnRate = spawnRate;
            this.bestPath = TowerDefense.findBestPath(nodes);

            this.temp = bestPath[0];
            this.Position = temp.actualPos;
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
                if (Position.Y > currentDest.Y)
                    Position = new Point(Position.X, Position.Y - speed);
                else if (Position.Y < currentDest.Y)
                    Position = new Point(Position.X, Position.Y + speed);
                else if (Position.X > currentDest.X)
                    Position = new Point(Position.X - speed, Position.Y);
                else if (Position.X < currentDest.X)
                    Position = new Point(Position.X + speed, Position.Y);

                else
                {
                    bestPath.Remove(temp);
                    if (bestPath.Count != 0)
                    {
                        if (temp.portal)
                        {
                            Position = temp.portalsTo.actualPos;
                            temp = bestPath[0];
                            bestPath.Remove(temp);
                            if (bestPath.Count != 0)
                            {
                                temp = bestPath[0];
                            }
                            else
                            {
                                currentDest = new Point(350, 750);
                                lose = true;
                            }
                            ResourceManager.PortalSound.Play();
                        }
                        else
                        {
                            temp = bestPath[0];
                        }
                        currentDest = temp.actualPos;
                    }
                    else
                    {
                        currentDest = new Point(350, 750);
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
