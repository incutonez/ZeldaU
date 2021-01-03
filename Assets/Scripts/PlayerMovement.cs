using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    private void Update()
    {
        if (PlayerBase.BlockAnimations)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.RightControl) && PlayerBase.CanAttack)
        {
            PlayerBase.AnimateAction();
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

            PlayerBase.AnimateMove(Movement);
        }
    }

    private void FixedUpdate()
    {
        if (Manager.Game.IsTransitioning || PlayerBase.BlockAnimations)
        {
            return;
        }
        // Good resource https://forum.unity.com/threads/the-proper-way-to-control-the-player.429459/
        transform.Translate(Movement * Time.deltaTime * Speed);
    }
}
