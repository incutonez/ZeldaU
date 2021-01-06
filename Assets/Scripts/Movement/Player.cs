using UnityEngine;

namespace Movement
{
    public class Player : Base.Movement
    {
        private void Update()
        {
            if (Animator.IsBlocked())
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.RightControl) && Animator.CanAttack)
            {
                Animator.AnimateAction();
            }
            else
            {
                float moveX = 0f;
                float moveY = 0f;
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    moveY = 1f;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    moveY = -1f;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    moveX = -1f;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    moveX = 1f;
                }
                Direction = new Vector3(moveX, moveY).normalized;

                Animator.AnimateMove(Direction);
            }
        }
    }

}
