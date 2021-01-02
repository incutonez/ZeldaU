using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class PlayerBase : MonoBehaviour
    {
        public SpriteAnimator SpriteAnimator;
        public List<Sprite> ActionUpAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> ActionDownAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> ActionRightAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> ActionLeftAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> IdleUpAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> IdleDownAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> IdleRightAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> IdleLeftAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> WalkUpAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> WalkDownAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> WalkRightAnimation { get; set; } = new List<Sprite>();
        public List<Sprite> WalkLeftAnimation { get; set; } = new List<Sprite>();
        public bool BlockAnimations { get; set; }
        // By default, all enemies can attack, but this should be overriden if they can't
        public bool CanAttack = true;
        public Vector3 LastMovement { get; set; }
        public Animations ActiveAnimation { get; set; } = Animations.IdleDown;

        private void Awake()
        {
            SpriteAnimator = GetComponent<SpriteAnimator>();
            LoadSprites();
            if (SpriteAnimator != null)
            {
                SpriteAnimator.OnAnimationStop += SpriteAnimator_OnAnimationStop;
            }
        }

        // Each subclass should implement this
        public virtual void LoadSprites() { }

        public virtual void SpriteAnimator_OnAnimationStop(object sender, EventArgs e)
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
            Vector3 destination = new Vector3(position.x, position.y + 0.9f);
            while (GetPosition() != destination)
            {
                transform.position = Vector3.MoveTowards(GetPosition(), destination, Time.deltaTime);
                yield return null;
            }
            BlockAnimations = false;
        }

        public virtual void AnimateAction()
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

        public virtual void AnimateMove(Vector3 movement)
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
}