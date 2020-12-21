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
        sceneBuilder.BuildScene("Screens/test");
        player = CharacterManager.SpawnPlayer(Vector3.zero);
        shieldHandler = player.GetComponentInChildren<ShieldHandler>(true);
        swordHandler = player.GetComponentInChildren<SwordHandler>(true);
        suitHandler = player.GetComponentInChildren<SuitHandler>(true);

        enemyManager.SpawnEnemy(new Vector3(-0.245f, 0.004f), Enemies.Armos);
        enemyManager.SpawnEnemy(new Vector3(-0.45f, 0.004f), Enemies.Gibdo);
        enemyManager.SpawnEnemy(new Vector3(-0.245f, -0.2f), Enemies.Bubble);
        enemyManager.SpawnEnemy(new Vector3(-0.45f, -0.2f), Enemies.Moldorm);
    }
}
