namespace Enemy
{
    public class Gibdo : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 12f;
            HealthModifier = 0.75f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 4f;
        }
    }
}