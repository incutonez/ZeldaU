namespace Boss
{
    /// <summary>
    /// Takes 3 arrows to kill
    /// 1st Quest E: 8 (total 2)
    /// 2nd Quest B: 6
    /// 2nd Quest E: 5 and 7
    /// 
    /// Other health:
    /// Must have eye open to hurt
    /// </summary>
    public class GohmaBlue : Gohma
    {
        public override void SetHealth()
        {
            Health = 6f;
            HealthModifier = 0f;
        }

        public override void SetAttackStrength()
        {
            // TODO: Verify damage
            TouchDamage = 2f;
            WeaponDamage = 2f;
        }
    }
}
