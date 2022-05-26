namespace Enemy {
  public class Gibdo : World.Enemy {
    public override float? Health { get; set; } = 12f;
    public override float HealthModifier { get; set; } = 0.75f;
    public override float TouchDamage { get; set; } = 4f;
  }
}
