namespace Boss {
  /// <summary>
  /// 1st Quest B: 3
  /// 1st Quest E: 4 (total 1) and 8 (total 3)
  /// 2nd Quest E: 2 (total 1), 5 (total 1), 6 (total 1), and 7 (total 2)
  /// 
  /// Other health:
  /// Each tentacle requires that many hits, so it's [4, 2, 1] x 4 tentacles
  /// If a tentacle is hit with 1 bomb, it's destroyed... blast radius could destroy multiple
  /// </summary>
  public class Manhandla : Gohma {
    public override float? Health { get; set; } = 8f;
    public override float TouchDamage { get; set; } = 2f;
    public override float WeaponDamage { get; set; } = 2f;
  }
}
