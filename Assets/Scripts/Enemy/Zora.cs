namespace Enemy {
  public class Zora : World.Enemy {
    public override void SetHealth() {
      Health = 4f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 1f;
      WeaponDamage = 1f;
    }
  }
}
