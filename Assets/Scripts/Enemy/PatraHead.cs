namespace Enemy {
  public class PatraHead : World.Enemy {
    public override void SetHealth() {
      // TODO: Get what this value should be
      Health = 20f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 4f;
    }
  }
}
