using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    class GameMap
    {
        Node[,] nodes = new Node[Constants.MapSize.X + 1, Constants.MapSize.Y + 1];

        internal GameMap()
        {
            CreateMap();
        }

        private void CreateMap()
        {
            int actualY = Constants.MapStart.Y;
            int actualX = Constants.MapStart.X;
            for (int i = 0; i <= Constants.MapSize.X; i++)
            {
                for (int j = 0; j <= Constants.MapSize.Y; j++)
                {
                    nodes[i, j] = new Node(new Point(actualX, actualY), new Point(i, j), ResourceManager.Grass);
                    actualY += Constants.NodeSize.Y;
                }
                actualY = Constants.MapStart.Y;
                actualX += Constants.NodeSize.X;
            }
        }

        internal void Draw(SpriteBatch batch)
        {
            for (int i = 0; i <= Constants.MapSize.X; i++)
            {
                for (int j = 0; j <= Constants.MapSize.Y; j++)
                {
                    nodes[i, j].Draw(batch);
                }
            }
        }

        internal void HandleMouseHover(MouseHandler mouse)
        {
            if (!GameStats.AttackPhase)
            {
                foreach (Node n in nodes)
                {
                    n.Hovering = n.BoundingBox().Contains(mouse.pos) && mouse.SelectionContext != SelectionContext.PlacingTower;
                    if (n.Hovering)
                    {
                        mouse.HoveredObject = n;
                        mouse.HoveringContext = n.wall || n.portal || n.cheese ? HoveringContext.FilledNode : HoveringContext.EmptyNode;
                    }
                }
            }
        }
    }
}
