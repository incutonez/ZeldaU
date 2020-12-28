using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Vector3 StartingPosition { get; set; }

    private void Start()
    {
        StartingPosition = transform.localPosition;
    }
}
