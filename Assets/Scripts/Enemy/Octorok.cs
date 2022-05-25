namespace Enemy {
  public class Octorok : World.Enemy {
    public override void SetHealth() {
      Health = 2f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 1f;
      WeaponDamage = 1f;
    }

    protected override void SetFrameRates() {
      // TODO: Figure out a good multiplier for this... it's too fast, but we do need to speed up the animations slightly
      Animation.ActionFrameRate = 0.33f / Movement.Speed;
      Animation.IdleFrameRate = 1f;
      Animation.WalkFrameRate = 0.3f / Movement.Speed;
    }
  }
}
