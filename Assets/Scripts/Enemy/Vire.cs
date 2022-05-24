namespace Enemy {
  /// <summary>
  /// When it's hit without magical sword, it turns into 2 Keese
  /// </summary>
  public class Vire : World.Enemy {
    public override void SetHealth() {
      Health = 2f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 2f;
    }
  }
}
