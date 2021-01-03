using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Idea taken from https://www.youtube.com/watch?v=db0KWYaWfeM
/// </summary>
public class EnemyAI : MonoBehaviour
{
    public enum MovementState
    {
        Roaming = 0,
        Chasing = 1,
        Firing = 2
    }

    // TODOJEF: Add aiming script
    private Vector3 StartingPosition { get; set; }
    private Vector3 RoamingPosition { get; set; }
    private EnemyPathfinding EnemyPathfinding { get; set; }
    private ScreenGrid<ScreenGridNode> Grid { get; set; }
    private MovementState movementState { get; set; } = MovementState.Roaming;
    private World.Enemy WorldEnemy { get; set; }

    private void Awake()
    {
        EnemyPathfinding = GetComponent<EnemyPathfinding>();
        WorldEnemy = GetComponent<World.Enemy>();
    }

    private void Start()
    {
        Grid = Manager.Game.Pathfinder.Grid;
        StartingPosition = transform.localPosition;
        RoamingPosition = Manager.Game.Pathfinder.GetRoamingPosition(StartingPosition);
    }

    private void Update()
    {
        if (Manager.Game.IsTransitioning)
        {
            return;
        }
        switch (movementState)
        {
            case MovementState.Firing:
                break;
            case MovementState.Chasing:
                Vector3 playerPosition = Manager.Game.Player.GetPosition();
                EnemyPathfinding.MoveTo(playerPosition);

                // If within this range, then shoot
                if (Vector3.Distance(transform.position, playerPosition) < 0.5f)
                {
                    //movementState = MovementState.Firing;
                    // When animation is done, then set this back to Chasing... can pass in a lambda function to fire when the animation is done
                    // Enemy shoots
                    // Enemy stops movement
                    // Enemy has a float of the time in between firing, similar to the sword concept in WorldPlayer... compare with Time.time
                    //  Set this value to Time.time
                }

                if (Vector3.Distance(transform.position, playerPosition) > 5f)
                {
                    movementState = MovementState.Roaming;
                }
                break;
            case MovementState.Roaming:
            default:
                EnemyPathfinding.MoveTo(RoamingPosition);
                if (Vector3.Distance(transform.position, RoamingPosition) <= 1f)
                {
                    RoamingPosition = Manager.Game.Pathfinder.GetRoamingPosition(RoamingPosition);
                }
                if (CanChase())
                {
                    FindTarget();
                }
                break;
        }
    }

    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, Manager.Game.Player.GetPosition()) < 4f)
        {
            movementState = MovementState.Chasing;
        }
    }

    private bool CanChase()
    {
        return false;
    }
}
