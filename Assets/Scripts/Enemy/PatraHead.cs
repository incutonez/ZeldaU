namespace Enemy {
  public class PatraHead : World.Enemy {
    public override void SetHealth() {
      // TODOJEF: GET?
      Health = 20f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 4f;
    }
  }
}
