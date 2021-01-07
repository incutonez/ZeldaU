namespace Enemy
{
    public class LynelBlue : Lynel
    {
        public override void SetHealth()
        {
            Health = 12f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 4f;
            WeaponDamage = 4f;
        }
    }
}