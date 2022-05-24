namespace Enemy {
  public class RopeBlue : Rope {
    public override void SetHealth() {
      Health = 8f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 1f;
    }
  }
}
