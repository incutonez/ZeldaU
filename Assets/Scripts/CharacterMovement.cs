using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed;
    public bool CanAttack { get; set; } = true;
    public Vector3 Movement { get; set; }
    public Vector3 Target { get; set; }
    public World.AnimatorBase Animator { get; set; }
    public bool MovementDisabled { get; set; }
    private bool UseTarget { get; set; }

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
        if (UseTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target, Time.deltaTime * Speed);
            Movement = (Target - transform.position).normalized;
        }
        else
        {
            // Good resource https://forum.unity.com/threads/the-proper-way-to-control-the-player.429459/
            transform.Translate(Movement * Time.deltaTime * Speed);
        }
    }

    public void SetTarget(Vector3 target)
    {
        Target = target;
        UseTarget = true;
        Enable();
    }

    public void UnsetTarget()
    {
        // When we've reached the final destination, let's snap to the exact position
        transform.position = Target;
        Target = Vector3.zero;
        UseTarget = false;
        Disable();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Disable()
    {
        MovementDisabled = true;
    }

    public void Enable()
    {
        MovementDisabled = false;
    }

    public bool IsDisabled()
    {
        return MovementDisabled;
    }
}
