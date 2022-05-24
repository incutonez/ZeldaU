namespace Enemy {
  public class Trap : World.Enemy {
    public override void SetAttackStrength() {
      TouchDamage = 1f;
    }
  }
}
