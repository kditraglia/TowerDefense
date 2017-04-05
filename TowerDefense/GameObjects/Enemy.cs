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
        string name;
        int HP;
        int maxHP;
        int speed;
        List<Node> bestPath;
        Point currentDest;
        float scale;

        CommandCard commandCard;

        public Enemy(int HP, int speed, Texture2D tex, List<Node> bestPath, string name, float scale) : base(tex, Point.Zero)
        {
            this.name = name;
            this.HP = HP;
            this.maxHP = HP;
            this.speed = speed;
            this.scale = scale;
            this.bestPath = bestPath;
            this.Position = currentDest = bestPath[0].actualPos;

            commandCard = new CommandCard(name, hp: HP.ToString(), speed: speed.ToString());
        }

        public void damage(int damage)
        {
            HP = HP - damage;
            commandCard.HP = HP.ToString();
            ResourceManager.DamagedSound.Play();
        }

        public bool Update(GameTime gameTime)
        {
            if (Position.Y > currentDest.Y)
            {
                Position = new Point(Position.X, Position.Y - speed);
            }
            else if (Position.Y < currentDest.Y)
            {
                Position = new Point(Position.X, Position.Y + speed);
            }
            else if (Position.X > currentDest.X)
            {
                Position = new Point(Position.X - speed, Position.Y);
            }
            else if (Position.X < currentDest.X)
            { 
                Position = new Point(Position.X + speed, Position.Y);
            }
            else
            {
                if (bestPath.Count > 1)
                {
                    Node CurrentNode = bestPath[0];
                    bestPath.Remove(CurrentNode);
                    if (CurrentNode.portal && CurrentNode.portalsTo != null)
                    {
                        Position = CurrentNode.portalsTo.actualPos;
                        ResourceManager.PortalSound.Play();
                    }
                    CurrentNode = bestPath[0];
                    currentDest = CurrentNode.actualPos;
                }
                else
                {
                    GameStats.PlayerLoses = true;
                    MessageLog.GameOver();

                    //Travel behind Africa banner
                    //TODO not this
                    currentDest = new Point(400, 750);
                }
            }

            return HP <= 0;
        }

        public override void ShowStats(SpriteBatch batch, Viewport viewport)
        {
            int Y = (int)(viewport.Height * .2f);
            int X = viewport.Width;

            commandCard.Draw(new Point(X, Y), batch);
        }
    }
}
