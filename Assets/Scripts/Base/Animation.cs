using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

// TODO: I think this should have an interface, and all animation extensions can override them
namespace Base {
  public class Animation : MonoBehaviour {
    private global::Animation.Frames Frames { get; set; }
    public Dictionary<Animations, List<Sprite>> AnimationSprites { get; set; } = new Dictionary<Animations, List<Sprite>>();

    protected bool BlockAnimations { get; set; }

    // By default, all enemies can attack, but this should be overriden if they can't
    public bool CanAttack { get; set; } = true;
    public Vector3 LastMovement { get; set; }
    private Animations ActiveAnimation { get; set; } = Animations.IdleDown;
    public virtual float WalkFrameRate { get; set; }
    public virtual float ActionFrameRate { get; set; }
    public virtual float IdleFrameRate { get; set; }
    private Movement CharacterMovement { get; set; }

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

    protected virtual void SpriteAnimator_OnAnimationStop(object sender, EventArgs e) {
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
      var x = LastMovement.x;
      var y = LastMovement.y;
      if (movement == Vector3.zero) {
        if (x < 0) {
          PlayAnimation(Animations.IdleLeft);
        }
        else if (x > 0) {
          PlayAnimation(Animations.IdleRight);
        }
        else if (y > 0) {
          PlayAnimation(Animations.IdleUp);
        }
        else {
          PlayAnimation(Animations.IdleDown);
        }
      }
      else {
        LastMovement = movement;
        var isHorizontal = Math.Abs(Math.Abs(x) - 1) < 0.000001;
        var xGreater = Math.Abs(x) >= Math.Abs(y);
        // We prefer horizontal animation over vertical
        if (x < 0 && (isHorizontal || xGreater)) {
          PlayAnimation(Animations.WalkLeft);
        }
        // We prefer horizontal animation over vertical
        else if (x > 0 && (isHorizontal || xGreater)) {
          PlayAnimation(Animations.WalkRight);
        }
        else if (y < 0) {
          PlayAnimation(Animations.WalkDown);
        }
        else {
          PlayAnimation(Animations.WalkUp);
        }
      }
    }

    public virtual void PlayAnimation(Animations type) {
      if (type != ActiveAnimation) {
        var frameRate = 0f;
        var loop = true;
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

        Frames.PlayAnimation(AnimationSprites[type], frameRate, loop);
        ActiveAnimation = type;
      }
    }
  }
}
