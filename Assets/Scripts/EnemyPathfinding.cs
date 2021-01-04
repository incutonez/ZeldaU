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


    private void Start()
    {
        Transform bodyTransform = transform.Find("Body");
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        //PrintPathfindingPath();
        if (pathVectorList != null)
        {
            Vector3 currentPosition = GetPosition();
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(currentPosition, targetPosition) > 0.02)
            {
                CharacterMovement.Movement = (targetPosition - currentPosition).normalized;
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
        else
        {
            StopMoving();
        }
    }

    private void PrintPathfindingPath()
    {
        if (pathVectorList != null)
        {
            for (int i = 0; i < pathVectorList.Count - 1; i++)
            {
                Debug.DrawLine(pathVectorList[i], pathVectorList[i + 1], Color.green);
            }
        }
    }

    public void MoveTo(Vector3 position)
    {
        SetTargetPosition(position);
    }

    private void StopMoving()
    {
        pathVectorList = null;
        CharacterMovement.Movement = Vector3.zero;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Manager.Game.Pathfinder.FindPath(GetPosition(), targetPosition);
        Debug.Log($"{GetPosition()} {pathVectorList[0]}");
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

}