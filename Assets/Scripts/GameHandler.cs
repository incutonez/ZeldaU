using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the main game handler object... it holds references to "global" variables that we use in other classes
/// </summary>
public class GameHandler : MonoBehaviour
{
    public static WorldPlayer Player { get; set; }
    public static ShieldHandler ShieldHandler { get; set; }
    public static SwordHandler SwordHandler { get; set; }
    public static SuitHandler SuitHandler { get; set; }
    public static CharacterManager CharacterManager { get; set; }
    public static SceneBuilder SceneBuilder { get; set; }
    public static bool IsTransitioning { get; set; }
    public static UIInventory Inventory { get; set; }
    public static Dictionary<Tiles, TileUVs> TileCoordinates { get; set; }
    public static Canvas MainCanvas { get; set; }
    public static Pathfinder Pathfinder { get; set; }
    public static bool DebugMode { get; set; } = true;

    private void Awake()
    {
        PrefabsManager.LoadAll();
        SpritesManager.LoadAll();
        Pathfinder = new Pathfinder(Constants.GRID_COLUMNS, Constants.GRID_ROWS);
        TileCoordinates = new Dictionary<Tiles, TileUVs>();
        // TODOJEF: Fix these... get actual values from material
        //Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        int width = 176;
        int height = 116;

        foreach (Sprite sprite in SpritesManager.Tiles)
        {
            Rect rect = sprite.rect;
            Enum.TryParse(sprite.name, out Tiles tileType);
            if (!TileCoordinates.ContainsKey(tileType))
            {
                TileCoordinates.Add(tileType, new TileUVs
                {
                    uv00 = new Vector2(rect.min.x / width, rect.min.y / height),
                    uv11 = new Vector2(rect.max.x / width, rect.max.y / height)
                });
            }
        }
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
    }

    public void Start()
    {
        StartScene();
    }

    public void StartScene()
    {
        Inventory = gameObject.AddComponent<UIInventory>();
        CharacterManager = gameObject.AddComponent<CharacterManager>();
        SceneBuilder = gameObject.AddComponent<SceneBuilder>();
        Player = CharacterManager.SpawnPlayer(Constants.STARTING_POSITION, GameObject.Find("Screens").transform);
        ShieldHandler = Player.GetComponentInChildren<ShieldHandler>(true);
        SwordHandler = Player.GetComponentInChildren<SwordHandler>(true);
        SuitHandler = Player.GetComponentInChildren<SuitHandler>(true);
        if (!DebugMode)
        {
            SceneBuilder.BuildScreen(new SceneViewModel { Name = "80" });
        }
        Player.gameObject.SetActive(true);
    }
}
