namespace Boss
{
    /// <summary>
    /// 1st Quest B: 1 and 7
    /// 2nd Quest B: 1
    /// 2nd Quest E: 4 and 8
    /// 
    /// Other health:
    /// Bombs - 2 hits
    /// Bow - 3 hits
    /// Wand - 3 hits
    /// </summary>
    public class Aquamentus : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 12f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
            WeaponDamage = 1f;
        }
    }
}
