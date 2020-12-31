using UnityEngine;

/// <summary>
/// Idea taken from https://www.youtube.com/watch?v=db0KWYaWfeM
/// </summary>
public class EnemyAI : MonoBehaviour
{
    private Vector3 StartingPosition { get; set; }

    private void Start()
    {
        StartingPosition = transform.localPosition;
    }
}
