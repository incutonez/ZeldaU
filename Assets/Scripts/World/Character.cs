using System;
using UnityEngine;

namespace World
{
    public class Character<T> : MonoBehaviour where T : Base.Character
    {
        public event EventHandler OnDestroy;
        public T BaseCharacter { get; set; }
        public SpriteRenderer Renderer { get; set; }
        public Base.Animation Animation { get; set; }
        public Base.AI AI { get; set; }

        private void Awake()
        {
            AI = GetComponent<Base.AI>();
        }

        public void SetCharacter(T character)
        {
            Renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
            BaseCharacter = character;
            character.Initialize();
            transform.name = character.characterType.GetDescription();
            SetAnimationBase();
            Renderer.sprite = Animation.GetDefaultSprite();
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
            OnDestroy?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }

        public T GetCharacter()
        {
            return BaseCharacter;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            AI.Reset();
            gameObject.SetActive(true);
        }
    }
}
