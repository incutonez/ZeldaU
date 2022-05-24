using NPCs;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyHelper {
  // TODO: Move these colors to constants
  /// <summary>
  /// This is used as the "body" color for enemies, as well as the primary color in CastleMaterials... it's white.
  /// </summary>
  public static readonly Color BodyColor = Color.white;

  /// <summary>
  /// This is used as the "base" color for enemies... it's black.
  /// </summary>
  public static readonly Color BaseColor = Color.black;

  /// <summary>
  /// This is used as the "accent" color for enemies, and also as a secondary color in CastleMaterials... it's red.
  /// </summary>
  public static readonly Color AccentColor = Color.red;

  /// <summary>
  /// This is used as the 3rd color that can be replaced in CastleMaterials... it's blue.
  /// </summary>
  public static readonly Color CastleDoorColor = Color.blue;

  public static readonly Color CommonRed = Utilities.HexToColor("f83800");
  public static readonly Color CommonOrange = Utilities.HexToColor("ffa044");
  public static readonly Color CommonOrangeDark = Utilities.HexToColor("e45c10");
  public static readonly Color CommonBlue = Utilities.HexToColor("0000bc");
  public static readonly Color CommonBlueLight = Utilities.HexToColor("6888ff");
  public static readonly Color CommonTeal = Utilities.HexToColor("008888");
  public static readonly Color CommonGray = Utilities.HexToColor("7c7c7c");
  public static readonly Color CommonGreen = Utilities.HexToColor("005800");
  public static readonly Color CommonWater = Utilities.HexToColor("2038ec");

  public static System.Type GetEnemyClass(Enemies type) {
    return System.Type.GetType($"Enemy.{type.GetDescription()}");
  }

  public static Dictionary<Animations, List<Sprite>> GetAnimations(
    Enemies baseType,
    string animationKey,
    Color[] colors
  ) {
    var allEnemies = Manager.Game.Graphics.EnemyAnimations;
    if (!allEnemies.ContainsKey(animationKey)) {
      allEnemies.Add(animationKey, Utilities.ColorAnimations(allEnemies[baseType.ToString()], colors));
    }

    return allEnemies[animationKey];
  }
}
