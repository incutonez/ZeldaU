namespace Enemy {
  public class Lanmola : World.Enemy {
    public override float? Health { get; set; } = 8f;
    public override float HealthModifier { get; set; } = 0f;
    public override float TouchDamage { get; set; } = 4f;
  }
}
