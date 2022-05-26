namespace Enemy {
  public class Octorok : World.Enemy {
    public override float? Health { get; set; } = 2f;
    public override float TouchDamage { get; set; } = 1f;
    public override float WeaponDamage { get; set; } = 1f;

    protected override void SetFrameRates() {
      // TODO: Figure out a good multiplier for this... it's too fast, but we do need to speed up the animations slightly
      Animation.ActionFrameRate = 0.33f / Movement.Speed;
      Animation.IdleFrameRate = 1f;
      Animation.WalkFrameRate = 0.3f / Movement.Speed;
    }
  }
}
