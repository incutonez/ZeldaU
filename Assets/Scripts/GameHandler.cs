using NPCs;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static WorldPlayer player;
    public static ShieldHandler shieldHandler;
    public static SwordHandler swordHandler;
    public static SuitHandler suitHandler;
    public static EnemyManager enemyManager;
    public static SceneBuilder sceneBuilder;

    public void Start()
    {
        enemyManager = gameObject.AddComponent<EnemyManager>();
        sceneBuilder = gameObject.AddComponent<SceneBuilder>();
        sceneBuilder.BuildScene(8, 0);
        player = CharacterManager.SpawnPlayer(Vector3.zero);
        shieldHandler = player.GetComponentInChildren<ShieldHandler>(true);
        swordHandler = player.GetComponentInChildren<SwordHandler>(true);
        suitHandler = player.GetComponentInChildren<SuitHandler>(true);

        enemyManager.SpawnEnemy(new Vector3(0, 0), Enemies.Armos);
    }
}
