namespace Enemy
{
    public class Rock : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 0f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
        }
    }
}