//using Unity.Collections;
//using Unity.Jobs;
//using Unity.Mathematics;
//using UnityEngine;

///// <summary>
///// Taken from https://www.youtube.com/watch?v=1bO1FdEThnU... kinda need to finish this with the job portion
///// then can look at https://www.youtube.com/watch?v=ubUPVu_DeVk
///// </summary>
//public class PathfinderDots : MonoBehaviour
//{
//    public const int DIAGONAL_COST = 14;
//    public const int STRAIGHT_COST = 10;

//    private void Start()
//    {
//        FindPath(new int2(0, 0), new int2(3, 1));
//    }

//    public void FindPath(int2 startPos, int2 endPos)
//    {
//        int2 gridSize = new int2(4, 4);
//        NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);
//        for (int x = 0; x < gridSize.x; x++)
//        {
//            for (int y = 0; y < gridSize.y; y++)
//            {
//                PathNode node = new PathNode();
//                node.X = x;
//                node.Y = y;
//                node.Index = CalculateIndex(x, y, gridSize.x);
//                node.WalkCost = int.MaxValue;
//                node.DistanceCost = CalculateDistanceCost(new int2(x, y), endPos);
//                node.CalculateTotalCost();
//                node.IsWalkable = true;
//                node.PreviousNodeIndex = -1;
//                pathNodeArray[node.Index] = node;
//            }
//        }

//        PathNode myNode = pathNodeArray[CalculateIndex(1, 0, gridSize.x)];
//        myNode.SetIsWalkable(false);
//        pathNodeArray[CalculateIndex(1, 0, gridSize.x)] = myNode;

//        myNode = pathNodeArray[CalculateIndex(1, 1, gridSize.x)];
//        myNode.SetIsWalkable(false);
//        pathNodeArray[CalculateIndex(1, 1, gridSize.x)] = myNode;

//        myNode = pathNodeArray[CalculateIndex(1, 2, gridSize.x)];
//        myNode.SetIsWalkable(false);
//        pathNodeArray[CalculateIndex(1, 2, gridSize.x)] = myNode;

//        NativeArray<int2> neighborOffsets = new NativeArray<int2>(new int2[]
//        {
//            // Left
//            new int2(-1, 0),
//            // Right
//            new int2(1, 0),
//            // Down
//            new int2(0, -1),
//            // Up
//            new int2(0, 1)
//        }, Allocator.Temp);
//        int endNodeIndex = CalculateIndex(endPos.x, endPos.y, gridSize.x);
//        PathNode startNode = pathNodeArray[CalculateIndex(startPos.x, startPos.y, gridSize.x)];
//        startNode.WalkCost = 0;
//        startNode.CalculateTotalCost();
//        // Because we're using a NativeArray, we need to reassign this value, as we don't update it by reference
//        pathNodeArray[startNode.Index] = startNode;

//        NativeList<int> openList = new NativeList<int>(Allocator.Temp);
//        NativeList<int> closeList = new NativeList<int>(Allocator.Temp);
//        openList.Add(startNode.Index);
//        while (openList.Length > 0)
//        {
//            int currentNodeIndex = GetLowestTotalCostNode(openList, pathNodeArray);
//            PathNode currentNode = pathNodeArray[currentNodeIndex];
//            if (currentNodeIndex == endNodeIndex)
//            {
//                // Reached destination
//                break;
//            }

//            for (int i = 0; i < openList.Length; i++)
//            {
//                if (openList[i] == currentNodeIndex)
//                {
//                    openList.RemoveAtSwapBack(i);
//                    break;
//                }
//            }
//            closeList.Add(currentNodeIndex);

//            for (int i = 0; i < neighborOffsets.Length; i++)
//            {
//                int2 neighborOffset = neighborOffsets[i];
//                int2 neighborPosition = new int2(currentNode.X + neighborOffset.x, currentNode.Y + neighborOffset.y);
//                if (!IsValidPosition(neighborPosition, gridSize.x))
//                {
//                    continue;
//                }
//                int neighborNodeIndex = CalculateIndex(neighborPosition.x, neighborPosition.y, gridSize.x);
//                if (closeList.Contains(neighborNodeIndex))
//                {
//                    // Already been searched
//                    continue;
//                }
//                PathNode neighborNode = pathNodeArray[neighborNodeIndex];
//                if (!neighborNode.IsWalkable)
//                {
//                    continue;
//                }

