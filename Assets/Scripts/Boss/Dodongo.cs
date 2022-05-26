namespace Boss {
  /// <summary>
  /// 1st Quest B: 2 (total 1)
  /// 1st Quest E: 5 (total 3 x 1 pair) and 7 (total 6 x 2 pairs)
  /// 2nd Quest B: 3 (total 3 x 1 pair) and 8 (total 3 x 1 pair)
  /// 2nd Quest E: 1 (total 1), 4 (total 4 x 1 x 1 pair), 8 (total 9 x 3 pairs)
  /// 
  /// Other health:
  /// If bomb is placed on its back, you can attack with sword for 1 hit kill
  /// </summary>
  public class Dodongo : World.Enemy {
    public override float? Health { get; set; } = 4f;
    public override float HealthModifier { get; set; } = 0f;
    public override float TouchDamage { get; set; } = 2f;
  }
}
