namespace NPCs
{
    public enum Characters
    {
        Fairy = 1,
        Link = 2,
        Merchant = 3,
        OldMan = 4,
        OldMan2 = 5,
        OldWoman = 6,
        Zelda = 7
    }

    public enum Enemies
    {
        /// <summary>
        /// Randomly spawn as very fast or slow
        /// </summary>
        [EnemyClass(typeof(Enemy.Armos))]
        Armos = 0,
        Rock = 1,
        Bubble = 2,
        BubbleBlue = 3,
        BubbleRed = 4,
        Darknut = 5,
        DarknutBlue = 6,
        Gel = 7,
        Ghini = 8,
        Gibdo = 9,
        GleeokHead = 10,
        Goriya = 11,
        GoriyaBlue = 12,
        Keese = 13,
        KeeseBlue = 14,
        KeeseRed = 15,
        Lanmola = 16,
        LanmolaBlue = 17,
        Leever = 18,
        LeeverBlue = 19,
        LikeLike = 20,
        Lynel = 21,
        LynelBlue = 22,
        Moblin = 23,
        MoblinBlue = 24,
        Moldorm = 25,
        [EnemyClass(typeof(Enemy.Octorok))]
        Octorok = 26,
        [EnemyClass(typeof(Enemy.OctorokBlue))]
        OctorokBlue = 27,
        Patra = 28,
        Peahat = 29,
        PolsVoice = 30,
        Rope = 31,
        RopeBlue = 32,
        Stalfos = 33,
        Tektite = 34,
        TektiteBlue = 35,
        Trap = 36,
        Vire = 37,
        Wallmaster = 38,
        Wizzrobe = 39,
        WizzrobeBlue = 40,
        Zol = 41,
        Zora = 42
    }

    public enum Bosses
    {
        Aquamentus,
        Digdogger,
        Dodongo,
        Ganon,
        Gleeok,
        Gohma,
        GohmaBlue,
        Manhandla
    }
}