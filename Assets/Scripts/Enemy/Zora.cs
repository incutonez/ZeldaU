namespace Enemy {
  public class Zora : World.Enemy {
    public override float? Health { get; set; } = 4f;
    public override float TouchDamage { get; set; } = 1f;
    public override float WeaponDamage { get; set; } = 1f;
  }
}
