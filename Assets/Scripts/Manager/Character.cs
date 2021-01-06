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
            Base.Character character = new Base.Character { characterType = Characters.Link };
            worldCharacter.SetCharacter(character);
            worldCharacter.InitializedCharacter();
            return worldCharacter;
        }

        public static void SpawnCharacter(Vector3 position, Characters characterType, Transform parent, bool active = true)
        {
            RectTransform transform = Spawn(position, Game.Prefabs.NPC, parent, active);

            World.Character<Base.Character> worldCharacter = transform.GetComponent<World.Character<Base.Character>>();
            Base.Character character = new Base.Character { characterType = characterType };
            worldCharacter.SetCharacter(character);
        }

        public static World.Enemy SpawnEnemy(Vector3 position, Enemies enemyType, Transform parent, bool active = false)
        {
            RectTransform transform = Spawn(position, Game.Prefabs.Enemy, parent, active);

            World.Enemy worldCharacter = transform.GetComponent<World.Enemy>();
            Base.Enemy character = new Base.Enemy { characterType = enemyType };
            worldCharacter.SetCharacter(character);
            return worldCharacter;
        }
    }
}
