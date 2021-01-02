using UnityEngine;

namespace World
{
    public class Character<T> : PlayerBase where T : BaseCharacter
    {
        public T BaseCharacter { get; set; }
        public SpriteRenderer Renderer { get; set; }

        public void SetCharacter(T character, Sprite sprite)
        {
            Renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
            BaseCharacter = character;
            character.Initialize();
            transform.name = character.characterType.GetDescription();
            if (sprite != null)
            {
                Renderer.sprite = sprite;
            }
        }

        public float GetTouchDamage()
        {
            return BaseCharacter.GetTouchDamage();
        }

        public float GetWeaponDamage()
        {
            return BaseCharacter.GetWeaponDamage();
        }

        public float? GetHealth()
        {
            return BaseCharacter.health;
        }

        public float? GetMaxHealth()
        {
            return BaseCharacter.maxHealth;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public T GetCharacter()
        {
            return BaseCharacter;
        }
    }
}
