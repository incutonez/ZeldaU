using UnityEngine;

namespace World
{
    public class Character<T> : MonoBehaviour where T : BaseCharacter
    {
        public T BaseCharacter { get; set; }
        public SpriteRenderer Renderer { get; set; }
        public AnimatorBase Animator { get; set; }

        public void SetCharacter(T character)
        {
            Renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
            BaseCharacter = character;
            character.Initialize();
            transform.name = character.characterType.GetDescription();
            SetAnimationBase();
            Renderer.sprite = Animator.GetDefaultSprite();
        }

        public virtual void SetAnimationBase() { }

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
