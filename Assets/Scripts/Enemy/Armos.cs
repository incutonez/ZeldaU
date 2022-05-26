namespace Enemy {
  /// <summary>
  /// Randomly spawn as very fast or slow
  /// </summary>
  public class Armos : World.Enemy {
    public override float? Health { get; set; } = 6f;
    public override float TouchDamage { get; set; } = 1f;
  }
}
