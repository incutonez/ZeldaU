namespace NPCs
{
    public enum Characters
    {
        Fairy,
        [Health(6)]
        Link,
        Merchant,
        OldMan,
        OldWoman,
        Zelda
    }

    public enum Enemies
    {
        /// <summary>
        /// Randomly spawn as very fast or slow
        /// </summary>
        [Damage(1)]
        [Health(6)]
        Armos,
        [Damage(1)]
        [Health()]
        Boulder,
        [Damage()]
        [Health()]
        Bubble,
        [Damage()]
        [Health()]
        BubbleBlue,
        [Damage()]
        [Health()]
        BubbleRed,
        [Damage(2)]
        [Health(8)]
        Darknut,
        [Damage(4)]
        [Health(16)]
        DarknutBlue,
        [Damage(1)]
        [Health(2)]
        Gel,
        /// <summary>
        /// Other health:
        /// You can only destroy the main Ghini... the other ones cannot be attacked
        /// </summary>
        [Damage(1)]
        [Health(22)]
        Ghini,
        [Damage(4)]
        [Health(12, 0.75f)]
        Gibdo,
        // TODO: Verify this damage
        [Damage(2, 2)]
        [Health()]
        GleeokHead,
        [Damage(1, 2)]
        [Health(6)]
        Goriya,
        [Damage(2, 2)]
        [Health(10)]
        GoriyaBlue,
        [Damage(1)]
        [Health(2)]
        Keese,
        [Damage(1)]
        [Health(2)]
        KeeseBlue,
        [Damage(1)]
        [Health(2)]
        KeeseRed,
        [Damage(4)]
        [Health(8, 0)]
        Lanmola,
        /// <summary>
        /// This one moves faster than the red version
        /// </summary>
        [Damage(4)]
        [Health(8, 0)]
        LanmolaBlue,
        [Damage(1)]
        [Health(4)]
        Leever,
        [Damage(2)]
        [Health(8)]
        LeeverBlue,
        /// <summary>
        /// If sucked in, the player will randomly lose their Magical Shield
        /// </summary>
        [Damage(2)]
        [Health(20)]
        LikeLike,
        [Damage(2, 2)]
        [Health(8)]
        Lynel,
        [Damage(4, 4)]
        [Health(12)]
        LynelBlue,
        [Damage(1, 1)]
        [Health(4)]
        Moblin,
        [Damage(1, 1)]
        [Health(6)]
        MoblinBlue,
        [Damage(1)]
        [Health(10, 0)]
        Moldorm,
        [Damage(1, 1)]
        [Health(2)]
        Octorok,
        [Damage(1, 1)]
        [Health(4)]
        OctorokBlue,
        [Damage(4)]
        [Health(20)]
        Patra,
        [Damage(1)]
        [Health(4)]
        Peahat,
        [Damage(4)]
        [Health(20)]
        PolsVoice,
        [Damage(1)]
        [Health(2)]
        Rope,
        [Damage(1)]
        [Health(8)]
        RopeBlue,
        // TODO: Verify the shoot damage of the sword
        [Damage(1/4, 1/4)]
        [Health(4)]
        Stalfos,
        [Damage(1)]
        [Health(2)]
        Tektite,
        [Damage(1)]
        [Health(2)]
        TektiteBlue,
        [Damage(1)]
        [Health()]
        Trap,
        /// <summary>
        /// When it's hit without magical sword, it turns into 2 Keese
        /// </summary>
        [Damage(2)]
        [Health(2)]
        Vire,
        /// <summary>
        /// When this catches player, the player returns to the beginning of the castle
        /// </summary>
        [Damage(1)]
        [Health(6)]
        Wallmaster,
        [Damage(2, 8)]
        [Health(6)]
        Wizzrobe,
        // TODO: Verify the shoot damage
        [Damage(4, 4)]
        [Health(10)]
        WizzrobeBlue,
        [Damage(2)]
        [Health(2)]
        Zol,
        [Damage(1, 1)]
        [Health(4)]
        Zora
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