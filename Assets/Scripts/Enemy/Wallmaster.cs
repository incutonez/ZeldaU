namespace Enemy
{
    /// <summary>
    /// When this catches player, the player returns to the beginning of the castle
    /// </summary>
    public class Wallmaster : World.Enemy
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