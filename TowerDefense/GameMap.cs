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

        internal void Update(GameTime gameTime, InputHandler inputHandler)
        {
            for (int i = 0; i <= Constants.MapSize.X; i++)
            {
                for (int j = 0; j <= Constants.MapSize.Y; j++)
                {
                    Node n = nodes[i, j];
                    n.Update(gameTime, inputHandler);
                    if (n.BoundingBox().Contains(inputHandler.Position) && inputHandler.SelectionOccurring)
                    {
                        HandleInput(inputHandler, n);
                    }
                }
            }
        }

        private void HandleInput(InputHandler inputHandler, Node n)
        {
            if (!GameStats.AttackPhase)
            {
                if (!n.IsEmpty && inputHandler.SelectionContext == SelectionContext.None)
                {
                    inputHandler.SelectionContext = SelectionContext.NodeSelected;
                    inputHandler.SelectedObject = n;
                    n.Selected = true;
                    return;
                }
                if (!n.IsEmpty)
                {
                    return;
                }
                if (inputHandler.SelectionContext == SelectionContext.PlacingWall)
                {
                    if (!CheckForPath(n.simplePos.X, n.simplePos.Y, inputHandler, CheckForPathType.TogglingWall))
                    {
                        MessageLog.IllegalPosition();
                    }
                    else if (GameStats.Gold >= 1)
                    {
                        n.wall = true;
                        n.UpdateTex(ResourceManager.Wall);
                        GameStats.Gold = GameStats.Gold - 1;
                        ResourceManager.WallSound.Play();
                        inputHandler.SelectionHandled = true;
                    }
                    else
                    {
                        MessageLog.NotEnoughGold();
                    }
                }
                else if (inputHandler.SelectionContext == SelectionContext.PlacingPortalEntrance)
                {
                    n.portal = true;
                    n.UpdateTex(ResourceManager.Portal);
                    inputHandler.PortalEntrance = n;
                    inputHandler.SelectionContext = SelectionContext.PlacingPortalExit;
                }
                else if (inputHandler.SelectionContext == SelectionContext.PlacingPortalExit)
                {
                    Node portalExit = n;
                    Node portalEntrance = inputHandler.PortalEntrance;
                    if (!CheckForPath(portalExit.simplePos.X, portalExit.simplePos.Y, inputHandler, CheckForPathType.AddingPortal))
                    {
                        MessageLog.IllegalPosition();
                    }
                    else if (GameStats.Gold >= 20)
                    {
                        portalExit.portal = true;
                        portalExit.UpdateTex(ResourceManager.Portal);
                        portalExit.portalsTo = portalEntrance;
                        portalEntrance.portalsTo = portalExit;
                        inputHandler.SelectionContext = SelectionContext.PlacingPortalEntrance;
                        GameStats.Gold = GameStats.Gold - 20;
                    }
                    else
                    {
                        MessageLog.NotEnoughGold();
                    }
                }
                else if (inputHandler.SelectionContext == SelectionContext.PlacingCheese)
                {
                    if (!CheckForPath(n.simplePos.X, n.simplePos.Y, inputHandler, CheckForPathType.TogglingCheese))
                    {
                        MessageLog.IllegalPosition();
                    }
                    else if (GameStats.Gold >= 20)
                    {
                        n.cheese = true;
                        n.UpdateTex(ResourceManager.Cheese);
                        GameStats.Gold = GameStats.Gold - 20;
                        ResourceManager.WallSound.Play();
                    }
                    else
                    {
                        MessageLog.NotEnoughGold();
                    }
                }
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

        internal List<Node> GetBestPath()
        {
            return PathFinding.findBestPath(nodes);
        }

        internal bool CheckForPath(int x, int y, InputHandler mouse, CheckForPathType type)
        {
            Node portaledTo = nodes[x, y].portalsTo;

            switch (type)
            {
                case CheckForPathType.AddingPortal:
                    nodes[x, y].portal = true;
                    nodes[x, y].portalsTo = mouse.PortalEntrance;
                    mouse.PortalEntrance.portalsTo = nodes[x, y];
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
                    portaledTo.portalsTo = nodes[x, y];
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
