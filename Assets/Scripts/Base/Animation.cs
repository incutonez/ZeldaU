using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: I think this should have an interface, and all animation extensions can override them
namespace Base {
  public class Animation : MonoBehaviour {
    public global::Animation.Frames Frames { get; set; }
    public Dictionary<Animations, List<Sprite>> AnimationSprites { get; set; } = new Dictionary<Animations, List<Sprite>>();

    public bool BlockAnimations { get; set; }

    // By default, all enemies can attack, but this should be overriden if they can't
    public bool CanAttack { get; set; } = true;
    public Vector3 LastMovement { get; set; }
    public Animations ActiveAnimation { get; set; } = Animations.IdleDown;
    public virtual float WalkFrameRate { get; set; } = 0f;
    public virtual float ActionFrameRate { get; set; } = 0f;
    public virtual float IdleFrameRate { get; set; } = 0f;
    public Movement CharacterMovement { get; set; }

    private void Awake() {
      Frames = GetComponent<global::Animation.Frames>();
      CharacterMovement = GetComponent<Movement>();
      if (Frames != null) {
        Frames.OnAnimationStop += SpriteAnimator_OnAnimationStop;
      }
    }

    public virtual Sprite GetDefaultSprite() {
      return AnimationSprites[Animations.IdleDown][0];
    }

    public bool IsBlocked() {
      return BlockAnimations || Manager.Game.IsPaused || CharacterMovement.IsDisabled();
    }

    public virtual void SpriteAnimator_OnAnimationStop(object sender, EventArgs e) {
      if (BlockAnimations) {
        BlockAnimations = false;
      }
    }

    public Vector3 GetPosition() {
      return CharacterMovement.GetPosition();
    }

    public virtual IEnumerator AnimateExit() {
      yield return null;
    }

    public virtual IEnumerator AnimateEnter() {
      yield return null;
    }

    public virtual void AnimateAction() {
      if (!CanAttack) {
        return;
      }

      BlockAnimations = true;
      if (LastMovement.x < 0) {
        PlayAnimation(Animations.ActionLeft);
      }
      else if (LastMovement.x > 0) {
        PlayAnimation(Animations.ActionRight);
      }
      else if (LastMovement.y > 0) {
        PlayAnimation(Animations.ActionUp);
      }
      else {
        PlayAnimation(Animations.ActionDown);
      }
    }

    public virtual void AnimateMove(Vector3 movement) {
      if (movement == Vector3.zero) {
        if (LastMovement.x < 0) {
          PlayAnimation(Animations.IdleLeft);
        }
        else if (LastMovement.x > 0) {
          PlayAnimation(Animations.IdleRight);
        }
        else if (LastMovement.y > 0) {
          PlayAnimation(Animations.IdleUp);
        }
        else {
          PlayAnimation(Animations.IdleDown);
        }
      }
      else {
        LastMovement = movement;
        float x = LastMovement.x;
        float y = LastMovement.y;
        bool xGreater = Math.Abs(x) >= Math.Abs(y);
        // We prefer horizontal animation over vertical
        if (x == -1 || x < 0 && xGreater) {
          PlayAnimation(Animations.WalkLeft);
        }
        // We prefer horizontal animation over vertical
        else if (x == 1 || x > 0 && xGreater) {
          PlayAnimation(Animations.WalkRight);
        }
        else if (y == -1 || y < 0) {
          PlayAnimation(Animations.WalkDown);
        }
        else {
          PlayAnimation(Animations.WalkUp);
        }
      }
    }

    public virtual void PlayAnimation(Animations type) {
      if (type != ActiveAnimation) {
        float frameRate = 0f;
        bool loop = true;
        switch (type) {
          case Animations.ActionUp:
          case Animations.ActionDown:
          case Animations.ActionRight:
          case Animations.ActionLeft:
            loop = false;
            frameRate = ActionFrameRate;
            break;
          case Animations.IdleUp:
          case Animations.IdleDown:
          case Animations.IdleRight:
          case Animations.IdleLeft:
            frameRate = IdleFrameRate;
            break;
          case Animations.Entering:
          case Animations.WalkUp:
          case Animations.Exiting:
          case Animations.WalkDown:
          case Animations.WalkRight:
          case Animations.WalkLeft:
            frameRate = WalkFrameRate;
            break;
        }

        // TODOJEF: Pick up here there's something odd here where some of the animations step on each other, like with Armos and moving sideways
        Frames.PlayAnimation(AnimationSprites[type], frameRate, loop);
        ActiveAnimation = type;
      }
    }
  }
}
