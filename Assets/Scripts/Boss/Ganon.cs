namespace Boss
{
    /// <summary>
    /// Final Boss
    /// 
    /// Other health:
    /// Must be defeated by silver arrow, and if he's not hit in the stunned state, then he will regain full health
    /// </summary>
    public class Ganon : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 32f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 4f;
            WeaponDamage = 2f;
        }
    }
}
