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
        public float? Health { get; set; }
        public float? MaxHealth { get; set; }
        public float HealthModifier { get; set; }
        public DamageAttribute AttackStrength { get; set; }
        public SpriteRenderer Renderer { get; set; }
        public Base.Animation Animation { get; set; }
        public Base.AI AI { get; set; }

        private void Awake()
        {
            AI = GetComponent<Base.AI>();
        }

        public virtual void Initialize(TEnum characterType)
        {
            Renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
            CharacterType = characterType;
            SetHealth();
            SetAttackStrength();
            transform.name = CharacterType.GetDescription();
            SetAnimationBase();
            Renderer.sprite = Animation.GetDefaultSprite();
        }

        public virtual void SetAnimationBase() { }

        public float? GetHealth()
        {
            return Health;
        }

        public float? GetMaxHealth()
        {
            return MaxHealth;
        }

        public void SetHealth()
        {
            HealthAttribute healthAttribute = CharacterType.GetAttribute<HealthAttribute>();
            if (healthAttribute != null)
            {
                int baseHealth = healthAttribute.Health;
                if (baseHealth != 0)
                {
                    Health = baseHealth;
                    MaxHealth = baseHealth;
                    HealthModifier = healthAttribute.Modifier;
                }
            }
        }

        public void SetAttackStrength()
        {
            AttackStrength = CharacterType.GetAttribute<DamageAttribute>();
        }

        public float GetTouchDamage()
        {
            return AttackStrength?.TouchDamage ?? 0f;
        }

        public float GetWeaponDamage()
        {
            return AttackStrength?.WeaponDamage ?? 0f;
        }

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
            if (increaseMaxHealth && MaxHealth != Constants.MAX_HEARTS)
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

        public void Enable()
        {
            AI.Reset();
            gameObject.SetActive(true);
        }
    }
}
