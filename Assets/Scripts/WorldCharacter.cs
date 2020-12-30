using UnityEngine;

public class WorldCharacter<T> : MonoBehaviour where T : BaseCharacter
{
    public T character;
    public SpriteRenderer sRenderer;

    public void SetCharacter(T character, Sprite sprite)
    {
        sRenderer = transform.Find("Body").GetComponent<SpriteRenderer>();
        this.character = character;
        character.Initialize();
        transform.name = character.characterType.GetDescription();
        sRenderer.sprite = sprite;
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
