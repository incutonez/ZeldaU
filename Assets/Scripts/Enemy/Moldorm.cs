namespace Enemy {
  public class Moldorm : World.Enemy {
    public override float? Health { get; set; } = 10f;
    public override float HealthModifier { get; set; } = 0f;
    public override float TouchDamage { get; set; } = 1f;
  }
}
