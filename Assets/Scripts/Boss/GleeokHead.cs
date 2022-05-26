namespace Boss {
  public class GleeokHead : World.Enemy {
    public override float? Health { get; set; } = 0f;

    // TODO: Verify this damage
    public override float TouchDamage { get; set; } = 2f;

    // TODO: Verify this damage
    public override float WeaponDamage { get; set; } = 2f;
  }
}
