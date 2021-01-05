using NPCs;
using UnityEngine;

namespace Manager
{
    public static class Character
    {
        public static RectTransform Spawn(Vector3 position, RectTransform prefab, Transform parent)
        {
            RectTransform transform = Object.Instantiate(prefab);
            transform.SetParent(parent);
            transform.localPosition = position;
            transform.rotation = Quaternion.identity;
            return transform;
        }

        public static World.Player SpawnPlayer(Vector3 position, Transform parent)
        {
            RectTransform transform = Spawn(position, Game.Prefabs.Player, parent);
            World.Player worldCharacter = transform.GetComponent<World.Player>();
            BaseCharacter character = new BaseCharacter { characterType = Characters.Link };
            worldCharacter.SetCharacter(character);
            worldCharacter.InitializedCharacter();
            return worldCharacter;
        }

        public static void SpawnCharacter(Vector3 position, Characters characterType, Transform parent)
        {
            RectTransform transform = Spawn(position, Game.Prefabs.NPC, parent);

            World.Character<BaseCharacter> worldCharacter = transform.GetComponent<World.Character<BaseCharacter>>();
            BaseCharacter character = new BaseCharacter { characterType = characterType };
            worldCharacter.SetCharacter(character);
        }

        public static void SpawnEnemy(Vector3 position, Enemies enemyType, Transform parent)
        {
            RectTransform transform = Spawn(position, Game.Prefabs.Enemy, parent);

            World.Enemy worldCharacter = transform.GetComponent<World.Enemy>();
            Enemy character = new Enemy { characterType = enemyType };
            worldCharacter.SetCharacter(character);
        }
    }
}
