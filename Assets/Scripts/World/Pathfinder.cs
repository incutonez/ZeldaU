using System.Collections.Generic;
using UnityEngine;

namespace World
{
    // TODOJEF: Or https://www.youtube.com/watch?v=ubUPVu_DeVk?
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
        private const int MoveStraightCost = 10;
        // Mathematical value for diagonal... sqrt(200) ~ 14
        private const int MoveDiagonalCost = 14;

        public Grid<GridCell> Grid { get; set; }
        /// <summary>
        /// List for nodes to be searched
        /// </summary>
        public List<GridCell> OpenList { get; set; }
        /// <summary>
        /// List for nodes that have already been searched
        /// </summary>
        public List<GridCell> ClosedList { get; set; }

        public Pathfinder(int width, int height)
        {
            Grid = new Grid<GridCell>(width, height, 1f, new Vector3(-8f, -7.5f), (grid, x, y) => new GridCell(grid, x, y));
        }

        public Vector3 GetQuadSize()
        {
            return Vector2.one * Grid.CellSize;
        }

        public Vector3 GetRoamingPosition(Vector3 startingPosition)
        {
            System.Random random = new System.Random();
            List<GridCell> openTiles = GetOpenTiles();
            List<Vector3> path = null;
            GridCell end = null;
            GridCell start = Grid.GetViewModel(startingPosition);
            openTiles.Remove(start);
            while ((path == null || path.Count == 1) && openTiles.Count > 0)
            {
                end = openTiles[random.Next(1, openTiles.Count)];
                path = FindPath(start, end);
                openTiles.Remove(end);
            }
            // TODOJEF: This is the issue
            return end == null ? Vector3.zero : new Vector3(end.X, end.Y);
        }

        public List<GridCell> GetOpenTiles()
        {
            List<GridCell> result = new();
            Grid.EachCell((viewModel, x, y) =>
            {
                // If we have the default value, then there's nothing in this tile
                if (viewModel.IsWalkable())
                {
                    result.Add(Grid.GetViewModel(Grid.GetWorldPosition(x, y)));
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

        public List<Vector3> FindPath(Vector3 start, Vector3 end) {
            return FindPath(Grid.GetViewModel(start), Grid.GetViewModel(end));
        }

        public List<Vector3> FindPath(GridCell startCell, GridCell endCell) {
            OpenList = new List<GridCell> { startCell };
            ClosedList = new List<GridCell>();

            Grid.EachCell((node, x, y) =>
            {
                node.WalkCost = int.MaxValue;
                node.CalculateTotalCost();
                node.PreviousCell = null;
            });

            startCell.WalkCost = 0;
            startCell.DistanceCost = CalculateDistanceCost(startCell, endCell);
            startCell.CalculateTotalCost();

            while (OpenList.Count > 0)
            {
                GridCell currentCell = GetLowestTotalCostNode(OpenList);
                if (currentCell == endCell)
                {
                    var nodes = CalculatePath(currentCell);
                    List<Vector3> result = new();
                    foreach (GridCell node in nodes)
                    {
                        result.Add(Grid.GetWorldPosition(node.X, node.Y));
                    }
                    return result;
                }
                OpenList.Remove(currentCell);
                ClosedList.Add(currentCell);

                foreach (GridCell neighbor in GetNeighbors(currentCell))
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

                    int walkCost = currentCell.WalkCost + CalculateDistanceCost(currentCell, neighbor);
                    if (walkCost < neighbor.WalkCost)
                    {
                        neighbor.PreviousCell = currentCell;
                        neighbor.WalkCost = walkCost;
                        neighbor.DistanceCost = CalculateDistanceCost(neighbor, endCell);
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
        private List<GridCell> GetNeighbors(GridCell cell)
        {
            List<GridCell> neighbors = new List<GridCell>();
            if (cell.X - 1 >= 0)
            {
                int localX = cell.X - 1;
                // Left
                neighbors.Add(Grid.GetViewModel(localX, cell.Y));
                // Diagonal down to the left
                if (cell.Y - 1 >= 0)
                {
                    // TODO: Currently, we don't allow these as neighbors... we only want up, down, left, and right... add a way to enable this?
                    //neighbors.Add(Grid.GetViewModel(localX, node.Y - 1));
                }
                // Diagonal up to the left
                if (cell.Y + 1 < Grid.Height)
                {
                    // TODO: Currently, we don't allow these as neighbors... we only want up, down, left, and right... add a way to enable this?
                    //neighbors.Add(Grid.GetViewModel(localX, node.Y + 1));
                }
            }
            if (cell.X + 1 < Grid.Width)
            {
                int localX = cell.X + 1;
                // Right
                neighbors.Add(Grid.GetViewModel(localX, cell.Y));
                // Diagonal down to the right
                if (cell.Y - 1 >= 0)
                {
                    // TODO: Currently, we don't allow these as neighbors... we only want up, down, left, and right... add a way to enable this?
                    //neighbors.Add(Grid.GetViewModel(localX, node.Y - 1));
                }
                // Diagonal up to the right
                if (cell.Y + 1 < Grid.Height)
                {
                    // TODO: Currently, we don't allow these as neighbors... we only want up, down, left, and right... add a way to enable this?
                    //neighbors.Add(Grid.GetViewModel(localX, node.Y + 1));
                }
            }
            // Down
            if (cell.Y - 1 >= 0)
            {
                neighbors.Add(Grid.GetViewModel(cell.X, cell.Y - 1));
            }
            // Up
            if (cell.Y + 1 < Grid.Height)
            {
                neighbors.Add(Grid.GetViewModel(cell.X, cell.Y + 1));
            }
            return neighbors;
        }

        private List<GridCell> CalculatePath(GridCell endCell)
        {
            List<GridCell> path = new List<GridCell> { endCell };
            GridCell currentCell = endCell;
            // While the current node has a parent
            while (currentCell.PreviousCell != null)
            {
                path.Add(currentCell.PreviousCell);
                currentCell = currentCell.PreviousCell;
            }
            // Because we came in from the end, we need to reverse it, so we're at the beginning
            path.Reverse();
            return path;
        }

        // TODOJEF: Maybe look into this and not allowing diagonal?
        private int CalculateDistanceCost(GridCell a, GridCell b)
        {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
        }

        // TODO: It'd be more performant to use a binary tree
        private GridCell GetLowestTotalCostNode(List<GridCell> nodes)
        {
            GridCell lowestTotalCostCell = nodes[0];
            foreach (GridCell node in nodes)
            {
                if (node.TotalCost < lowestTotalCostCell.TotalCost)
                {
                    lowestTotalCostCell = node;
                }
            }
            return lowestTotalCostCell;
        }
    }

}
