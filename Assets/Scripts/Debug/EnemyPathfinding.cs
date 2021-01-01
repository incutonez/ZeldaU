using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    private const float speed = 5f;

    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private Vector3 moveDir { get; set; }


    private void Start()
    {
        Transform bodyTransform = transform.Find("Body");
    }

    private void Update()
    {
        HandleMovement();
        transform.position += moveDir * speed * Time.deltaTime;
    }

    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 currentPosition = GetPosition();
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(currentPosition, targetPosition) > 0.25f)
            {
                moveDir = (targetPosition - currentPosition).normalized;
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
        SetTargetPosition(position);
    }

    private void StopMoving()
    {
        pathVectorList = null;
        moveDir = Vector3.zero;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = GameHandler.Pathfinder.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

}