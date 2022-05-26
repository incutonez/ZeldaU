namespace Enemy {
  public class Rope : World.Enemy {
    public override float? Health { get; set; } = 2f;
    public override float TouchDamage { get; set; } = 1f;
  }
}
