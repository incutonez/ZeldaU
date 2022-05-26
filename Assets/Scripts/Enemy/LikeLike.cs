namespace Enemy {
  /// <summary>
  /// If sucked in, the player will randomly lose their Magical Shield
  /// </summary>
  public class LikeLike : World.Enemy {
    public override float? Health { get; set; } = 20f;
    public override float TouchDamage { get; set; } = 2f;
  }
}
