namespace Enemy
{
    public class PolsVoice : World.Enemy
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