//                int2 currentNodePos = new int2(currentNode.X, currentNode.Y);
//                int tentativeCost = currentNode.WalkCost + CalculateDistanceCost(currentNodePos, neighborPosition);
//                if (tentativeCost < neighborNode.WalkCost)
//                {
//                    neighborNode.PreviousNodeIndex = currentNodeIndex;
//                    neighborNode.WalkCost = tentativeCost;
//                    neighborNode.CalculateTotalCost();
//                    pathNodeArray[neighborNodeIndex] = neighborNode;

//                    if (!openList.Contains(neighborNode.Index))
//                    {
//                        openList.Add(neighborNode.Index);
//                    }
//                }
//            }
//        }

//        PathNode endNode = pathNodeArray[endNodeIndex];
//        if (endNode.PreviousNodeIndex == -1)
//        {
//            // No path found
//            Debug.Log("No path found.");
//        }
//        else
//        {
//            // Found a path
//            NativeList<int2> path = GetPath(pathNodeArray, endNode);
//            foreach (int2 pathPos in path)
//            {
//                Debug.Log(pathPos);
//            }
//            path.Dispose();
//        }

//        pathNodeArray.Dispose();
//        neighborOffsets.Dispose();
//        openList.Dispose();
//        closeList.Dispose();
//    }

//    public NativeList<int2> GetPath(NativeArray<PathNode> nodes, PathNode endNode)
//    {
//        NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
//        if (endNode.PreviousNodeIndex != -1)
//        {
//            // Found a path
//            path.Add(new int2(endNode.X, endNode.Y));
//            PathNode currentNode = endNode;
//            while (currentNode.PreviousNodeIndex != -1)
//            {
//                PathNode previousNode = nodes[currentNode.PreviousNodeIndex];
//                path.Add(new int2(previousNode.X, previousNode.Y));
//                currentNode = previousNode;
//            }
//        }
//        return path;
//    }

//    public bool IsValidPosition(int2 gridPos, int2 gridSize)
//    {
//        return gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x < gridSize.x && gridPos.y < gridSize.y;
//    }

//    public int GetLowestTotalCostNode(NativeList<int> openList, NativeArray<PathNode> nodes)
//    {
//        PathNode lowestNode = nodes[openList[0]];
//        for (int i = 1; i < openList.Length; i++)
//        {
//            PathNode node = nodes[openList[i]];
//            if (node.TotalCost < lowestNode.TotalCost)
//            {
//                lowestNode = node;
//            }
//        }
//        return lowestNode.Index;
//    }

//    public int CalculateDistanceCost(int2 a, int2 b)
//    {
//        int x = math.abs(a.x - b.x);
//        int y = math.abs(a.y - b.y);
//        int remaining = math.abs(x - y);
//        return DIAGONAL_COST * math.min(x, y) + STRAIGHT_COST * remaining;
//    }

//    public int CalculateIndex(int x, int y, int gridWidth)
//    {
//        return x + y * gridWidth;
//    }

//    public struct PathNodeJob : IJob
//    {
//        public int2 StartPos { get; set; }
//        public int2 EndPost { get; set; }

//        public void Execute()
//        {
//            throw new System.NotImplementedException();
//        }
//    }

//    public struct PathNode
//    {
//        public int X { get; set; }
//        public int Y { get; set; }
//        public int Index { get; set; }
//        public int WalkCost { get; set; }
//        public int DistanceCost { get; set; }
//        public int TotalCost { get; set; }
//        public bool IsWalkable { get; set; }
//        public int PreviousNodeIndex { get; set; }

//        public void CalculateTotalCost()
//        {
//            TotalCost = WalkCost + DistanceCost;
//        }

//        public void SetIsWalkable(bool isWalkable)
//        {
//            IsWalkable = isWalkable;
//        }
//    }
//}
