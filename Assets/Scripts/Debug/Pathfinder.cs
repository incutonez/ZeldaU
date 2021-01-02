using System.Collections.Generic;
using UnityEngine;

// TODOJEF: Switch to Unity dots?  https://www.youtube.com/watch?v=1bO1FdEThnU
// TODOJEF: Or https://www.youtube.com/watch?v=ubUPVu_DeVk?
// TODOJEF: Also move this class out of debug
/// <summary>
/// Idea taken from https://www.youtube.com/watch?v=alU04hvz6L4
/// 
/// Open List - Nodes queued up for searching
/// Closed List - Nodes that have already been searched
/// G - Walking cost from start node
/// H - Heuristic cost to reach end node
/// F - G + H (overall cost)
/// </summary>
public class Pathfinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    // Mathematical value for diagonal... sqrt(200) ~ 14
    private const int MOVE_DIAGONAL_COST = 14;

    public ScreenGrid<ScreenGridNode> Grid { get; set; }
    /// <summary>
    /// List for nodes to be searched
    /// </summary>
    public List<ScreenGridNode> OpenList { get; set; }
    /// <summary>
    /// List for nodes that have already been searched
    /// </summary>
    public List<ScreenGridNode> ClosedList { get; set; }

    public Pathfinder(int width, int height)
    {
        Grid = new ScreenGrid<ScreenGridNode>(width, height, 1f, new Vector3(-8f, -7.5f), (grid, x, y) => new ScreenGridNode(grid, x, y));
    }

    public Vector3 GetQuadSize()
    {
        return Vector2.one * Grid.CellSize;
    }

    public Vector3 GetRoamingPosition(Vector3 startingPosition)
    {
        System.Random random = new System.Random();
        List<Vector3> openTiles = GetOpenTiles();
        List<Vector3> path = null;
        Vector3 position = Vector3.zero;
        openTiles.Remove(startingPosition);
        while ((path == null || path.Count == 1) && openTiles.Count > 0)
        {
            position = openTiles[random.Next(1, openTiles.Count)];
            path = FindPath(startingPosition, position);
            openTiles.Remove(position);
        }
        return position;
    }

    public List<Vector3> GetOpenTiles()
    {
        List<Vector3> result = new List<Vector3>();
        Grid.EachCell((viewModel, x, y) =>
        {
            // If we have the default value, then there's nothing in this tile
            if (viewModel.TileType == Tiles.None)
            {
                result.Add(Grid.GetWorldPosition(x, y));
            }
        });
        return result;
    }


    /// <summary>
    /// This method is a little confusing but because our meshes and sprites are pivoted in the center, but our grid positions
    /// them in the lower left of the cell, we need to adjust and center position the world position
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="quadSize"></param>
    /// <returns></returns>
    public Vector3 GetWorldPositionOffset(Vector3 worldPosition, Vector3 quadSize)
    {
        return worldPosition + quadSize * 0.5f;
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        Grid.GetXY(start, out int startX, out int startY);
        Grid.GetXY(end, out int endX, out int endY);
        List<ScreenGridNode> nodes = FindPath(startX, startY, endX, endY);
        if (nodes == null)
        {
            return null;
        }
        List<Vector3> result = new List<Vector3>();
        Vector3 quadSize = GetQuadSize();
        foreach (ScreenGridNode node in nodes)
        {
            result.Add(GetWorldPositionOffset(Grid.GetWorldPosition(node.X, node.Y), quadSize));
        }
        return result;
    }

    public List<ScreenGridNode> FindPath(int startX, int startY, int endX, int endY)
    {
        ScreenGridNode startNode = Grid.GetViewModel(startX, startY);
        ScreenGridNode endNode = Grid.GetViewModel(endX, endY);

        OpenList = new List<ScreenGridNode> { startNode };
        ClosedList = new List<ScreenGridNode>();

        Grid.EachCell((node, x, y) =>
        {
            node.WalkCost = int.MaxValue;
            node.CalculateTotalCost();
            node.PreviousNode = null;
        });

        startNode.WalkCost = 0;
        startNode.DistanceCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateTotalCost();

        while (OpenList.Count > 0)
        {
            ScreenGridNode currentNode = GetLowestTotalCostNode(OpenList);
            if (currentNode == endNode)
            {
                return CalculatePath(currentNode);
            }
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            foreach (ScreenGridNode neighbor in GetNeighbors(currentNode))
            {
                // We've already searched this neighbor, so let's go to next neighbor
                if (ClosedList.Contains(neighbor))
                {
                    continue;
                }
                if (!neighbor.IsWalkable())
                {
                    ClosedList.Add(neighbor);
                    continue;
                }

                int walkCost = currentNode.WalkCost + CalculateDistanceCost(currentNode, neighbor);
                if (walkCost < neighbor.WalkCost)
                {
                    neighbor.PreviousNode = currentNode;
                    neighbor.WalkCost = walkCost;
                    neighbor.DistanceCost = CalculateDistanceCost(neighbor, endNode);
                    neighbor.CalculateTotalCost();

                    if (!OpenList.Contains(neighbor))
                    {
                        OpenList.Add(neighbor);
                    }
                }
            }
        }

        // This means that we could not find a valid path
        return null;
    }

    // TODO: Can optimize this by pre-calculating all of the neighbors as soon as we create the grid
    private List<ScreenGridNode> GetNeighbors(ScreenGridNode node)
    {
        List<ScreenGridNode> neighbors = new List<ScreenGridNode>();
        if (node.X - 1 >= 0)
        {
            int localX = node.X - 1;
            // Left
            neighbors.Add(Grid.GetViewModel(localX, node.Y));
            // Diagonal down to the left
            if (node.Y - 1 >= 0)
            {
                // TODO: Currently, we don't allow these as neighbors... we only want up, down, left, and right... add a way to enable this?
                //neighbors.Add(Grid.GetViewModel(localX, node.Y - 1));
            }
            // Diagonal up to the left
            if (node.Y + 1 < Grid.Height)
            {
                // TODO: Currently, we don't allow these as neighbors... we only want up, down, left, and right... add a way to enable this?
                //neighbors.Add(Grid.GetViewModel(localX, node.Y + 1));
            }
        }
        if (node.X + 1 < Grid.Width)
        {
            int localX = node.X + 1;
            // Right
            neighbors.Add(Grid.GetViewModel(localX, node.Y));
            // Diagonal down to the right
            if (node.Y - 1 >= 0)
            {
                // TODO: Currently, we don't allow these as neighbors... we only want up, down, left, and right... add a way to enable this?
                //neighbors.Add(Grid.GetViewModel(localX, node.Y - 1));
            }
            // Diagonal up to the right
            if (node.Y + 1 < Grid.Height)
            {
                // TODO: Currently, we don't allow these as neighbors... we only want up, down, left, and right... add a way to enable this?
                //neighbors.Add(Grid.GetViewModel(localX, node.Y + 1));
            }
        }
        // Down
        if (node.Y - 1 >= 0)
        {
            neighbors.Add(Grid.GetViewModel(node.X, node.Y - 1));
        }
        // Up
        if (node.Y + 1 < Grid.Height)
        {
            neighbors.Add(Grid.GetViewModel(node.X, node.Y + 1));
        }
        return neighbors;
    }

    private List<ScreenGridNode> CalculatePath(ScreenGridNode endNode)
    {
        List<ScreenGridNode> path = new List<ScreenGridNode> { endNode };
        ScreenGridNode currentNode = endNode;
        // While the current node has a parent
        while (currentNode.PreviousNode != null)
        {
            path.Add(currentNode.PreviousNode);
            currentNode = currentNode.PreviousNode;
        }
        // Because we came in from the end, we need to reverse it, so we're at the beginning
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(ScreenGridNode a, ScreenGridNode b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    // TODO: It'd be more performant to use a binary tree
    private ScreenGridNode GetLowestTotalCostNode(List<ScreenGridNode> nodes)
    {
        ScreenGridNode lowestTotalCostNode = nodes[0];
        foreach (ScreenGridNode node in nodes)
        {
            if (node.TotalCost < lowestTotalCostNode.TotalCost)
            {
                lowestTotalCostNode = node;
            }
        }
        return lowestTotalCostNode;
    }
}
