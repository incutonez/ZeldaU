using System.Collections.Generic;
using UnityEngine;

public class PathTesting : MonoBehaviour
{
    public PathfindingVisual Visual;
    public Pathfinder PathFinder { get; set; }
    public PathfindingDebug PathfindingDebug;

    private void Start()
    {
        PathFinder = new Pathfinder(20, 10);
        //PathfindingDebug.Setup(PathFinder.Grid);
        Visual.SetGrid(PathFinder.Grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var grid = PathFinder.Grid;
            grid.GetXY(position, out int x, out int y);
            List<PathNode> path = PathFinder.FindPath(0, 0, x, y);
            Vector3 quadSize = Vector2.one * grid.CellSize * 0.5f;
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    var node = path[i];
                    var nextNode = path[i + 1];
                    Debug.DrawLine(grid.GetWorldPosition(node.X, node.Y) + quadSize, grid.GetWorldPosition(nextNode.X, nextNode.Y) + quadSize, Color.green, 100f);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PathNode node = PathFinder.Grid.GetViewModel(position);
            node.SetWalkable(false);
        }
    }
}
