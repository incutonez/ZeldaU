using NPCs;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    public void Awake()
    {
        LoadSprites($"{Constants.PATH_SPRITES}characters");
    }

    public static WorldPlayer SpawnPlayer(Vector3 position, Transform parent)
    {
        RectTransform transform = Instantiate(Resources.Load<RectTransform>($"{Constants.PATH_PREFABS}Character"));
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.rotation = Quaternion.identity;

        WorldPlayer worldCharacter = transform.GetComponent<WorldPlayer>();
        BaseCharacter character = new BaseCharacter { characterType = Characters.Link };
        worldCharacter.SetCharacter(character, false);
        worldCharacter.InitializedCharacter();

        return worldCharacter;
    }
}
