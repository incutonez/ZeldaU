namespace Enemy {
  /// <summary>
  /// When this catches player, the player returns to the beginning of the castle
  /// </summary>
  public class Wallmaster : World.Enemy {
    public override float? Health { get; set; } = 6f;
    public override float TouchDamage { get; set; } = 1f;
  }
}
