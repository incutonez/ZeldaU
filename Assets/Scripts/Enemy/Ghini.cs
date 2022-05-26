namespace Enemy {
  /// <summary>
  /// You can only destroy the main Ghini... the other ones cannot be attacked
  /// </summary>
  public class Ghini : World.Enemy {
    public override float? Health { get; set; } = 22f;
    public override float TouchDamage { get; set; } = 1f;
  }
}
