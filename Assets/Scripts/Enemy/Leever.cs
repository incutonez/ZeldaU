namespace Enemy {
  public class Leever : World.Enemy {
    public override float? Health { get; set; } = 4f;
    public override float TouchDamage { get; set; } = 1f;
  }
}
