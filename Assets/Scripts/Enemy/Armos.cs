namespace Enemy
{
    public class Armos : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 6f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
        }
    }
}