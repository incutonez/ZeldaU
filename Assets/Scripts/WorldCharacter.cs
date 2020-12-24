using UnityEngine;

public class WorldCharacter<T> : MonoBehaviour where T : BaseCharacter
{
    public T character;
    public WorldObjectData worldObjectData;

    public void Awake()
    {
        worldObjectData = GetComponent<WorldObjectData>();
    }

    public void SetCharacter(T character, bool updateCollider = true)
    {
        this.character = character;
        character.Initialize();
        transform.name = character.characterType.GetDescription();
        if (updateCollider)
        {
            // TODOJEF: COME BACK TO THIS
            //worldObjectData.UpdatePolygonCollider2D();
        }
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
