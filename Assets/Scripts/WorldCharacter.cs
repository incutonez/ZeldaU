using UnityEngine;

public class WorldCharacter<T> : MonoBehaviour where T : BaseCharacter
{
    public T character;

    private WorldObjectData worldObjectData;

    public void Awake()
    {
        worldObjectData = GetComponent<WorldObjectData>();
    }

    public void SetCharacter(T character)
    {
        this.character = character;
        character.Initialize();
    }

    public void SetCharacter(T character, Sprite sprite)
    {
        this.character = character;
        character.Initialize();
        worldObjectData.SetObjectData(sprite);
    }

    public float GetTouchDamage()
    {
        return character.GetTouchDamage();
    }

    public float GetWeaponDamage()
    {
        return character.GetWeaponDamage();
    }

    public float? GetHealth()
    {
        return character.health;
    }

    public float? GetMaxHealth()
    {
        return character.maxHealth;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public T GetCharacter()
    {
        return character;
    }
}
