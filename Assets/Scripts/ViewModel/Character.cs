// ReSharper disable ClassNeverInstantiated.Global

using Enums;

namespace ViewModel {
  public class Character<TEnum> {
    public float X { get; set; }
    public float Y { get; set; }
    public TEnum Type { get; set; }
    public float? Speed { get; set; } = 3f;
    public float? Health { get; set; }
    public float? HealthModifier { get; set; }
    public float? TouchDamage { get; set; }
    public float? WeaponDamage { get; set; }
    public int Count { get; set; } = 1;
    /// <summary>
    /// This comes in as pairs... the first color is the target, and the second is the replacement.
    /// If a 3rd color is specified, then that's the 2nd target color, and so on.
    /// </summary>
    public WorldColors[] Colors { get; set; }
  }
}
