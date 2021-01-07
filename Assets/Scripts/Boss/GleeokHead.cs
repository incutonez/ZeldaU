namespace Boss
{
    public class GleeokHead : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 0f;
        }

        public override void SetAttackStrength()
        {
            // TODO: Verify this damage
            TouchDamage = 2f;
            WeaponDamage = 2f;
        }
    }
}
