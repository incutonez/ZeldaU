using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Idea taken from https://www.youtube.com/watch?v=db0KWYaWfeM
/// </summary>
public class EnemyAI : MonoBehaviour
{
    public enum MovementState
    {
        None = 0,
        Roaming = 1,
        Chasing = 2,
        Firing = 3
    }

    // TODOJEF: Add aiming script
    private Vector3 RoamingPosition { get; set; }
    private EnemyPathfinding EnemyPathfinding { get; set; }
    private ScreenGrid<ScreenGridNode> Grid { get; set; }
    private MovementState State { get; set; } = MovementState.Roaming;
    private World.Enemy WorldEnemy { get; set; }
    private bool HasStarted { get; set; }

    private void Awake()
    {
        EnemyPathfinding = GetComponent<EnemyPathfinding>();
        WorldEnemy = GetComponent<World.Enemy>();
    }

    private void Start()
    {
        Grid = Manager.Game.Pathfinder.Grid;
    }

    private void Update()
    {
        if (Manager.Game.IsTransitioning)
        {
            return;
        }
        switch (State)
        {
            case MovementState.Firing:
                break;
            case MovementState.Chasing:
                Vector3 playerPosition = GetPlayerPosition();
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
                // Otherwise, if player is too far, let's stop chasing
                else if (Vector3.Distance(transform.position, playerPosition) > 5f)
                {
                    State = MovementState.Roaming;
                    // Move back to the roaming position
                    EnemyPathfinding.MoveTo(RoamingPosition);
                }
                // Otherwise, continue pursuit and update with new position
                else
                {
                    EnemyPathfinding.MoveTo(playerPosition);
                }
                break;
            case MovementState.Roaming:
                // Once we've reached the destination, let's pick a new one
                if (!HasStarted || transform.position == RoamingPosition)
                {
                    RoamingPosition = Manager.Game.Pathfinder.GetRoamingPosition(RoamingPosition);
                    EnemyPathfinding.MoveTo(RoamingPosition);
                    HasStarted = true;
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
        Vector3 playerPosition = GetPlayerPosition();
        if (Vector3.Distance(transform.position, playerPosition) < 4f)
        {
            State = MovementState.Chasing;
            // Let's start chasing toward the player
            EnemyPathfinding.MoveTo(playerPosition);
        }
    }

    private bool CanChase()
    {
        return false;
    }

    public Vector3 GetPlayerPosition()
    {
        return Manager.Game.Player.Animator.GetPosition();
    }
}
