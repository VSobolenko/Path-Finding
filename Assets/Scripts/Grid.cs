using System.Collections;
using System.Collections.Generic;

namespace PathFinding
{
public class Grid : IEnumerable
{
    private readonly Node[,] _nodes;

    public int XMax => _nodes.GetLength(0);
    public int YMax => _nodes.GetLength(1);

    public Grid(int x, int y)
    {
        _nodes = new Node[x, y];
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (var x = 0; x < XMax; x++)
        {
            for (var y = 0; y < YMax; y++)
            {
                _nodes[x, y] = new Node(x, y);
            }
        }
    }

    public Node[] GetNeighbors(Node node)
    {
        var neighbors = new List<Node>();

        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                var checkX = node.X + x;
                var checkY = node.Y + y;

                if (checkX >= 0 && checkX < XMax && checkY >= 0 && checkY < YMax)
                    neighbors.Add(_nodes[checkX, checkY]);
            }
        }

        return neighbors.ToArray();
    }

    public Node this[int x, int y] => _nodes[x, y];

    public IEnumerator GetEnumerator() => _nodes.GetEnumerator();
}
}