namespace Enemy
{
    public class Darknut : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 8f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 2f;
        }
    }
}