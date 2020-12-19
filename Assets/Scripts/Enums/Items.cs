public enum Items
{
    [Damage(0, 4)]
    Arrow,
    [Damage(0, 8)]
    ArrowSilver,
    [Damage(0, 8)]
    Bomb,
    Boomerang,
    BoomerangMagical,
    Bow,
    [Damage(0, 2)]
    Candle,
    [Damage(0, 2)]
    CandleRed,
    Clock,
    Compass,
    Flute,
    Food,
    Heart,
    HeartContainer,
    Key,
    KeySkeleton,
    Ladder,
    // Missing
    Letter,
    MagicBook,
    Map,
    PotionBlue,
    PotionRed,
    PowerBracelet,
    // Raft changes colors based on suit
    Raft,
    // Cuts damage by 1/2
    RingBlue,
    RingGreen,
    // Cuts damage by 1/4
    RingRed,
    RupeeFive,
    RupeeOne,
    // On character sprite
    Shield,
    // On character sprite
    ShieldMagical,
    [Damage()]
    Sword,
    [Damage(0, 2)]
    SwordWhite,
    [Damage(0, 4)]
    SwordMagical,
    // Split between Shard1 and 2
    TriforceShard,
    [Damage(0, 4)]
    Wand
}