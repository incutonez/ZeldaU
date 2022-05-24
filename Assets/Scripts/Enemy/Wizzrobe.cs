namespace Enemy {
  public class Wizzrobe : World.Enemy {
    public override void SetHealth() {
      Health = 6f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 2f;
      WeaponDamage = 8f;
    }
  }
}
