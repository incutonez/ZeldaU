using System;
using System.Collections;
using Enums;
using UnityEngine;

namespace Animation {
  public class Player : Base.Animation {
    public override float WalkFrameRate => 0.15f;
    public override float ActionFrameRate => 0.33f;

    public override void AnimateMove(Vector3 Movement) {
      base.AnimateMove(Movement);
      Manager.Game.Shield.ToggleShields(LastMovement.y == -1f || LastMovement == Vector3.zero, LastMovement.x > 0f, LastMovement.x < 0f);
    }

    public override IEnumerator AnimateEnter() {
      BlockAnimations = true;
      base.PlayAnimation(Animations.Entering);
      Vector3 position = GetPosition();
      Vector3 destination = new Vector3(position.x, position.y - 1);
      while (GetPosition() != destination) {
        transform.position = Vector3.MoveTowards(GetPosition(), destination, Time.deltaTime);
        yield return null;
      }

      BlockAnimations = false;
    }

    public override IEnumerator AnimateExit() {
      BlockAnimations = true;
      base.PlayAnimation(Animations.Exiting);
      Vector3 position = GetPosition();
      Vector3 destination = new Vector3(position.x, position.y + 0.9f);
      while (GetPosition() != destination) {
        transform.position = Vector3.MoveTowards(GetPosition(), destination, Time.deltaTime);
        yield return null;
      }

      BlockAnimations = false;
    }

    public override void AnimateAction() {
      base.AnimateAction();
      Manager.Game.Sword.Swing();
    }

    protected override void SpriteAnimator_OnAnimationStop(object sender, EventArgs e) {
      base.SpriteAnimator_OnAnimationStop(sender, e);
      // TODO: Don't like this... maybe make it specific to the action?
      Manager.Game.Sword.ToggleSword();
    }
  }
}
