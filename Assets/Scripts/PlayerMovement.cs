using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    private void Update()
    {
        if (Animator.BlockAnimations)
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
            Movement = new Vector3(moveX, moveY).normalized;

            Animator.AnimateMove(Movement);
        }
    }
}
