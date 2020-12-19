using NPCs;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will manage all of the enemies needed for the current screen
/// </summary>
public class EnemyManager : BaseManager<EnemyManager>
{
    private List<WorldEnemy> activeEnemies = new List<WorldEnemy>();

    public void Awake()
    {
        LoadSprites("Sprites/enemies");
        LoadPrefab("Prefabs/Enemy");
    }

    public void SpawnEnemy (Vector3 position, Enemies enemyType)
    {
        activeEnemies.Add(Spawn<WorldEnemy, Enemy, Enemies>(position, enemyType));
    }
}
