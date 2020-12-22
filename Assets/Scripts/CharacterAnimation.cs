using System.Collections;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;

    private Vector3 lastMovement;

    public void Awake()
    {
        // TODOJEF: Figure out a better way?
        var body = transform.Find("Body");
        body.GetComponent<WorldObjectData>().SetObjectSize(body.GetComponent<SpriteRenderer>().sprite.bounds.size);
    }

    // Idea taken from https://www.youtube.com/watch?v=Bf_5qIt9Gr8
    public void Animate(Vector3 movement)
    {
        if (movement == Vector3.zero)
        {
            Idle(movement);
        }
        else
        {
            lastMovement = movement;
            Walk(movement);
        }
        GameHandler.shieldHandler.ToggleShields(lastMovement.y == -1f || lastMovement == Vector3.zero, lastMovement.x > 0f, lastMovement.x < 0f);
    }

    public void Idle(Vector3 movement)
    {
        animator.SetBool("isMoving", false);
    }

    public void Walk(Vector3 movement)
    {
        animator.SetFloat("xMove", movement.x);
        animator.SetFloat("yMove", movement.y);
        animator.SetBool("isMoving", true);
    }

    public IEnumerator Attack()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", true);
        yield return GameHandler.swordHandler.Swing(lastMovement);
        animator.SetBool("isAttacking", false);
    }
}
