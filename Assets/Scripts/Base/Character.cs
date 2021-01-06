using System;

namespace Base
{
    [Serializable]
    public class Character
    {
        public Enum characterType;

        /// <summary>
        /// If this value is null, then that means the enemy cannot take any damage
        /// </summary>
        public float? health { get; set; }
        public float? maxHealth { get; set; }
        public float healthModifier { get; set; }
        public DamageAttribute attackStrength { get; set; }

        public void Initialize()
        {
            SetHealth();
            SetAttackStrength();
        }

        public string GetSpriteName()
        {
            return characterType.GetCustomAttr("Resource");
        }

        public void SetHealth()
        {
            HealthAttribute healthAttribute = characterType.GetAttribute<HealthAttribute>();
            if (healthAttribute != null)
            {
                int baseHealth = healthAttribute.Health;
                if (baseHealth != 0)
                {
                    health = baseHealth;
                    maxHealth = baseHealth;
                    healthModifier = healthAttribute.Modifier;
                }
            }
        }

        public void SetAttackStrength()
        {
            attackStrength = characterType.GetAttribute<DamageAttribute>();
        }

        public float GetTouchDamage()
        {
            return attackStrength?.TouchDamage ?? 0f;
        }

        public float GetWeaponDamage()
        {
            return attackStrength?.WeaponDamage ?? 0f;
        }

        public bool IsDead()
        {
            return health.HasValue && health <= 0;
        }

        public void TakeDamage(float damage, float damageModifier = 0)
        {
            if (health.HasValue)
            {
                // If the modifier is 0, then that mean this enemy constantly loses the same amount, no matter the sword
                if (healthModifier == 0f || damageModifier == 0)
                {
                    health -= damage;
                }
                else
                {
                    health -= damage * damageModifier * healthModifier;
                }
            }
        }

        public void AddHealth(float addedHealth, bool increaseMaxHealth = false)
        {
            health += addedHealth;
            if (increaseMaxHealth && maxHealth != Constants.MAX_HEARTS)
            {
                maxHealth += addedHealth;
            }
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }
}