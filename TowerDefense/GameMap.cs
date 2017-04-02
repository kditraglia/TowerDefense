using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{

    internal enum CheckForPathType
    {
        TogglingWall, TogglingCheese, AddingPortal, RemovingPortal
    }

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

        internal List<Node> GetBestPath()
        {
            return PathFinding.findBestPath(nodes);
        }

        internal bool CheckForPath(int x, int y, MouseHandler mouse, CheckForPathType type)
        {
            Node portaledTo = nodes[x, y].portalsTo;

            switch (type)
            {
                case CheckForPathType.AddingPortal:
                    nodes[x, y].portal = true;
                    nodes[x, y].portalsTo = mouse.PortalEntrance.Clone();
                    mouse.PortalEntrance.portalsTo = nodes[x, y].Clone();
                    break;
                case CheckForPathType.RemovingPortal:
                    nodes[x, y].portal = false;
                    nodes[x, y].portalsTo = null;
                    portaledTo.portalsTo = null;
                    portaledTo.portal = false;
                    break;
                case CheckForPathType.TogglingCheese:
                    nodes[x, y].cheese = !nodes[x, y].cheese;
                    break;
                default:
                    nodes[x, y].wall = !nodes[x, y].wall;
                    break;
            }

            List<Node> bestPath = PathFinding.findBestPath(nodes);

            switch (type)
            {
                case CheckForPathType.AddingPortal:
                    nodes[x, y].portal = false;
                    nodes[x, y].portalsTo = null;
                    mouse.PortalEntrance.portalsTo = null;
                    break;
                case CheckForPathType.RemovingPortal:
                    nodes[x, y].portal = true;
                    nodes[x, y].portalsTo = portaledTo;
                    portaledTo.portalsTo = nodes[x, y].Clone();
                    portaledTo.portal = true;
                    break;
                case CheckForPathType.TogglingCheese:
                    nodes[x, y].cheese = !nodes[x, y].cheese;
                    break;
                default:
                    nodes[x, y].wall = !nodes[x, y].wall;
                    break;
            }

            return bestPath != null;
        }
    }
}
