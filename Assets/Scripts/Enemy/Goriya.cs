namespace Enemy {
  public class Goriya : World.Enemy {
    public override float? Health { get; set; } = 6f;
    public override float TouchDamage { get; set; } = 1f;
    public override float WeaponDamage { get; set; } = 2f;
  }
}
