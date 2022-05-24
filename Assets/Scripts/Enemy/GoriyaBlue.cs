namespace Enemy {
  public class GoriyaBlue : Goriya {
    public override void SetHealth() {
      Health = 10f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 2f;
      WeaponDamage = 2f;
    }
  }
}
