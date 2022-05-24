namespace Enemy {
  public class DarknutBlue : Darknut {
    public override void SetHealth() {
      Health = 16f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 4f;
    }
  }
}
