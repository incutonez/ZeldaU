/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingDebug : MonoBehaviour
{

    public static PathfindingDebug Instance { get; private set; }

    [SerializeField]
    private Transform pfPathfindingDebugNode;
    private List<Transform> visualNodeList;
    private List<GridSnapshotAction> gridSnapshotActionList;
    private bool autoShowSnapshots;
    private float autoShowSnapshotsTimer;
    private Transform[,] visualNodeArray;

    private void Awake()
    {
        Instance = this;
        visualNodeList = new List<Transform>();
        gridSnapshotActionList = new List<GridSnapshotAction>();
    }

    public void Setup(ScreenGrid<PathNode> grid)
    {
        visualNodeArray = new Transform[grid.Width, grid.Height];

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                Vector3 gridPosition = new Vector3(x, y) * grid.CellSize + Vector3.one * grid.CellSize * .5f;
                Transform visualNode = CreateVisualNode(gridPosition);
                visualNodeArray[x, y] = visualNode;
                visualNodeList.Add(visualNode);
            }
        }
        HideNodeVisuals();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextSnapshot();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            autoShowSnapshots = true;
        }

        if (autoShowSnapshots)
        {
            float autoShowSnapshotsTimerMax = .05f;
            autoShowSnapshotsTimer -= Time.deltaTime;
            if (autoShowSnapshotsTimer <= 0f)
            {
                autoShowSnapshotsTimer += autoShowSnapshotsTimerMax;
                ShowNextSnapshot();
                if (gridSnapshotActionList.Count == 0)
                {
                    autoShowSnapshots = false;
                }
            }
        }
    }

    private void ShowNextSnapshot()
    {
        if (gridSnapshotActionList.Count > 0)
        {
            GridSnapshotAction gridSnapshotAction = gridSnapshotActionList[0];
            gridSnapshotActionList.RemoveAt(0);
            gridSnapshotAction.TriggerAction();
        }
    }

    public void ClearSnapshots()
    {
        gridSnapshotActionList.Clear();
    }

    public void TakeSnapshot(ScreenGrid<PathNode> grid, PathNode current, List<PathNode> openList, List<PathNode> closedList)
    {
        GridSnapshotAction gridSnapshotAction = new GridSnapshotAction();
        gridSnapshotAction.AddAction(HideNodeVisuals);

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                PathNode pathNode = grid.GetViewModel(x, y);

                int gCost = pathNode.WalkCost;
                int hCost = pathNode.DistanceCost;
                int fCost = pathNode.TotalCost;
                Vector3 gridPosition = new Vector3(pathNode.X, pathNode.Y) * grid.CellSize + Vector3.one * grid.CellSize * .5f;
                bool isCurrent = pathNode == current;
                bool isInOpenList = openList.Contains(pathNode);
                bool isInClosedList = closedList.Contains(pathNode);
                int tmpX = x;
                int tmpY = y;

                gridSnapshotAction.AddAction(() => {
                    Transform visualNode = visualNodeArray[tmpX, tmpY];
                    SetupVisualNode(visualNode, gCost, hCost, fCost);

                    Color backgroundColor = Utilities.HexToColor("636363");

                    if (isInClosedList)
                    {
                        backgroundColor = new Color(1, 0, 0);
                    }
                    if (isInOpenList)
                    {
                        backgroundColor = Utilities.HexToColor("009AFF");
                    }
                    if (isCurrent)
                    {
                        backgroundColor = new Color(0, 1, 0);
                    }

                    visualNode.Find("sprite").GetComponent<SpriteRenderer>().color = backgroundColor;
                });
            }
        }

        gridSnapshotActionList.Add(gridSnapshotAction);
    }

    public void TakeSnapshotFinalPath(ScreenGrid<PathNode> grid, List<PathNode> path)
    {
        GridSnapshotAction gridSnapshotAction = new GridSnapshotAction();
        gridSnapshotAction.AddAction(HideNodeVisuals);

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                PathNode pathNode = grid.GetViewModel(x, y);

                int gCost = pathNode.WalkCost;
                int hCost = pathNode.DistanceCost;
                int fCost = pathNode.TotalCost;
                Vector3 gridPosition = new Vector3(pathNode.X, pathNode.Y) * grid.CellSize + Vector3.one * grid.CellSize * .5f;
                bool isInPath = path.Contains(pathNode);
                int tmpX = x;
                int tmpY = y;

                gridSnapshotAction.AddAction(() => {
                    Transform visualNode = visualNodeArray[tmpX, tmpY];
                    SetupVisualNode(visualNode, gCost, hCost, fCost);

                    Color backgroundColor;

                    if (isInPath)
                    {
                        backgroundColor = new Color(0, 1, 0);
                    }
                    else
                    {
                        backgroundColor = Utilities.HexToColor("636363");
                    }

                    visualNode.Find("sprite").GetComponent<SpriteRenderer>().color = backgroundColor;
                });
            }
        }

        gridSnapshotActionList.Add(gridSnapshotAction);
    }

    private void HideNodeVisuals()
    {
        foreach (Transform visualNodeTransform in visualNodeList)
        {
            SetupVisualNode(visualNodeTransform, 9999, 9999, 9999);
        }
    }

    private Transform CreateVisualNode(Vector3 position)
    {
        Transform visualNodeTransform = Instantiate(pfPathfindingDebugNode, position, Quaternion.identity);
        return visualNodeTransform;
    }

    private void SetupVisualNode(Transform visualNodeTransform, int gCost, int hCost, int fCost)
    {
        if (fCost < 1000)
        {
            visualNodeTransform.Find("gCostText").GetComponent<TextMeshPro>().SetText(gCost.ToString());
            visualNodeTransform.Find("hCostText").GetComponent<TextMeshPro>().SetText(hCost.ToString());
            visualNodeTransform.Find("fCostText").GetComponent<TextMeshPro>().SetText(fCost.ToString());
        }
        else
        {
            visualNodeTransform.Find("gCostText").GetComponent<TextMeshPro>().SetText("");
            visualNodeTransform.Find("hCostText").GetComponent<TextMeshPro>().SetText("");
            visualNodeTransform.Find("fCostText").GetComponent<TextMeshPro>().SetText("");
        }
    }

    private class GridSnapshotAction
    {

        private Action action;

        public GridSnapshotAction()
        {
            action = () => { };
        }

        public void AddAction(Action action)
        {
            this.action += action;
        }

        public void TriggerAction()
        {
            action();
        }

    }

}

