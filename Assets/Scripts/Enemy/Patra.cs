namespace Enemy
{
    public class Patra : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 20f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 4f;
        }
    }
}