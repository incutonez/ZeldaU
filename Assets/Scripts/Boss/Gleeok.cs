namespace Boss
{
    /// <summary>
    /// 1st Quest B: 4 (2 heads) and 8 (4 heads)
    /// 1st Quest E: 6 (3 heads)
    /// 2nd Quest B: 2 (2 heads), 5 (3 heads), and 7 (4 heads)
    /// 2nd Quest E: 6 (2 heads)
    /// 
    /// Other health:
    /// The health shown here is for each head
    /// Wand will take out head in 4 hits... same damage as white sword
    /// The Magical Shield cannot block its fireballs
    /// </summary>
    public class Gleeok : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 16f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 2f;
            WeaponDamage = 2f;
        }
    }
}
