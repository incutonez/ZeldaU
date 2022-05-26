namespace Enemy {
  public class Darknut : World.Enemy {
    public override float? Health { get; set; } = 8f;
    public override float TouchDamage { get; set; } = 2f;
  }
}
