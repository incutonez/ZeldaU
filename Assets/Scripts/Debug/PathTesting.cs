using System.Collections.Generic;
using UnityEngine;

public class PathTesting : MonoBehaviour
{
    public WorldScreen Visual;
    public Pathfinder PathFinder { get; set; }

    private void Start()
    {
        PathFinder = new Pathfinder(20, 10);
        Visual.SetGrid(PathFinder.Grid, true);
    }

    private void Update()
    {
        var grid = PathFinder.Grid;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(position, out int x, out int y);
            List<ScreenGridNode> path = PathFinder.FindPath(0, 0, x, y);
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
            ScreenGridNode node = grid.GetViewModel(position);
            node.Color = WorldColors.Tan;
            node.SetTileType(GetRandomTile());
        }
    }

    public Tiles GetRandomTile()
    {
        System.Random random = new System.Random();
        List<Tiles> tiles = EnumExtensions.GetValues<Tiles>();
        Tiles tile = tiles[random.Next(-1, tiles.Count - 1)];
        while (tile == Tiles.None || tile == Tiles.Castle || tile == Tiles.Door || tile == Tiles.Transition)
        {
            tile = tiles[random.Next(-1, tiles.Count - 1)];
        }
        return tile;
    }
}
