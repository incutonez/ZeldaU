namespace Boss
{
    /// <summary>
    /// 1st Quest B: 5
    /// 1st Quest E: 7 (total 7 x 1 x 2 pairs of 3)
    /// 2nd Quest B: 4 (total 3 x 1 pair)
    /// 2nd Quest E: 4 (total 3 x 1 pair) and 8 (total 6 x 2 pairs)
    /// 
    /// Other health:
    /// First requires Flute to be played
    /// The health listed here is for individual Digdogger heads
    /// Bombs kill in 2 hits, same as magical sword
    /// Bow and Wand kill in 4 hits, same as white sword
    /// </summary>
    public class Digdogger : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 16f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 4f;
        }
    }
}
