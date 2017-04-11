using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TowerDefense
{
    enum VampireAnim
    {
        Down = 0,
        Left = 1,
        Right = 2,
        Up = 3
    }
    class Enemy : GameObject
    {
        string name;
        int HP;
        int maxHP;
        int speed;
        List<Node> bestPath;
        Point currentDest;
        VampireAnim anim = VampireAnim.Down;

        CommandCard commandCard;
        Color color = Color.White;

        public override Color Color
        {
            get
            {
                return color;
            }
        }

        public Enemy(int HP, int speed, Texture2D tex, List<Node> bestPath, string name) : base(tex, Point.Zero)
        {
            this.name = name;
            this.HP = HP;
            this.maxHP = HP;
            this.speed = speed;
            this.bestPath = bestPath;
            this.Position = currentDest = bestPath[0].actualPos;

            commandCard = new CommandCard(name, hp: HP.ToString(), speed: speed.ToString());
            SpriteHeight = 20;
            SpriteWidth = 18;
            currentFrame = 0;
            frameCount = 2;
    }

        public void damage(int damage)
        {
            HP = HP - damage;
            commandCard.HP = HP.ToString();
            ResourceManager.DamagedSound.Play();
        }

        public override bool Update(GameTime gameTime)
        {
            if (HP <= 0)
            {
                color.R -= 15;
                color.G -= 15;
                color.B -= 15;
                return color == Color.Black;
            }

            if (Position != currentDest)
            {
                Point diff = currentDest - Position;

                // Prefer vertical movement, eliminate diagonal movement
                Position += new Point(diff.Y == 0 ? speed * Math.Sign(diff.X) : 0, speed * Math.Sign(diff.Y));
                anim = diff.Y == 0 ? diff.X < 0 ? VampireAnim.Left : VampireAnim.Right : diff.Y < 0 ? VampireAnim.Up : VampireAnim.Down;
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

                    //TODO not this
                    currentDest = new Point(400, 736);
                }
            }

            return base.Update(gameTime);
        }

        public override void ShowStats(SpriteBatch batch)
        {
            int Y = (int)(Constants.GameSize.Y * .2f);
            int X = Constants.GameSize.X;

            commandCard.Draw(new Point(X, Y), batch);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(ResourceManager.Block, new Rectangle(Position.X - 2, Position.Y - 7, SpriteWidth * 2 + 4, 8), Color.Black);
            float HPPercent = (float)HP / maxHP;
            batch.Draw(ResourceManager.Block, new Rectangle(Position.X, Position.Y - 5, (int)(HPPercent * SpriteWidth * 2), 4), new Color(1.0f - HPPercent, HPPercent, 0));
            batch.Draw(Tex, Position.ToVector2(), new Rectangle(new Point(currentFrame * SpriteWidth, (int)anim * SpriteHeight), new Point(SpriteWidth, SpriteHeight)), Color, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
        }
    }
}
