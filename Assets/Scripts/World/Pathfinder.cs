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
        private const int MOVE_STRAIGHT_COST = 10;
        // Mathematical value for diagonal... sqrt(200) ~ 14
        private const int MOVE_DIAGONAL_COST = 14;

        public Grid<GridNode> Grid { get; set; }
        /// <summary>
        /// List for nodes to be searched
        /// </summary>
        public List<GridNode> OpenList { get; set; }
        /// <summary>
        /// List for nodes that have already been searched
        /// </summary>
        public List<GridNode> ClosedList { get; set; }

        public Pathfinder(int width, int height)
        {
            Grid = new Grid<GridNode>(width, height, 1f, new Vector3(-8f, -7.5f), (grid, x, y) => new GridNode(grid, x, y));
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
                if (viewModel.IsWalkable())
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
            List<GridNode> nodes = FindPath(startX, startY, endX, endY);
            if (nodes == null)
            {
                return null;
            }
            List<Vector3> result = new List<Vector3>();
            foreach (GridNode node in nodes)
            {
                result.Add(Grid.GetWorldPosition(node.X, node.Y));
            }
            return result;
        }

        public List<GridNode> FindPath(int startX, int startY, int endX, int endY)
        {
            GridNode startNode = Grid.GetViewModel(startX, startY);
            GridNode endNode = Grid.GetViewModel(endX, endY);

            OpenList = new List<GridNode> { startNode };
            ClosedList = new List<GridNode>();

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
                GridNode currentNode = GetLowestTotalCostNode(OpenList);
                if (currentNode == endNode)
                {
                    return CalculatePath(currentNode);
                }
                OpenList.Remove(currentNode);
                ClosedList.Add(currentNode);

                foreach (GridNode neighbor in GetNeighbors(currentNode))
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
        private List<GridNode> GetNeighbors(GridNode node)
        {
            List<GridNode> neighbors = new List<GridNode>();
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

        private List<GridNode> CalculatePath(GridNode endNode)
        {
            List<GridNode> path = new List<GridNode> { endNode };
            GridNode currentNode = endNode;
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

        // TODOJEF: Maybe look into this and not allowing diagonal?
        private int CalculateDistanceCost(GridNode a, GridNode b)
        {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        // TODO: It'd be more performant to use a binary tree
        private GridNode GetLowestTotalCostNode(List<GridNode> nodes)
        {
            GridNode lowestTotalCostNode = nodes[0];
            foreach (GridNode node in nodes)
            {
                if (node.TotalCost < lowestTotalCostNode.TotalCost)
                {
                    lowestTotalCostNode = node;
                }
            }
            return lowestTotalCostNode;
        }
    }

}
