using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace TowerDefense
{
    [DebuggerDisplay("{simplePos}")]
    class Node : GameObject, ICloneable
    {
        public Point actualPos;
        public Point simplePos;
        public Texture2D tex2;
        public int gScore;
        public int fScore;
        public Node parent;
        public bool wall = false;
        public bool portal = false;
        public bool cheese = false;
        public Node portalsTo = null;

        public override Color Color
        {
            get
            {
                if (portalsTo != null && portalsTo.Hovering)
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
        }
        public override void Draw(SpriteBatch batch) 
        {
            base.Draw(batch);
            if ( tex2 != null )
                batch.Draw(tex2, actualPos.ToVector2(), null, Color);
        }
        public void UpdateTex(Texture2D tex)
        {
            this.tex2 = tex;
        }
        public void defaultSet()
        {
            this.tex2 = null;
        }
        public List<Node> getNeighbors(Node[,] nodes)
        {
            List<Node> neighbors = new List<Node>();
            if (portal && (parent != null && !parent.portal))
            {
                neighbors.Add(portalsTo);
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

        public override void ShowStats(SpriteBatch batch, SpriteFont font, Viewport viewport)
        {
            if (wall)
            {
                String[] string1 = new String[6];
                int[] stringlength1 = new int[6];
                int[] stringlength2 = new int[6];
                int Y = (int)(viewport.Height * .2f);
                string1[0] = "Wall";
                string1[1] = "damage - 0";
                string1[2] = "attack speed - N/A";
                string1[3] = "range - N/A";
                string1[4] = "cost - 1";
                string1[5] = "description - Blocks enemies";

                for (int i = 0; i < 6; i++)
                {
                    stringlength1[i] = (int)font.MeasureString(string1[i]).X + 10;
                    stringlength2[i] = (int)font.MeasureString(string1[i]).Y + 10;
                    Y = Y + stringlength2[i];
                    batch.DrawString(font, string1[i], new Vector2(viewport.Width - stringlength1[i], Y), Color.Black,
                        0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                }
            }
            else if (portal)
            {
                String[] string1 = new String[6];
                int[] stringlength1 = new int[6];
                int[] stringlength2 = new int[6];
                int Y = (int)(viewport.Height * .2f);
                string1[0] = "Portal";
                string1[1] = "damage - 0";
                string1[2] = "attack speed - N/A";
                string1[3] = "range - N/A";
                string1[4] = "cost - 20";
                string1[5] = "description - Warps enemies";

                for (int i = 0; i < 6; i++)
                {
                    stringlength1[i] = (int)font.MeasureString(string1[i]).X + 10;
                    stringlength2[i] = (int)font.MeasureString(string1[i]).Y + 10;
                    Y = Y + stringlength2[i];
                    batch.DrawString(font, string1[i], new Vector2(viewport.Width - stringlength1[i], Y), Color.Black,
                        0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                }
            }
            else if (cheese)
            {
                String[] string1 = new String[6];
                int[] stringlength1 = new int[6];
                int[] stringlength2 = new int[6];
                int Y = (int)(viewport.Height * .2f);
                string1[0] = "Cheese";
                string1[1] = "damage - 0";
                string1[2] = "attack speed - N/A";
                string1[3] = "range - N/A";
                string1[4] = "cost - 20";
                string1[5] = "description - Irresistible to enemies";

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

        public object Clone()
        {
            Node clone = new Node(actualPos, simplePos, Tex);
            clone.wall = wall;
            clone.portal = portal;
            clone.cheese = cheese;
            clone.portalsTo = portalsTo;

            return clone;
        }
    }
}
