namespace Enemy {
  /// <summary>
  /// If sucked in, the player will randomly lose their Magical Shield
  /// </summary>
  public class LikeLike : World.Enemy {
    public override void SetHealth() {
      Health = 20f;
    }

    public override void SetAttackStrength() {
      TouchDamage = 2f;
    }
  }
}
