using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SpritesManager
{
    public static List<Sprite> Characters { get; set; }
    public static List<Sprite> Enemies { get; set; }
    public static List<Sprite> Items { get; set; }
    public static List<Sprite> Tiles { get; set; }

    public static void LoadAll()
    {
        Characters = LoadSprites("characters");
        Enemies = LoadSprites("enemies");
        Items = LoadSprites("items");
        Tiles = LoadSprites("tiles");
    }

    public static List<Sprite> LoadSprites(string name)
    {
        return Resources.LoadAll<Sprite>($"{Constants.PATH_SPRITES}{name}").ToList();
    }

    public static Sprite GetCharacter(string name)
    {
        return Characters.Find(s => s.name == name);
    }

    public static Sprite GetEnemy(string name)
    {
        return Enemies.Find(s => s.name == name);
    }

    public static Sprite GetItem(string name)
    {
        return Items.Find(s => s.name == name);
    }

    public static Sprite GetItem(Items type)
    {
        return GetItem(type.GetCustomAttr("Resource"));
    }
}