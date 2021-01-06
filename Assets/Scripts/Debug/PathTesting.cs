using System;
using System.Collections.Generic;
using UnityEngine;

public class PathTesting : MonoBehaviour
{
    public World.Screen Visual;
    public World.Pathfinder PathFinder { get; set; }

    private void Awake()
    {
        Manager.Game.OnLaunch += Game_OnLaunch;
    }

    private void Game_OnLaunch(object sender, EventArgs e)
    {
        PathFinder = Manager.Game.Pathfinder;
        Visual.SetGrid(PathFinder.Grid, true);
    }

    private void Update()
    {
        // Game hasn't launched yet
        if (PathFinder == null)
        {
            return;
        }
        var grid = PathFinder.Grid;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(position, out int x, out int y);
            List<World.GridNode> path = PathFinder.FindPath(0, 0, x, y);
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
            World.GridNode node = grid.GetViewModel(position);
            node.Color = WorldColors.Tan;
            node.SetTileType(GetRandomTile());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            World.GridNode node = grid.GetViewModel(position);
            Manager.Character.SpawnEnemy(grid.GetWorldPosition(node.X, node.Y), NPCs.Enemies.Octorok, Visual.transform, true);
        }
    }

    public Tiles GetRandomTile()
    {
        System.Random random = new System.Random();
        List<Tiles> tiles = EnumExtensions.GetValues<Tiles>();
        tiles.Remove(Tiles.None);
        tiles.Remove(Tiles.Castle);
        tiles.Remove(Tiles.Door);
        tiles.Remove(Tiles.Transition);
        return tiles[random.Next(0, tiles.Count - 1)];
    }
}
