namespace Enemy
{
    public class Goriya : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 6f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
            WeaponDamage = 2f;
        }
    }
}