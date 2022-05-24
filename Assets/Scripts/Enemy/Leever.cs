namespace Enemy {
  public class Leever : World.Enemy {
    public override void SetHealth() {
      Health = 4f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 1f;
    }
  }
}
