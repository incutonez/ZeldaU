using System.Collections;
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
    public static GameObject Inventory { get; set; }

    public void Start()
    {
        StartCoroutine(StartScene());
    }

    public IEnumerator StartScene()
    {
        Inventory = GameObject.Find("Inventory");
        Inventory.SetActive(false);
        CharacterManager = gameObject.AddComponent<CharacterManager>();
        SceneBuilder = gameObject.AddComponent<SceneBuilder>();
        Player = CharacterManager.SpawnPlayer(Constants.STARTING_POSITION, GameObject.Find("Screens").transform);
        Player.gameObject.SetActive(false);
        yield return StartCoroutine(SceneBuilder.LoadScreen("80"));
        ShieldHandler = Player.GetComponentInChildren<ShieldHandler>(true);
        SwordHandler = Player.GetComponentInChildren<SwordHandler>(true);
        SuitHandler = Player.GetComponentInChildren<SuitHandler>(true);
        Inventory.SetActive(true);
        Player.gameObject.SetActive(true);
    }
}
