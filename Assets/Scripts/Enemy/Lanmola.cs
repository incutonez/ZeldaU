namespace Enemy {
  public class Lanmola : World.Enemy {
    public override void SetHealth() {
      Health = 8f;
      HealthModifier = 0f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 4f;
    }
  }
}
