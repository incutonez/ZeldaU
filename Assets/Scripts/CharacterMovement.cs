using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed;
    public bool CanAttack { get; set; } = true;
    public Vector3 Movement { get; set; }
    public World.AnimatorBase Animator { get; set; }

    private void Start()
    {
        Animator = GetComponent<World.AnimatorBase>();
    }

    private void Update()
    {
        if (Animator.IsBlocked())
        {
            return;
        }
        Animator.AnimateMove(Movement);
    }

    private void FixedUpdate()
    {
        if (Animator.IsBlocked())
        {
            return;
        }
        // Good resource https://forum.unity.com/threads/the-proper-way-to-control-the-player.429459/
        transform.Translate(Movement * Time.deltaTime * Speed);
    }
}
