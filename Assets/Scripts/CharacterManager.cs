using NPCs;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    public void Awake()
    {
        LoadSprites($"{Constants.PATH_SPRITES}characters");
    }

    public static WorldPlayer SpawnPlayer(Vector3 position)
    {
        RectTransform transform = Instantiate(Resources.Load<RectTransform>($"{Constants.PATH_PREFABS}Character"), position, Quaternion.identity);

        WorldPlayer worldCharacter = transform.GetComponent<WorldPlayer>();
        BaseCharacter character = new BaseCharacter { characterType = Characters.Link };
        worldCharacter.SetCharacter(character);
        worldCharacter.InitializedCharacter();

        return worldCharacter;
    }
}
