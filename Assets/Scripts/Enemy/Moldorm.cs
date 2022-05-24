namespace Enemy {
  public class Moldorm : World.Enemy {
    public override void SetHealth() {
      Health = 10f;
      HealthModifier = 0f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 1f;
    }
  }
}
