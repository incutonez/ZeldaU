namespace Enemy {
  public class Lynel : World.Enemy {
    public override float? Health { get; set; } = 8f;
    public override float TouchDamage { get; set; } = 2f;
    public override float WeaponDamage { get; set; } = 2f;
  }
}
