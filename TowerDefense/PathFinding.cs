using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public static class PathFinding
    {
        private static int heuristic(Node current)
        {
            return Constants.MapSize.Y - current.simplePos.Y;
        }

        /// <summary>
        /// Gets the best path to get to the bottom of the map starting from the top.
        /// </summary>
        /// <param name="nodes">The map nodes.</param>
        /// <returns>Best path of travel to get to the bottom of the map.</returns>
        internal static List<Node> findBestPath(Node[,] nodes)
        {
            int numberOfCheese = 0;

            foreach (Node n in nodes)
            {
                numberOfCheese += n.cheese ? 1 : 0;
            }

            List<Node> startNodes = new List<Node>();
            List<Node> bestPathSoFar = new List<Node>();
            Node[,] nodesClone = cloneNodes(nodes);

            for (int c = numberOfCheese; c >= 0; c--)
            {
                if (nodesClone == null)
                {
                    nodesClone = cloneNodes(nodes);
                    for (int i = 0; i <= Constants.MapSize.X; i++)
                    {
                        if (!nodesClone[i, 0].wall)
                        {
                            startNodes.Add(nodesClone[i, 0]);
                            nodesClone[i, 0].fScore = 0;
                        }
                    }
                }
                else
                {
                    nodesClone = cloneNodes(nodesClone);
                }
                for (int i = 0; i <= Constants.MapSize.X; i++)
                {
                    for (int j = 0; j <= Constants.MapSize.Y; j++)
                    {
                        nodesClone[i, j].parent = null;
                        nodesClone[i, j].gScore = int.MaxValue;
                        nodesClone[i, j].fScore = int.MaxValue;
                    }
                }
                List<Node> bestPathRelay = findBestPath(nodesClone, startNodes, c);
                if (bestPathRelay == null)
                {
                    return null;
                }
                bestPathRelay.Reverse();
                bestPathSoFar.AddRange(bestPathRelay);

                startNodes.Clear();
                startNodes.Add(bestPathSoFar.Last());
                startNodes.ForEach(s => s.cheese = false);
                startNodes.First().gScore = 0;
            }

            return bestPathSoFar;
        }

        private static Node[,] cloneNodes(Node[,] nodes)
        {
            Node[,] nodesClone = new Node[Constants.MapSize.X + 1, Constants.MapSize.Y + 1];
            for (int i = 0; i <= Constants.MapSize.X; i++)
            {
                for (int j = 0; j <= Constants.MapSize.Y; j++)
                {
                    nodesClone[i, j] = (Node)nodes[i, j].Clone();
                }
            }

            return nodesClone;
        }

        internal static List<Node> findBestPath(Node[,] nodes, List<Node> startNodes, int numberOfCheese)
        {
            List<Node> available = new List<Node>(startNodes);
            HashSet<Node> visited = new HashSet<Node>();

            while (available.Count != 0)
            {
                Node current = available.OrderBy(n => n.fScore).First();
                if ((numberOfCheese > 0 && current.cheese) || (numberOfCheese == 0 && current.simplePos.Y == Constants.MapSize.Y))
                {
                    List<Node> bestPath = new List<Node>();
                    bestPath.Add(current);
                    while (current.parent != null && !startNodes.Contains(current))
                    {
                        bestPath.Add(current.parent);
                        current = current.parent;
                    }
                    return bestPath;
                }
                available.Remove(current);
                visited.Add(current);
                foreach (Node n in current.getNeighbors(nodes))
                {
                    if (visited.Contains(n))
                    {
                        continue;
                    }
                    int possibleScore = current.gScore + 1;
                    if (!available.Contains(n))
                    {
                        available.Add(n);
                    }
                    else if (possibleScore >= n.gScore)
                    {
                        continue;
                    }
                    n.parent = current;
                    n.gScore = possibleScore;
                    n.fScore = possibleScore + heuristic(n);
                }
            }
            return null;
        }
    }
}
