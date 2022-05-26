namespace Enemy {
  public class Patra : World.Enemy {
    public override float? Health { get; set; } = 20f;
    public override float TouchDamage { get; set; } = 4f;
  }
}
