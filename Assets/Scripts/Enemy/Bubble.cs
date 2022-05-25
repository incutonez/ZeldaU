using UnityEngine;

namespace Enemy {
  /// <summary>
  /// This one is a little tricky because the base version flashes between all 4 variants... I'm not sure what the other 2 flash between.
  /// The body for this is made up of 3 different colors... the body, which is white, unless specified, and the accent/base
  /// 
  /// In order to regain control over his sword, he must touch a Blue Bubble, visit a Fairy Fountain, drink a potion, or pick up a Triforce Shard. 
  /// </summary>
  public class Bubble : World.Enemy {
    // TODO: REVISIT THIS AND REMOVE
    // This is like a burnt orange one
    public static readonly Color Base1 = Utilities.HexToColor("b8f818");

    public static readonly Color Accent1 = Utilities.HexToColor("ffe0a8");

    // This is a green one, which is used in the rotation of the 4... need to figure out how to keep swapping colors
    public static readonly Color Body4 = Utilities.HexToColor("58f898");
    public static readonly Color Base4 = EnemyHelper.CommonGreen;
    public static readonly Color Accent4 = Utilities.HexToColor("00a844");
  }
}
