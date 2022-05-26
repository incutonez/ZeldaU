namespace Enemy {
  /// <summary>
  /// When it's hit without magical sword, it turns into 2 Keese
  /// </summary>
  public class Vire : World.Enemy {
    public override float? Health { get; set; } = 2f;
    public override float TouchDamage { get; set; } = 2f;
  }
}
