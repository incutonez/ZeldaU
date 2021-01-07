namespace Enemy
{
    public class Octorok : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 2f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
            WeaponDamage = 1f;
        }

        public override void SetFrameRates()
        {
            Animation.ActionFrameRate = 0.3f;
            Animation.IdleFrameRate = 0.33f;
            Animation.WalkFrameRate = 1f;
        }
    }
}
