using System;
using System.Linq;
using PathFinding.Dijkstr;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PathFinding
{
public class PathVisualizer : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new(10, 10);
    [SerializeField] private float nodeSize = 1f;
    [SerializeField] private Transform start, end;
    [SerializeField, Range(0, 100)] private float wallPercentage = 30f;

    private IPathFinding _pathFinding;

    private void Start()
    {
        GenerateGrid();
    }

    [ContextMenu("Re-Generate Grid")]
    private void GenerateGrid()
    {
        var grid = new Grid(gridSize.x, gridSize.y);
        foreach (Node node in grid)
            node.Walkable = Random.value > wallPercentage / 100;
        _debugGizmosPath = Array.Empty<Node>();
        
        //_pathFinding = new AStarPathFinding(grid);
        _pathFinding = new DijkstraPathFinding(grid);
    }

    private void Update()
    {
        _debugGizmosPath = null;

        if (start == null || end == null)
            return;

        var startNode = NodeFromWorldPoint(start.localPosition, _pathFinding.Grid);
        var endNode = NodeFromWorldPoint(end.localPosition, _pathFinding.Grid);

        if (_pathFinding.TryFindPath(startNode, endNode, out var path) == false)
        {
            Debug.LogWarning("Path Not Found!");
            return;
        }
        _debugGizmosPath = path;
    }

    private Node NodeFromWorldPoint(Vector3 worldPosition, Grid grid)
    {
        var percentX = (worldPosition.x + (float) gridSize.x / 2) / gridSize.x;
        var percentY = (worldPosition.z + (float) gridSize.y / 2) / gridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        var gridSizeX = Mathf.RoundToInt(gridSize.x / nodeSize);
        var gridSizeY = Mathf.RoundToInt(gridSize.y / nodeSize);
        var x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        var y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        x = Mathf.Clamp(x, 0, grid.XMax - 1);
        y = Mathf.Clamp(y, 0, grid.YMax - 1);

        return grid[x, y];
    }

    private Vector3 WorldPointFromNode(Node node)
    {
        var worldBottomLeft =
            transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        var nodeRadius = nodeSize / 2;
        var worldPoint = worldBottomLeft + Vector3.right * (node.X * nodeSize + nodeRadius) +
                         Vector3.forward * (node.Y * nodeSize + nodeRadius);

        return worldPoint;
    }

    [SerializeField, Range(0, 1)] private float debugAlpha = 0.7f;
    private Node[] _debugGizmosPath;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1f, gridSize.y));

        if (_pathFinding == null)
            return;

        foreach (Node node in _pathFinding.Grid)
        {
            var color = node.Walkable ? Color.white : Color.red;

            if (_debugGizmosPath != null && _debugGizmosPath.ToList().Contains(node))
                color = Color.black;

            color.a = debugAlpha;
            Gizmos.color = color;
            Gizmos.DrawCube(WorldPointFromNode(node), Vector3.one * (nodeSize - 0.1f));
        }
    }
}
}