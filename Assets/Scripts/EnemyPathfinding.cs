using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private CharacterMovement CharacterMovement { get; set; }

    private void Awake()
    {
        CharacterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if (Manager.Game.IsTransitioning || CharacterMovement.IsDisabled())
        {
            return;
        }
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (pathVectorList == null)
        {
            StopMoving();
        }
        else
        {
            Vector3 currentPosition = CharacterMovement.GetPosition();
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(currentPosition, targetPosition) > 0.02)
            {
                CharacterMovement.SetTarget(targetPosition);
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                }
            }
        }
    }

    public void MoveTo(Vector3 position)
    {
        CharacterMovement.Enable();
        currentPathIndex = 0;
        pathVectorList = Manager.Game.Pathfinder.FindPath(CharacterMovement.GetPosition(), position);
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    private void StopMoving()
    {
        pathVectorList = null;
        CharacterMovement.UnsetTarget();
    }
}