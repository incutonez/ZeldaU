using System;
using UnityEngine;

namespace World
{
    public class Character<TEnum> : MonoBehaviour where TEnum : Enum
    {
        public event EventHandler OnDestroy;
        public TEnum CharacterType { get; set; }
        /// <summary>
        /// If this value is null, then that means the enemy cannot take any damage
        /// </summary>
        public virtual float? Health { get; set; }
        public virtual float? MaxHealth { get; set; }
        public virtual float HealthModifier { get; set; } = 1f;
        public virtual float TouchDamage { get; set; } = 0f;
        public virtual float WeaponDamage { get; set; } = 0f;
        public SpriteRenderer Renderer { get; set; }
        public Base.Animation Animation { get; set; }

        // TODOJEF: Potentially add ability to pass in a config?
        public virtual void Initialize(TEnum characterType)
        {
            Renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
            CharacterType = characterType;
            SetHealth();
            SetMaxHealth();
            SetAttackStrength();
            transform.name = CharacterType.GetDescription();
            SetAnimationBase();
            Renderer.sprite = Animation.GetDefaultSprite();
        }

        public virtual void SetAnimationBase() { }

        public virtual void SetHealth() { }

        public virtual void SetMaxHealth()
        {
            MaxHealth = Health;
        }

        public virtual void SetAttackStrength() { }

        public bool IsDead()
        {
            return Health.HasValue && Health <= 0;
        }

        public void TakeDamage(float damage, float damageModifier = 0)
        {
            if (Health.HasValue)
            {
                // If the modifier is 0, then that mean this enemy constantly loses the same amount, no matter the sword
                if (HealthModifier == 0f || damageModifier == 0)
                {
                    Health -= damage;
                }
                else
                {
                    Health -= damage * damageModifier * HealthModifier;
                }
            }
        }

        public void AddHealth(float addedHealth, bool increaseMaxHealth = false)
        {
            Health += addedHealth;
            if (increaseMaxHealth && MaxHealth != Constants.MaxHearts)
            {
                MaxHealth += addedHealth;
            }
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        public void DestroySelf()
        {
            OnDestroy?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}
