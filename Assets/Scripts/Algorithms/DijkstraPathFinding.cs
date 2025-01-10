using System;
using System.Collections.Generic;

namespace PathFinding.Dijkstr
{
public class DijkstraPathFinding : IPathFinding
{
    public Grid Grid { get; }

    public DijkstraPathFinding(Grid grid)
    {
        Grid = grid;
    }

    public bool TryFindPath(Node start, Node end, out Node[] path)
    {
        path = Array.Empty<Node>();
        Dictionary<Node, Node> nextNodeToGoal = new Dictionary<Node, Node>();
        Dictionary<Node, int> costToReachNode = new Dictionary<Node, int>();

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(end, 0);
        costToReachNode[end] = 0;

        while (frontier.Count > 0)
        {
            Node curNode = frontier.Dequeue();

            if (curNode == start)
                break;

            foreach (Node neighbor in Grid.GetNeighbors(curNode))
            {
                int newCost = costToReachNode[curNode] + 1;
                if (costToReachNode.ContainsKey(neighbor) == false || newCost < costToReachNode[neighbor])
                {
                    if (neighbor.Walkable)
                    {
                        costToReachNode[neighbor] = newCost;
                        int priority = newCost;
                        frontier.Enqueue(neighbor, priority);
                        nextNodeToGoal[neighbor] = curNode;
                    }
                }
            }
        }

        if (nextNodeToGoal.ContainsKey(start) == false)
            return false;

        var resultPath = new Queue<Node>();
        var pathNode = start;
        while (end != pathNode)
        {
            pathNode = nextNodeToGoal[pathNode];
            resultPath.Enqueue(pathNode);
        }

        path = resultPath.ToArray();

        return true;
    }

    private class PriorityQueue<T>
    {
        private readonly List<Tuple<T, int>> _elements = new();

        public int Count => _elements.Count;

        public void Enqueue(T item, int priority) => _elements.Add(Tuple.Create(item, priority));

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i].Item2 < _elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            T bestItem = _elements[bestIndex].Item1;
            _elements.RemoveAt(bestIndex);

            return bestItem;
        }
    }
}
}