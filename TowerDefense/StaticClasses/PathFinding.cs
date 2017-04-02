using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public static class PathFinding
    {
        internal static List<Node> findBestPath(Node[,] nodes)
        {
            int numberOfCheese = 0;

            foreach (Node n in nodes)
            {
                numberOfCheese += n.cheese ? 1 : 0;
            }

            List<Node> startNodes = new List<Node>();
            List<Node> bestPathSoFar = new List<Node>();
            List<Node> goalNodes = new List<Node>();

            for (int i = 0; i <= Constants.MapSize.X; i++)
            {
                if (!nodes[i, 0].wall && !nodes[i,0].portal)
                {
                    startNodes.Add(nodes[i, 0]);
                }
            }
            foreach (Node n in nodes)
            {
                if (n.cheese)
                {
                    goalNodes.Add(n);
                }
            }

            for (int c = numberOfCheese; c >= 0; c--)
            {
                if (goalNodes.Count == 0)
                {
                    for (int i = 0; i <= Constants.MapSize.X; i++)
                    {
                        if (!nodes[i, Constants.MapSize.Y].wall)
                        {
                            goalNodes.Add(nodes[i, Constants.MapSize.Y]);
                        }
                    }
                }
                List<Node> bestPathRelay = findBestPath(nodes, startNodes, goalNodes);
                if (bestPathRelay == null)
                {
                    return null;
                }
                bestPathRelay.Reverse();
                bestPathSoFar.AddRange(bestPathRelay);

                startNodes.Clear();
                startNodes.Add(bestPathSoFar.Last());
                goalNodes.Remove(bestPathSoFar.Last());
            }

            return bestPathSoFar;
        }

        private static int heuristic(Node current)
        {
            return Constants.MapSize.Y - current.simplePos.Y;
        }

        internal static List<Node> findBestPath(Node[,] nodes, List<Node> startNodes, List<Node> goalNodes)
        {
            List<Node> available = new List<Node>(startNodes);
            HashSet<Node> visited = new HashSet<Node>();
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            Dictionary<Node, int> gScore = new Dictionary<Node, int>();
            Dictionary<Node, int> fScore = new Dictionary<Node, int>();

            foreach(Node n in startNodes)
            {
                gScore[n] = 0;
                fScore[n] = heuristic(n);
            }

            while (available.Count != 0)
            {
                Node current = available.OrderBy(n => fScore.ContainsKey(n) ? fScore[n] : int.MaxValue).First();
                if (goalNodes.Contains(current))
                {
                    List<Node> bestPath = new List<Node>();
                    bestPath.Add(current);
                    while (cameFrom.ContainsKey(current) && !startNodes.Contains(current))
                    {
                        bestPath.Add(cameFrom[current]);
                        current = cameFrom[current];
                    }
                    return bestPath;
                }
                available.Remove(current);
                visited.Add(current);
                Node currentNodeParent = cameFrom.ContainsKey(current) ? cameFrom[current] : null;
                foreach (Node n in current.getNeighbors(nodes, currentNodeParent))
                {
                    if (visited.Contains(n))
                    {
                        continue;
                    }
                    int possibleScore = gScore[current] + 1;
                    if (!available.Contains(n))
                    {
                        available.Add(n);
                    }
                    else if (gScore.ContainsKey(n) && possibleScore >= gScore[n])
                    {
                        continue;
                    }
                    cameFrom[n] = current;
                    gScore[n] = possibleScore;
                    fScore[n] = possibleScore + heuristic(n);
                }
            }
            return null;
        }
    }
}
