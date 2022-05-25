using System.Collections.Generic;
using Enums;
using UnityEngine;

public static class EnemyHelper {
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
