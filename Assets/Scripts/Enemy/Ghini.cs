namespace Enemy
{
    /// <summary>
    /// You can only destroy the main Ghini... the other ones cannot be attacked
    /// </summary>
    public class Ghini : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 22f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
        }
    }
}