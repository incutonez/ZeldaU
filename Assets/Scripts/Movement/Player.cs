using UnityEngine;

namespace Movement {
  public class Player : Base.Movement {
    public override void Start() {
      // TODO: Potentially figure out a better way of doing this?
      Speed = 5f;
      base.Start();
    }

    private void Update() {
      if (Animator.IsBlocked()) {
        return;
      }

      if (Input.GetKeyDown(KeyCode.RightControl) && Animator.CanAttack) {
        Animator.AnimateAction();
      }
      else {
        float moveX = 0f;
        float moveY = 0f;
        if (Controls.IsUpKey()) {
          moveY = 1f;
        }

        if (Controls.IsDownKey()) {
          moveY = -1f;
        }

        if (Controls.IsRightKey()) {
          moveX = 1f;
        }

        if (Controls.IsLeftKey()) {
          moveX = -1f;
        }

        Direction = new Vector3(moveX, moveY).normalized;

        Animator.AnimateMove(Direction);
      }
    }
  }
}
