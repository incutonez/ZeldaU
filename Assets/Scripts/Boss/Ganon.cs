namespace Boss {
  /// <summary>
  /// Final Boss
  /// 
  /// Other health:
  /// Must be defeated by silver arrow, and if he's not hit in the stunned state, then he will regain full health
  /// </summary>
  public class Ganon : World.Enemy {
    public override float? Health { get; set; } = 32f;
    public override float TouchDamage { get; set; } = 4f;
    public override float WeaponDamage { get; set; } = 2f;
  }
}
