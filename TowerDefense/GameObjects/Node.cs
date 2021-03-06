﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace TowerDefense
{
    [DebuggerDisplay("{simplePos}")]
    class Node : GameObject
    {
        public Point actualPos;
        public Point simplePos;
        public Texture2D tex2;
        public bool wall = false;
        public bool portal = false;
        public bool cheese = false;
        public Node portalsTo = null;

        CommandCard emptyCard = new CommandCard("Grass", description: "Empty square");
        CommandCard wallCard = new CommandCard("Wall", cost: "1", description: "Blocks enemies");
        CommandCard portalCard = new CommandCard("Portal", cost: "20", description: "Warps enemies");
        CommandCard cheeseCard = new CommandCard("Cheese", cost: "20", description: "Irresistible to enemies");

        public bool IsEmpty { get { return !wall && !portal && !cheese; } }
        public override Color Color
        {
            get
            {
                if (portalsTo != null && portalsTo.Selected)
                {
                    return Color.Green;
                }
                return base.Color;
            }
        }

        public Node(Point actualPos, Point simplePos, Texture2D tex) : base(tex, actualPos)
        {
            this.actualPos = actualPos;
            this.simplePos = simplePos;

            SpriteHeight = 32;
            SpriteWidth = 32;
            currentFrame = 0;
        }

        public override bool Update(GameTime gameTime, InputHandler inputHandler)
        {
            frameCount = portal ? 4 : 0;
            currentFrame = portal ? currentFrame : 0;

            return base.Update(gameTime, inputHandler);
        }

        public override void Draw(SpriteBatch batch) 
        {
            base.Draw(batch);
            if (tex2 != null)
            {
                batch.Draw(tex2, Position.ToVector2(), new Rectangle(new Point(currentFrame * SpriteWidth, 0), new Point(SpriteWidth, SpriteHeight)), Color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }
        internal void Sell()
        {
            GameStats.Gold = GameStats.Gold + (wall ? 1 : 20);
            wall = false;
            portal = false;
            portalsTo?.defaultSet();
            portalsTo = null;
            cheese = false;
            defaultSet();
            ResourceManager.SellSound.Play();
        }
        public void UpdateTex(Texture2D tex)
        {
            this.tex2 = tex;
        }
        public void defaultSet()
        {
            this.tex2 = null;
        }
        public List<Node> getNeighbors(Node[,] nodes, Node parent)
        {
            List<Node> neighbors = new List<Node>();
            if (portal && portalsTo != null)
            {
                neighbors.Add(portalsTo.Clone());
            }
            else
            {
                if ((simplePos.Y + 1) <= Constants.MapSize.Y && !nodes[simplePos.X, simplePos.Y + 1].wall)
                {
                    Node tempNode = nodes[simplePos.X, simplePos.Y + 1];
                    neighbors.Add(tempNode);
                }
                if ((simplePos.Y - 1) >= 0 && !nodes[simplePos.X, simplePos.Y - 1].wall)
                {
                    Node tempNode = nodes[simplePos.X, simplePos.Y - 1];
                    neighbors.Add(tempNode);
                }
                if ((simplePos.X + 1) <= Constants.MapSize.X && !nodes[simplePos.X + 1, simplePos.Y].wall)
                {
                    Node tempNode = nodes[simplePos.X + 1, simplePos.Y];
                    neighbors.Add(tempNode);
                }
                if ((simplePos.X - 1) >= 0 && !nodes[simplePos.X - 1, simplePos.Y].wall)
                {
                    Node tempNode = nodes[simplePos.X - 1, simplePos.Y];
                    neighbors.Add(tempNode);
                }
            }

            return neighbors;
        }

        public override void ShowStats(SpriteBatch batch)
        {
            int Y = (int)(Constants.GameSize.Y * .2f);
            int X = Constants.GameSize.X;

            if (wall)
            {
                wallCard.Draw(new Point(X, Y), batch);
            }
            else if (portal)
            {
                portalCard.Draw(new Point(X, Y), batch);
            }
            else if (cheese)
            {
                cheeseCard.Draw(new Point(X, Y), batch);
            }
            else
            {
                emptyCard.Draw(new Point(X, Y), batch);
            }
        }

        internal Node Clone()
        {
            Node clone = new Node(actualPos, simplePos, Tex);
            clone.wall = wall;
            clone.portal = portal;
            clone.cheese = cheese;

            return clone;
        }
    }
}
