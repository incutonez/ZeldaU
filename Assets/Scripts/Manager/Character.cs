using NPCs;
using UnityEngine;

namespace Manager
{
    public static class Character
    {
        public static RectTransform Spawn(Vector3 position, RectTransform prefab, Transform parent, bool active = true)
        {
            RectTransform transform = Object.Instantiate(prefab);
            transform.gameObject.SetActive(active);
            transform.SetParent(parent);
            transform.position = position;
            transform.rotation = Quaternion.identity;
            return transform;
        }

        public static World.Player SpawnPlayer(Vector3 position, Transform parent, bool active = false)
        {
            RectTransform transform = Spawn(position, Game.Prefabs.Player, parent, active);
            World.Player worldCharacter = transform.GetComponent<World.Player>();
            worldCharacter.Initialize(Characters.Link);
            return worldCharacter;
        }

        public static void SpawnCharacter(Vector3 position, Characters characterType, Transform parent, bool active = true)
        {
            RectTransform transform = Spawn(position, Game.Prefabs.NPC, parent, active);

            World.Character<Characters> worldCharacter = transform.GetComponent<World.Character<Characters>>();
            worldCharacter.Initialize(characterType);
        }

        public static World.Enemy SpawnEnemy(Vector3 position, Enemies enemyType, Transform parent, bool active = false)
        {
            RectTransform transform = Spawn(position, Game.Prefabs.Enemy, parent, active);
            // We grab the class's type based on the enum name, and the namespace we use for enemies
            World.Enemy enemy = (World.Enemy) transform.gameObject.AddComponent(EnemyHelper.GetEnemyClass(enemyType));
            enemy.Initialize(enemyType);
            return enemy;
        }
    }
}
