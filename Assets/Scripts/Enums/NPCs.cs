namespace NPCs
{
    public enum Characters
    {
        Fairy = 1,
        [Health(32)]
        Link = 2,
        Merchant = 3,
        OldMan = 4,
        OldWoman = 5,
        Zelda = 6
    }

    public enum Enemies
    {
        /// <summary>
        /// Randomly spawn as very fast or slow
        /// </summary>
        [Damage(1)]
        [Health(6)]
        Armos = 0,
        [Damage(1)]
        [Health()]
        Boulder = 1,
        [Damage()]
        [Health()]
        Bubble = 2,
        [Damage()]
        [Health()]
        BubbleBlue = 3,
        [Damage()]
        [Health()]
        BubbleRed = 4,
        [Damage(2)]
        [Health(8)]
        Darknut = 5,
        [Damage(4)]
        [Health(16)]
        DarknutBlue = 6,
        [Damage(1)]
        [Health(2)]
        Gel = 7,
        /// <summary>
        /// Other health:
        /// You can only destroy the main Ghini... the other ones cannot be attacked
        /// </summary>
        [Damage(1)]
        [Health(22)]
        Ghini = 8,
        [Damage(4)]
        [Health(12, 0.75f)]
        Gibdo = 9,
        // TODO: Verify this damage
        [Damage(2, 2)]
        [Health()]
        GleeokHead = 10,
        [Damage(1, 2)]
        [Health(6)]
        Goriya = 11,
        [Damage(2, 2)]
        [Health(10)]
        GoriyaBlue = 12,
        [Damage(1)]
        [Health(2)]
        Keese = 13,
        [Damage(1)]
        [Health(2)]
        KeeseBlue = 14,
        [Damage(1)]
        [Health(2)]
        KeeseRed = 15,
        [Damage(4)]
        [Health(8, 0)]
        Lanmola = 16,
        /// <summary>
        /// This one moves faster than the red version
        /// </summary>
        [Damage(4)]
        [Health(8, 0)]
        LanmolaBlue = 17,
        [Damage(1)]
        [Health(4)]
        Leever = 18,
        [Damage(2)]
        [Health(8)]
        LeeverBlue = 19,
        /// <summary>
        /// If sucked in, the player will randomly lose their Magical Shield
        /// </summary>
        [Damage(2)]
        [Health(20)]
        LikeLike = 20,
        [Damage(2, 2)]
        [Health(8)]
        Lynel = 21,
        [Damage(4, 4)]
        [Health(12)]
        LynelBlue = 22,
        [Damage(1, 1)]
        [Health(4)]
        Moblin = 23,
        [Damage(1, 1)]
        [Health(6)]
        MoblinBlue = 24,
        [Damage(1)]
        [Health(10, 0)]
        Moldorm = 25,
        [Damage(1, 1)]
        [Health(2)]
        Octorok = 26,
        [Damage(1, 1)]
        [Health(4)]
        OctorokBlue = 27,
        [Damage(4)]
        [Health(20)]
        Patra = 28,
        [Damage(1)]
        [Health(4)]
        Peahat = 29,
        [Damage(4)]
        [Health(20)]
        PolsVoice = 30,
        [Damage(1)]
        [Health(2)]
        Rope = 31,
        [Damage(1)]
        [Health(8)]
        RopeBlue = 32,
        // TODO: Verify the shoot damage of the sword
        [Damage(1/4, 1/4)]
        [Health(4)]
        Stalfos = 33,
        [Damage(1)]
        [Health(2)]
        Tektite = 34,
        [Damage(1)]
        [Health(2)]
        TektiteBlue = 35,
        [Damage(1)]
        [Health()]
        Trap = 36,
        /// <summary>
        /// When it's hit without magical sword, it turns into 2 Keese
        /// </summary>
        [Damage(2)]
        [Health(2)]
        Vire = 37,
        /// <summary>
        /// When this catches player, the player returns to the beginning of the castle
        /// </summary>
        [Damage(1)]
        [Health(6)]
        Wallmaster = 38,
        [Damage(2, 8)]
        [Health(6)]
        Wizzrobe = 39,
        // TODO: Verify the shoot damage
        [Damage(4, 4)]
        [Health(10)]
        WizzrobeBlue = 40,
        [Damage(2)]
        [Health(2)]
        Zol = 41,
        [Damage(1, 1)]
        [Health(4)]
        Zora = 42
    }

    public enum Bosses
    {
        /// <summary>
        /// 1st Quest B: 1 and 7
        /// 2nd Quest B: 1
        /// 2nd Quest E: 4 and 8
        /// 
        /// Other health:
        /// Bombs - 2 hits
        /// Bow - 3 hits
        /// Wand - 3 hits
        /// </summary>
        [Damage(1, 1)]
        [Health(12)]
        Aquamentus,
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
        [Damage(4)]
        [Health(16)]
        Digdogger,
        /// <summary>
        /// 1st Quest B: 2 (total 1)
        /// 1st Quest E: 5 (total 3 x 1 pair) and 7 (total 6 x 2 pairs)
        /// 2nd Quest B: 3 (total 3 x 1 pair) and 8 (total 3 x 1 pair)
        /// 2nd Quest E: 1 (total 1), 4 (total 4 x 1 x 1 pair), 8 (total 9 x 3 pairs)
        /// 
        /// Other health:
        /// If bomb is placed on its back, you can attack with sword for 1 hit kill
        /// </summary>
        [Damage(2)]
        [Health(4, 0)]
        Dodongo,
        /// <summary>
        /// Final Boss
        /// 
        /// Other health:
        /// Must be defeated by silver arrow, and if he's not hit in the stunned state, then he will regain full health
        /// </summary>
        [Damage(4, 2)]
        [Health(32)]
        Ganon,
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
        [Damage(2, 2)]
        [Health(16)]
        Gleeok,
        /// <summary>
        /// Takes 1 arrow to kill
        /// 1st Quest B: 6
        /// 
        /// Other health:
        /// Must have eye open to hurt
        /// </summary>
        [Damage(2, 2)]
        [Health(2, 0)]
        Gohma,
        /// <summary>
        /// Takes 3 arrows to kill
        /// 1st Quest E: 8 (total 2)
        /// 2nd Quest B: 6
        /// 2nd Quest E: 5 and 7
        /// 
        /// Other health:
        /// Must have eye open to hurt
        /// </summary>
        // TODO: Verify damage
        [Damage(2, 2)]
        [Health(6, 0)]
        GohmaBlue,
        /// <summary>
        /// 1st Quest B: 3
        /// 1st Quest E: 4 (total 1) and 8 (total 3)
        /// 2nd Quest E: 2 (total 1), 5 (total 1), 6 (total 1), and 7 (total 2)
        /// 
        /// Other health:
        /// Each tentacle requires that many hits, so it's [4, 2, 1] x 4 tentacles
        /// If a tentacle is hit with 1 bomb, it's destroyed... blast radius could destroy multiple
        /// </summary>
        [Damage(2, 2)]
        [Health(8)]
        Manhandla
    }
}