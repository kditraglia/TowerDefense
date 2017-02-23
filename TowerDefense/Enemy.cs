using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace TowerDefense
{
    class Enemy
    {
        public String name;
        public int HP;
        public int maxHP;
        public int speed;
        public Vector2 pos;
        public Texture2D tex;
        List<Node> bestPath = new List<Node>();
        Node temp;
        public bool spawned = false;
        Vector2 currentDest;
        public bool dead = false;
        public bool hovering = false;
        public Color color = Color.White;
        public bool lose = false;
        public float scale;
        public int spawnRate;
        SoundEffect damaged;
        SoundEffect portal;
        public int ID;


        public Enemy(int HP, int speed, Texture2D tex, Node[,] nodes, String name, float scale, int spawnRate, SoundEffect damaged, SoundEffect portal, int ID)
        {
            this.name = name;
            this.HP = HP;
            this.maxHP = HP;
            this.speed = speed;
            this.scale = scale;
            this.ID = ID;
            this.spawnRate = spawnRate;
            this.bestPath = findBestPath(nodes);
            this.bestPath.Reverse();
            this.tex = tex;
            this.temp = bestPath[0];
            this.pos = temp.actualPos;
            this.bestPath.Remove(temp);
            this.temp = bestPath[0];
            this.damaged = damaged;
            this.portal = portal;
            currentDest = temp.actualPos;
        }
        public void damage(int damage)
        {
            HP = HP - damage;
            damaged.Play();
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
                if (pos.Y > currentDest.Y)
                    pos = new Vector2(pos.X, pos.Y - speed);
                else if (pos.Y < currentDest.Y)
                    pos = new Vector2(pos.X, pos.Y + speed);
                else if (pos.X > currentDest.X)
                    pos = new Vector2(pos.X - speed, pos.Y);
                else if (pos.X < currentDest.X)
                    pos = new Vector2(pos.X + speed, pos.Y);

                else
                {
                    bestPath.Remove(temp);
                    if (bestPath.Count != 0)
                    {
                        if (temp.portal)
                        {
                            pos = temp.portalsTo.actualPos;
                            temp = bestPath[0];
                            bestPath.Remove(temp);
                            temp = bestPath[0];
                            portal.Play();
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


        public void Draw(SpriteBatch batch) //Draw function, same as mousehandler one.
        {
            if (spawned)
                batch.Draw(tex, pos, null, color, 0, new Vector2(0,0), scale, SpriteEffects.None,.5f);
        }

        public List<Node> findBestPath(Node[,] nodes)
        {
            List<Node> available = new List<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            List<Node> temp = new List<Node>();
            List<Node> destinations = new List<Node>();

            for (int i = 0; i < 17; i++)
                for (int j = 0; j < 21; j++)
                {
                    nodes[i, j].parent = null;
                    nodes[i, j].cost = 1000;
                }
            for (int i = 0; i < 17; i++)
            {
                if (!nodes[i, 0].wall)
                {
                    available.Add(nodes[i, 0]);
                    visited.Add(nodes[i, 0]);
                    nodes[i, 0].cost = 0;
                }
            }
            while (available.Count != 0)
            {
                foreach (Node n in available)
                {
                    if ( n.simplePos.Y == 20 )
                    {
                        destinations.Add(n);
                    }

                    if (n.portal)
                    {
                        if (n.portalsTo.cost > n.cost + 1)
                        {
                            temp.Add(n.portalsTo);
                            n.portalsTo.parent = n;
                            n.portalsTo.cost = n.cost + 1;
                        }
                    }
                    if (!n.portal || n.parent.portal)
                    {
                        if (((int)n.simplePos.Y + 1) < 21 && !nodes[(int)n.simplePos.X, (int)n.simplePos.Y + 1].wall)
                        {
                            Node tempNode = nodes[(int)n.simplePos.X, (int)n.simplePos.Y + 1];
                            if (tempNode.cost > n.cost + 1)
                            {
                                temp.Add(tempNode);
                                tempNode.parent = n;
                                tempNode.cost = n.cost + 1;
                            }
                        }
                        if (((int)n.simplePos.Y - 1) >= 0 && !nodes[(int)n.simplePos.X, (int)n.simplePos.Y - 1].wall)
                        {
                            Node tempNode = nodes[(int)n.simplePos.X, (int)n.simplePos.Y - 1];
                            if (tempNode.cost > n.cost + 1)
                            {
                                temp.Add(tempNode);
                                tempNode.parent = n;
                                tempNode.cost = n.cost + 1;
                            }
                        }
                        if (((int)n.simplePos.X + 1) < 17 && !nodes[(int)n.simplePos.X + 1, (int)n.simplePos.Y].wall)
                        {
                            Node tempNode = nodes[(int)n.simplePos.X + 1, (int)n.simplePos.Y];
                            if (tempNode.cost > n.cost + 1)
                            {
                                temp.Add(tempNode);
                                tempNode.parent = n;
                                tempNode.cost = n.cost + 1;
                            }
                        }
                        if (((int)n.simplePos.X - 1) >= 0 && !nodes[(int)n.simplePos.X - 1, (int)n.simplePos.Y].wall)
                        {
                            Node tempNode = nodes[(int)n.simplePos.X - 1, (int)n.simplePos.Y];
                            if (tempNode.cost > n.cost + 1)
                            {
                                temp.Add(tempNode);
                                tempNode.parent = n;
                                tempNode.cost = n.cost + 1;
                            }
                        }
                    }
                    visited.Add(n);
                }
                available.AddRange(temp);
                available.RemoveAll(a=>visited.Contains(a));
                
                temp.Clear();
            }

            List<Node> bestPath = new List<Node>();
            int bestCost = 10000000;
            Node bestDest = null;
            foreach (Node n in destinations)
            {
                if (n.cost < bestCost)
                {
                    bestCost = n.cost;
                    bestDest = n;
                }

            }
            bestPath.Add(bestDest);
            while (bestDest.parent != null)
            {
                bestPath.Add(bestDest.parent);
                bestDest = bestDest.parent;
            }
            return bestPath;
        }
    }
}
