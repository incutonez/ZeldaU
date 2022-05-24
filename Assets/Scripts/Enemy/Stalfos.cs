namespace Enemy {
  public class Stalfos : World.Enemy {
    public override void SetHealth() {
      Health = 4f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 0.25f;
      // TODO: Verify the shoot damage of the sword
      WeaponDamage = 0.25f;
    }
  }
}
