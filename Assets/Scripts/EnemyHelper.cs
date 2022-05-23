using NPCs;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyHelper
{
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

    public static System.Type GetEnemyClass(Enemies type)
    {
        return System.Type.GetType($"Enemy.{type.GetDescription()}");
    }

    public static void GetSubTypes(
        Enemies baseType,
        Dictionary<Animations, List<Sprite>> enemyAnimations,
        Dictionary<Enemies, Dictionary<Animations, List<Sprite>>> allEnemies
    )
    {
        List<Enemies> subTypes = new List<Enemies>();
        List<Color[]> colors = new List<Color[]>();
        // Dynamically invoke the enemy's static method... we create a get method for each enum type
        GetEnemyClass(baseType).GetMethod("GetConfig")?.Invoke(null, new object[] { subTypes, colors });
        List<Dictionary<Animations, List<Sprite>>> variants = new List<Dictionary<Animations, List<Sprite>>>();
        foreach (Enemies enemyType in subTypes)
        {
            Dictionary<Animations, List<Sprite>> animations = new Dictionary<Animations, List<Sprite>>();
            allEnemies.Add(enemyType, animations);
            variants.Add(animations);
        }
        Utilities.ColorAnimations(enemyAnimations, variants, colors);
    }
}
