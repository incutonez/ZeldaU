namespace Enemy {
  public class Rope : World.Enemy {
    public override void SetHealth() {
      Health = 2f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 1f;
    }
  }
}
