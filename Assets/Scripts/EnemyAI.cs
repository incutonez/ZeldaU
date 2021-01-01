using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Idea taken from https://www.youtube.com/watch?v=db0KWYaWfeM
/// Pick up at 5:30
/// </summary>
public class EnemyAI : MonoBehaviour
{
    private Vector3 StartingPosition { get; set; }
    private Vector3 RoamingPosition { get; set; }
    private EnemyPathfinding EnemyPathfinding { get; set; }
    private ScreenGrid<ScreenGridNode> Grid { get; set; }

    private void Awake()
    {
        EnemyPathfinding = GetComponent<EnemyPathfinding>();
        Grid = GameHandler.Pathfinder.Grid;
    }

    private void Start()
    {
        StartingPosition = transform.localPosition;
        RoamingPosition = GameHandler.Pathfinder.GetRoamingPosition(StartingPosition);
    }

    private void Update()
    {
        EnemyPathfinding.MoveTo(RoamingPosition);
        if (Vector3.Distance(transform.position, RoamingPosition) <= 1f)
        {
            RoamingPosition = GameHandler.Pathfinder.GetRoamingPosition(RoamingPosition);
        }
    }
}
