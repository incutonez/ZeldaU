namespace Enemy {
  public class Stalfos : World.Enemy {
    public override float? Health { get; set; } = 4f;
    public override float TouchDamage { get; set; } = 0.25f;
    // TODO: Verify the shoot damage of the sword
    public override float WeaponDamage { get; set; } = 0.25f;
  }
}
