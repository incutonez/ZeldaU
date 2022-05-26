namespace Boss {
  /// <summary>
  /// 1st Quest B: 1 and 7
  /// 2nd Quest B: 1
  /// 2nd Quest E: 4 and 8
  /// 
  /// Other health:
  /// Bombs - 2 hits
  /// Bow - 3 hits
  /// Wand - 3 hits
  /// </summary>
  public class Aquamentus : World.Enemy {
    public override float? Health { get; set; } = 12f;
    public override float TouchDamage { get; set; } = 1f;
    public override float WeaponDamage { get; set; } = 1f;
  }
}
