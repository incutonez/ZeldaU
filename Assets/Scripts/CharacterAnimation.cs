using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;

    private Vector3 lastMovement;
    private Dictionary<string, float> animationLengths = new Dictionary<string, float>();

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

    public IEnumerator Enter()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isEntering", true);
        AudioManager.Instance.PlayFX(FX.Stairs);
        // We want to start the load of the new screen a little earlier, so we divide by 1.5
        yield return new WaitForSeconds(GetAnimationClipLength("Entering") / 1.5f);
    }

    public IEnumerator Exit()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isExiting", true);
        AudioManager.Instance.PlayFX(FX.Stairs);
        yield return new WaitForSeconds(GetAnimationClipLength("Entering"));
        animator.SetBool("isExiting", false);
    }

    // Idea taken from https://forum.unity.com/threads/how-to-find-animation-clip-length.465751/
    public float GetAnimationClipLength(string animationName)
    {
        if (animationLengths.ContainsKey(animationName))
        {
            return animationLengths[animationName];
        }
        float length = 0f;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                length = clip.length;
                animationLengths.Add(animationName, length);
                break;
            }
        }
        return length;
    }

}
