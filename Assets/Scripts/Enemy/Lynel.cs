namespace Enemy
{
    public class Lynel : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 8f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 2f;
            WeaponDamage = 2f;
        }
    }
}