using System;
using System.Collections;
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
        public static Canvas MainCanvas { get; set; }
        public static World.Pathfinder Pathfinder { get; set; }
        public static Graphics Graphics { get; set; }
        public static bool IsDebugMode { get; set; }
        public bool DebugMode;


        private void Awake()
        {
            IsDebugMode = DebugMode;
            Graphics = new Graphics();
            Audio = gameObject.AddComponent<Audio>();
        }

        public void StartLaunch()
        {
            StartCoroutine(Launch());
        }

        public IEnumerator Launch()
        {
            Pathfinder = new World.Pathfinder(Constants.GRID_COLUMNS, Constants.GRID_ROWS);
            MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            Inventory = gameObject.AddComponent<PlayerInventory>();
            Scene = gameObject.AddComponent<World.Builder>();
            Player = Character.SpawnPlayer(Constants.STARTING_POSITION, GameObject.Find("Screens").transform);
            Shield = Player.GetComponentInChildren<Shield>(true);
            Sword = Player.GetComponentInChildren<Sword>(true);
            Suit = Player.GetComponentInChildren<Suit>(true);
            if (!IsDebugMode)
            {
                yield return Scene.BuildScreen(new ViewModel.Grid { Name = "80" });
            }
            Player.gameObject.SetActive(true);
            OnLaunch?.Invoke(this, EventArgs.Empty);
        }
    }
}
