namespace Enemy {
  public class PatraHead : World.Enemy {
    // TODO: Get what this value should be
    public override float? Health { get; set; } = 20f;
    public override float TouchDamage { get; set; } = 4f;
  }
}
