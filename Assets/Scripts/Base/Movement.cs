using UnityEngine;

namespace Base
{
    public class Movement : MonoBehaviour
    {
        public float Speed { get; set; } = 3f;
        public bool CanAttack { get; set; } = true;
        public Vector3 Direction { get; set; }
        public Vector3 Target { get; set; }
        public Animation Animator { get; set; }
        public bool MovementDisabled { get; set; }
        private bool UseTarget { get; set; }

        public virtual void Start()
        {
            Animator = GetComponent<Animation>();
        }

        private void Update()
        {
            if (Animator.IsBlocked())
            {
                return;
            }
            Animator.AnimateMove(Direction);
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
                Direction = (Target - transform.position).normalized;
            }
            else
            {
                // Good resource https://forum.unity.com/threads/the-proper-way-to-control-the-player.429459/
                transform.Translate(Direction * Time.deltaTime * Speed);
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
}
