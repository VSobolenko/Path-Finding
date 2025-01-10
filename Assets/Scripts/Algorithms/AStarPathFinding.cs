using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding.AStar
{
public class AStarPathFinding : IPathFinding
{
    public Grid Grid { get; }

    public AStarPathFinding(Grid grid)
    {
        Grid = grid;
    }

    public bool TryFindPath(Node start, Node end, out Node[] path)
    {
        path = Array.Empty<Node>();
        var capacity = Mathf.Abs(start.X - end.X);
        var openSet = new List<Node>(capacity);
        var closedSet = new HashSet<Node>(capacity);

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            var currentNode = openSet[0];
            for (var i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost ||
                    (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == end)
            {
                path = RetracePath(start, end);

                return true;
            }

            foreach (var neighbor in Grid.GetNeighbors(currentNode))
            {
                if (!neighbor.Walkable || closedSet.Contains(neighbor))
                    continue;

                var newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.GCost || openSet.Contains(neighbor) == false)
                {
                    neighbor.GCost = newMovementCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, end);
                    neighbor.Parent = currentNode;

                    if (openSet.Contains(neighbor) == false)
                        openSet.Add(neighbor);
                }
            }
        }

        return false;
    }

    private static int GetDistance(Node nodeA, Node nodeB)
    {
        var dstX = Mathf.Abs(nodeA.X - nodeB.X);
        var dstY = Mathf.Abs(nodeA.Y - nodeB.Y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }

    private static Node[] RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Node>();
        var currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        return path.ToArray();
    }
}
}