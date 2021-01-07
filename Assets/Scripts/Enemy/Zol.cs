namespace Enemy
{
    /// <summary>
    /// If hit with the regular sword, it breaks into 2 gels
    /// </summary>
    public class Zol : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 2f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 2f;
        }
    }
}