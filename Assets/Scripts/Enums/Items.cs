public enum Items
{
    None = 0,
    [Damage(0, 4)]
    Arrow = 1,
    [Damage(0, 8)]
    ArrowSilver = 2,
    [Damage(0, 8)]
    Bomb = 3,
    Boomerang = 4,
    BoomerangMagical = 5,
    Bow = 6,
    [Damage(0, 2)]
    Candle = 7,
    [Damage(0, 2)]
    CandleRed = 8,
    Clock = 9,
    Compass = 10,
    Flute = 11,
    Food = 12,
    Heart = 13,
    HeartHalf = 130,
    HeartEmpty = 131,
    HeartAlt = 133,
    HeartContainer = 14,
    Key = 15,
    KeySkeleton = 16,
    Ladder = 17,
    Letter = 18,
    MagicBook = 19,
    Map = 20,
    PotionBlue = 21,
    PotionRed = 22,
    PowerBracelet = 23,
    // Raft changes colors based on suit
    Raft = 24,
    [Damage(0.5f)]
    RingBlue = 25,
    [Damage(1f)]
    RingGreen = 26,
    [Damage(0.25f)]
    RingRed = 27,
    RupeeFive = 28,
    RupeeOne = 29,
    // On character sprite
    Shield = 30,
    // On character sprite
    ShieldMagical = 31,
    [Damage()]
    Sword = 32,
    [Damage(0, 2)]
    SwordWhite = 33,
    [Damage(0, 4)]
    SwordMagical = 34,
    // Split between Shard1 and 2
    TriforceShard = 35,
    // Split between Shard1 and 2
    TriforceShardAlt = 355,
    [Damage(0, 4)]
    Wand = 36
}