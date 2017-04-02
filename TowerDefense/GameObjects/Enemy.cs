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
        List<Node> bestPath;
        Node temp;
        public bool spawned = false;
        Point currentDest;
        public bool dead = false;
        public bool lose = false;
        public float scale;
        public double spawnRate;

        CommandCard commandCard;

        public Enemy(int HP, int speed, Texture2D tex, List<Node> bestPath, String name, float scale, double spawnRate) : base(tex, Point.Zero)
        {
            this.name = name;
            this.HP = HP;
            this.maxHP = HP;
            this.speed = speed;
            this.scale = scale;
            this.spawnRate = spawnRate;
            this.bestPath = bestPath;

            this.temp = bestPath[0];
            this.Position = temp.actualPos;
            this.bestPath.Remove(temp);
            this.temp = bestPath[0];
            currentDest = temp.actualPos;

            commandCard = new CommandCard(name, hp: HP.ToString(), speed: speed.ToString());
        }
        public void damage(int damage)
        {
            HP = HP - damage;
            commandCard.HP = HP.ToString();
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
                        if (temp.portal && temp.portalsTo != null)
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
        public override void ShowStats(SpriteBatch batch, Viewport viewport)
        {
            int Y = (int)(viewport.Height * .2f);
            int X = viewport.Width;

            commandCard.Draw(new Point(X, Y), batch);
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
