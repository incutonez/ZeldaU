using System;
using System.Collections;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public SpriteAnimator SpriteAnimator;
    public Sprite[] ActionUpAnimation;
    public Sprite[] ActionDownAnimation;
    public Sprite[] ActionRightAnimation;
    public Sprite[] ActionLeftAnimation;
    public Sprite[] IdleUpAnimation;
    public Sprite[] IdleDownAnimation;
    public Sprite[] IdleRightAnimation;
    public Sprite[] IdleLeftAnimation;
    public Sprite[] WalkUpAnimation;
    public Sprite[] WalkDownAnimation;
    public Sprite[] WalkRightAnimation;
    public Sprite[] WalkLeftAnimation;
    public bool BlockAnimations { get; set; }
    // By default, all enemies can attack, but this should be overriden if they can't
    public bool CanAttack = true;

    private Vector3 LastMovement { get; set; }
    private Animations ActiveAnimation { get; set; } = Animations.IdleDown;

    private void Awake()
    {
        SpriteAnimator = GetComponent<SpriteAnimator>();
        if (SpriteAnimator != null)
        {
            SpriteAnimator.OnAnimationStop += SpriteAnimator_OnAnimationStop;
        }
    }

    private void SpriteAnimator_OnAnimationStop(object sender, EventArgs e)
    {
        if (BlockAnimations)
        {
            BlockAnimations = false;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public IEnumerator AnimateEnter()
    {
        BlockAnimations = true;
        PlayAnimation(Animations.Entering);
        Vector3 position = GetPosition();
        Vector3 destination = new Vector3(position.x, position.y - 1);
        while (GetPosition() != destination)
        {
            transform.position = Vector3.MoveTowards(GetPosition(), destination, Time.deltaTime);
            yield return null;
        }
        BlockAnimations = false;
    }

    public IEnumerator AnimateExit()
    {
        BlockAnimations = true;
        PlayAnimation(Animations.Exiting);
        Vector3 position = GetPosition();
        Vector3 destination = new Vector3(position.x, position.y + 1);
        while (GetPosition() != destination)
        {
            transform.position = Vector3.MoveTowards(GetPosition(), destination, Time.deltaTime);
            yield return null;
        }
        BlockAnimations = false;
    }

    public void AnimateAction()
    {
        if (!CanAttack)
        {
            return;
        }
        BlockAnimations = true;
        if (LastMovement.x < 0)
        {
            PlayAnimation(Animations.ActionLeft);
        }
        else if (LastMovement.x > 0)
        {
            PlayAnimation(Animations.ActionRight);
        }
        else if (LastMovement.y > 0)
        {
            PlayAnimation(Animations.ActionUp);
        }
        else
        {
            PlayAnimation(Animations.ActionDown);
        }
    }

    public void AnimateMove(Vector3 movement)
    {
        if (movement == Vector3.zero)
        {
            if (LastMovement.x < 0)
            {
                PlayAnimation(Animations.IdleLeft);
            }
            else if (LastMovement.x > 0)
            {
                PlayAnimation(Animations.IdleRight);
            }
            else if (LastMovement.y > 0)
            {
                PlayAnimation(Animations.IdleUp);
            }
            else
            {
                PlayAnimation(Animations.IdleDown);
            }
        }
        else
        {
            LastMovement = movement;
            if (LastMovement.x < 0)
            {
                PlayAnimation(Animations.WalkLeft);
            }
            else if (LastMovement.x > 0)
            {
                PlayAnimation(Animations.WalkRight);
            }
            else if (LastMovement.y > 0)
            {
                PlayAnimation(Animations.WalkUp);
            }
            else
            {
                PlayAnimation(Animations.WalkDown);
            }
        }
    }

    public void PlayAnimation(Animations type)
    {
        if (type != ActiveAnimation)
        {
            switch (type)
            {
                case Animations.ActionUp:
                    SpriteAnimator.PlayAnimation(ActionUpAnimation, 0.33f, false);
                    break;
                case Animations.ActionDown:
                    SpriteAnimator.PlayAnimation(ActionDownAnimation, 0.33f, false);
                    break;
                case Animations.ActionRight:
                    SpriteAnimator.PlayAnimation(ActionRightAnimation, 0.33f, false);
                    break;
                case Animations.ActionLeft:
                    SpriteAnimator.PlayAnimation(ActionLeftAnimation, 0.33f, false);
                    break;
                case Animations.IdleUp:
                    SpriteAnimator.PlayAnimation(IdleUpAnimation, 1f);
                    break;
                case Animations.IdleDown:
                    SpriteAnimator.PlayAnimation(IdleDownAnimation, 1f);
                    break;
                case Animations.IdleRight:
                    SpriteAnimator.PlayAnimation(IdleRightAnimation, 1f);
                    break;
                case Animations.IdleLeft:
                    SpriteAnimator.PlayAnimation(IdleLeftAnimation, 1f);
                    break;
                case Animations.Entering:
                case Animations.WalkUp:
                    SpriteAnimator.PlayAnimation(WalkUpAnimation, 0.15f);
                    break;
                case Animations.Exiting:
                case Animations.WalkDown:
                    SpriteAnimator.PlayAnimation(WalkDownAnimation, 0.15f);
                    break;
                case Animations.WalkRight:
                    SpriteAnimator.PlayAnimation(WalkRightAnimation, 0.15f);
                    break;
                case Animations.WalkLeft:
                    SpriteAnimator.PlayAnimation(WalkLeftAnimation, 0.15f);
                    break;
            }
            ActiveAnimation = type;
        }
    }
}
