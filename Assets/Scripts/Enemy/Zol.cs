namespace Enemy {
  /// <summary>
  /// If hit with the regular sword, it breaks into 2 gels
  /// </summary>
  public class Zol : World.Enemy {
    public override float? Health { get; set; } = 2f;
    public override float TouchDamage { get; set; } = 2f;
  }
}
