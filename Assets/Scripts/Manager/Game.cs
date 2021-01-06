using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// This is the main game handler object... it holds references to "global" variables that we use in other classes
    /// </summary>
    public class Game : MonoBehaviour
    {
        public static event EventHandler OnLaunch;
        public static World.Player Player { get; set; }
        public static Audio Audio { get; set; }
        public static Shield Shield { get; set; }
        public static Sword Sword { get; set; }
        public static Suit Suit { get; set; }
        public static World.Builder Scene { get; set; }
        public static bool IsTransitioning { get; set; }
        public static PlayerInventory Inventory { get; set; }
        public static Dictionary<Tiles, World.TileUVs> TileCoordinates { get; set; }
        public static Canvas MainCanvas { get; set; }
        public static World.Pathfinder Pathfinder { get; set; }
        public static Sprites Sprites { get; set; }
        public static Prefabs Prefabs { get; set; }
        public static bool IsDebugMode { get; set; }
        public bool DebugMode;


        private void Awake()
        {
            IsDebugMode = DebugMode;
            Sprites = new Sprites();
        }

        public void Launch()
        {
            Prefabs = new Prefabs();
            Pathfinder = new World.Pathfinder(Constants.GRID_COLUMNS, Constants.GRID_ROWS);
            TileCoordinates = new Dictionary<Tiles, World.TileUVs>();
            // TODOJEF: Fix these... get actual values from material
            //Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
            int width = 176;
            int height = 116;

            foreach (Sprite sprite in Sprites.Tiles)
            {
                Rect rect = sprite.rect;
                Enum.TryParse(sprite.name, out Tiles tileType);
                if (!TileCoordinates.ContainsKey(tileType))
                {
                    TileCoordinates.Add(tileType, new World.TileUVs
                    {
                        uv00 = new Vector2(rect.min.x / width, rect.min.y / height),
                        uv11 = new Vector2(rect.max.x / width, rect.max.y / height)
                    });
                }
            }
            MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            Audio = gameObject.AddComponent<Audio>();
            Inventory = gameObject.AddComponent<PlayerInventory>();
            Scene = gameObject.AddComponent<World.Builder>();
            Player = Character.SpawnPlayer(Constants.STARTING_POSITION, GameObject.Find("Screens").transform);
            Shield = Player.GetComponentInChildren<Shield>(true);
            Sword = Player.GetComponentInChildren<Sword>(true);
            Suit = Player.GetComponentInChildren<Suit>(true);
            if (!IsDebugMode)
            {
                Scene.BuildScreen(new ViewModel.Grid { Name = "80" });
            }
            Player.gameObject.SetActive(true);
            OnLaunch?.Invoke(this, EventArgs.Empty);
        }
    }
}
