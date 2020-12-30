using UnityEngine;

public static class PrefabsManager
{
    public static RectTransform WorldDoor { get; set; }
    public static RectTransform WorldTransition { get; set; }
    public static RectTransform WorldScreen { get; set; }
    public static RectTransform Enemy { get; set; }
    public static RectTransform NPC { get; set; }
    public static RectTransform Item { get; set; }
    public static RectTransform Player { get; set; }
    public static RectTransform UIHeart { get; set; }

    public static void LoadAll()
    {
        WorldDoor = LoadPrefab("WorldDoor");
        WorldTransition = LoadPrefab("WorldTransition");
        WorldScreen = LoadPrefab("WorldScreen");
        Enemy = LoadPrefab("Enemy");
        NPC = LoadPrefab("NPC");
        Item = LoadPrefab("Item");
        Player = LoadPrefab("Character");
        UIHeart = LoadPrefab("HeartTemplate");
    }

    public static RectTransform LoadPrefab(string name)
    {
        return Resources.Load<RectTransform>($"{Constants.PATH_PREFABS}{name}");
    }
}