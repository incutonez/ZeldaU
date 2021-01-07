namespace Boss
{
    /// <summary>
    /// Takes 1 arrow to kill
    /// 1st Quest B: 6
    /// 
    /// Other health:
    /// Must have eye open to hurt
    /// </summary>
    public class Gohma : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 2f;
            HealthModifier = 0f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 2f;
            WeaponDamage = 2f;
        }
    }
}
