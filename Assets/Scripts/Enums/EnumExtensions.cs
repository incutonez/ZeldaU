using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Idea taken from https://codereview.stackexchange.com/questions/5352/getting-the-value-of-a-custom-attribute-from-an-enum
public class CustomAttribute : Attribute {
  public string Value { get; }
  private string Name { get; }

  internal CustomAttribute(string name, string value) {
    Name = name;
    Value = value;
  }
}

public class ColorAttribute : Attribute {
  public Color Color { get; }

  public ColorAttribute(string color) {
    Color = Utilities.HexToColor(color);
  }
}

public class EnemyClassAttribute : Attribute {
  private object EnemyClass { get; }

  public EnemyClassAttribute(Type enemyClass) {
    EnemyClass = enemyClass;
  }
}

public class DamageAttribute : Attribute {
  /// <summary>
  /// This is based on 1/2 heart, so a Damage of 1 will take away:
  /// - 1/2 heart with green ring
  /// - 1/4 heart with blue ring
  /// - 1/8 heart with red ring
  /// </summary>
  public float TouchDamage { get; }

  public float WeaponDamage { get; }

  internal DamageAttribute(float touchDamage = 0f, float weaponDamage = 0f) {
    TouchDamage = touchDamage;
    WeaponDamage = weaponDamage;
  }
}

public class HealthAttribute : Attribute {
  /// <summary>
  /// The health/damage system is based on 1/2 hearts
  /// </summary>
  private int Health { get; }

  private float Modifier { get; }

  internal HealthAttribute(int health = 0, float modifier = 1f) {
    Health = health;
    Modifier = modifier;
  }
}

public static class EnumExtensions {
  // Taken from https://stackoverflow.com/questions/972307/how-to-loop-through-all-enum-values-in-c
  public static List<T> GetValues<T>() {
    return Enum.GetValues(typeof(T)).Cast<T>().ToList();
  }

  public static Color GetColor(this Enum value) {
    return GetAttribute<ColorAttribute>(value).Color;
  }

  public static string GetDescription(this Enum value) {
    return GetCustomAttr(value, "Description");
  }

  public static T GetAttribute<T>(this Enum value) where T : Attribute {
    var type = value.GetType();
    var name = Enum.GetName(type, value);
    var field = type.GetField(name);
    return field?.GetCustomAttribute<T>();
  }

  public static string GetCustomAttr(this Enum value, string propertyName) {
    var type = value.GetType();
    var name = Enum.GetName(type, value);
    var field = type.GetField(name);
    if (field != null) {
      if (propertyName == "Description") {
        var attr = field.GetCustomAttribute<DescriptionAttribute>();
        if (attr != null) {
          return attr.Description;
        }
      }
      else {
        var attr = field.GetCustomAttribute<CustomAttribute>();
        if (attr != null) {
          return attr.Value;
        }
      }
    }

    return name;
  }
}
