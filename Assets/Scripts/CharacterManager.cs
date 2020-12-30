using NPCs;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public RectTransform Spawn(Vector3 position, RectTransform prefab, Transform parent)
    {
        RectTransform transform = Instantiate(prefab);
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.rotation = Quaternion.identity;
        return transform;
    }

    public WorldPlayer SpawnPlayer(Vector3 position, Transform parent)
    {
        RectTransform transform = Spawn(position, PrefabsManager.Player, parent);
        WorldPlayer worldCharacter = transform.GetComponent<WorldPlayer>();
        BaseCharacter character = new BaseCharacter { characterType = Characters.Link };
        worldCharacter.SetCharacter(character, null);
        worldCharacter.InitializedCharacter();
        return worldCharacter;
    }

    public void SpawnCharacter(Vector3 position, Characters characterType, Transform parent)
    {
        RectTransform transform = Spawn(position, PrefabsManager.NPC, parent);

        WorldCharacter<BaseCharacter> worldCharacter = transform.GetComponent<WorldCharacter<BaseCharacter>>();
        BaseCharacter character = new BaseCharacter { characterType = characterType };
        worldCharacter.SetCharacter(character, SpritesManager.GetCharacter(character.GetSpriteName()));
    }

    public void SpawnEnemy(Vector3 position, Enemies enemyType, Transform parent)
    {
        RectTransform transform = Spawn(position, PrefabsManager.Enemy, parent);

        WorldEnemy worldCharacter = transform.GetComponent<WorldEnemy>();
        Enemy character = new Enemy { characterType = enemyType };
        worldCharacter.SetCharacter(character, SpritesManager.GetEnemy(character.GetSpriteName()));
    }
}
