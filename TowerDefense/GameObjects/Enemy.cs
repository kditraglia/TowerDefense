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

        int spriteHeight = 20;
        int spriteWidth = 18;
        int currentFrame = 0;
        int frameCount = 2;
        int frameTotalDuration = 200;
        int frameDuration = 0;
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
        }

        public void damage(int damage)
        {
            HP = HP - damage;
            commandCard.HP = HP.ToString();
            ResourceManager.DamagedSound.Play();
        }

        public bool Update(GameTime gameTime)
        {
            if (HP <= 0)
            {
                color.R -= 15;
                color.G -= 15;
                color.B -= 15;
                return color == Color.Black;
            }

            frameDuration += gameTime.ElapsedGameTime.Milliseconds;
            if (frameDuration > frameTotalDuration)
            {
                frameDuration = 0;
                currentFrame++;
                currentFrame %= frameCount;
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

                    //Travel behind Africa banner
                    //TODO not this
                    currentDest = new Point(400, 736);
                }
            }

            return false;
        }

        public override void ShowStats(SpriteBatch batch, Viewport viewport)
        {
            int Y = (int)(viewport.Height * .2f);
            int X = viewport.Width;

            commandCard.Draw(new Point(X, Y), batch);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(ResourceManager.Block, new Rectangle(Position.X - 2, Position.Y - 7, spriteWidth * 2 + 4, 8), Color.Black);
            float HPPercent = (float)HP / maxHP;
            batch.Draw(ResourceManager.Block, new Rectangle(Position.X, Position.Y - 5, (int)(HPPercent * spriteWidth * 2), 4), new Color(1.0f - HPPercent, HPPercent, 0));
            batch.Draw(Tex, Position.ToVector2(), new Rectangle(new Point(currentFrame * spriteWidth, (int)anim * spriteHeight), new Point(spriteWidth, spriteHeight)), Color, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
        }
    }
}
