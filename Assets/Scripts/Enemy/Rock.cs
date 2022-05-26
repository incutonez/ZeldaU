namespace Enemy {
  public class Rock : World.Enemy {
    public override float? Health { get; set; } = 0f;
    public override float TouchDamage { get; set; } = 1f;
  }
}
