namespace Enemy
{
    public class LeeverBlue : Leever
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