namespace Enemy
{
    public class MoblinBlue : Moblin
    {
        public override void SetHealth()
        {
            Health = 6f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
            WeaponDamage = 1f;
        }
    }
}