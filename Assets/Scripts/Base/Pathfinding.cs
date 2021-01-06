using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public class Pathfinding : MonoBehaviour
    {
        private int CurrentPathIndex { get; set; }
        private List<Vector3> Path { get; set; }
        private Movement Movement { get; set; }

        private void Awake()
        {
            Movement = GetComponent<Movement>();
        }

        private void Update()
        {
            if (Manager.Game.IsTransitioning || Movement.IsDisabled())
            {
                return;
            }
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (Path == null)
            {
                StopMoving();
            }
            else
            {
                Vector3 currentPosition = Movement.GetPosition();
                Vector3 targetPosition = Path[CurrentPathIndex];
                if (Vector3.Distance(currentPosition, targetPosition) > 0.02)
                {
                    Movement.SetTarget(targetPosition);
                }
                else
                {
                    CurrentPathIndex++;
                    if (CurrentPathIndex >= Path.Count)
                    {
                        StopMoving();
                    }
                }
            }
        }

        public void MoveTo(Vector3 position)
        {
            Movement.Enable();
            CurrentPathIndex = 0;
            Path = Manager.Game.Pathfinder.FindPath(Movement.GetPosition(), position);
            if (Path != null && Path.Count > 1)
            {
                Path.RemoveAt(0);
            }
        }

        private void StopMoving()
        {
            Path = null;
            Movement.UnsetTarget();
        }
    }
}