namespace Enemy
{
    public class WizzrobeBlue : Wizzrobe
    {
        public override void SetHealth()
        {
            Health = 10f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 4f;
            // TODO: Verify the shoot damage
            WeaponDamage = 4f;
        }
    }
}