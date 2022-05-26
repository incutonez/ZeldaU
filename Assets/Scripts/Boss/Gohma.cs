namespace Boss {
  /// <summary>
  /// Takes 1 arrow to kill
  /// 1st Quest B: 6
  /// 
  /// Other health:
  /// Must have eye open to hurt
  /// </summary>
  public class Gohma : World.Enemy {
    public override float? Health { get; set; } = 2f;
    public override float HealthModifier { get; set; } = 0f;
    public override float TouchDamage { get; set; } = 2f;
    public override float WeaponDamage { get; set; } = 2f;
  }
}
