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
    class Node : GameObject
    {
        public Vector2 actualPos;
        public Vector2 simplePos;
        public Texture2D tex2;
        public int gScore;
        public int fScore;
        public Node parent;
        public bool wall = false;
        public bool portal = false;
        public Node portalsTo = null;
        public Node(Vector2 actualPos, Vector2 simplePos, Texture2D tex) : base(tex, actualPos)
        {
            this.actualPos = actualPos;
            this.simplePos = simplePos;
        }
        public override void Draw(SpriteBatch batch) 
        {
            base.Draw(batch);
            if ( tex2 != null )
                batch.Draw(tex2, actualPos, null, Color);
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
            if (portal && !parent.portal)
            {
                neighbors.Add(portalsTo);
            }
            else
            {
                if (((int)simplePos.Y + 1) <= Constants.MapSize.Y && !nodes[(int)simplePos.X, (int)simplePos.Y + 1].wall)
                {
                    Node tempNode = nodes[(int)simplePos.X, (int)simplePos.Y + 1];
                    neighbors.Add(tempNode);
                }
                if (((int)simplePos.Y - 1) >= 0 && !nodes[(int)simplePos.X, (int)simplePos.Y - 1].wall)
                {
                    Node tempNode = nodes[(int)simplePos.X, (int)simplePos.Y - 1];
                    neighbors.Add(tempNode);
                }
                if (((int)simplePos.X + 1) <= Constants.MapSize.X && !nodes[(int)simplePos.X + 1, (int)simplePos.Y].wall)
                {
                    Node tempNode = nodes[(int)simplePos.X + 1, (int)simplePos.Y];
                    neighbors.Add(tempNode);
                }
                if (((int)simplePos.X - 1) >= 0 && !nodes[(int)simplePos.X - 1, (int)simplePos.Y].wall)
                {
                    Node tempNode = nodes[(int)simplePos.X - 1, (int)simplePos.Y];
                    neighbors.Add(tempNode);
                }
            }

            return neighbors;
        }
    }
}
