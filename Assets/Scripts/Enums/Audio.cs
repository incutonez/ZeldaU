using System.ComponentModel;

namespace Audio
{
    public enum FX
    {
        [Description("ArrowBoomerang")]
        Arrow,
        BombBlow,
        BombDrop,
        [Description("ArrowBoomerang")]
        Boomerang,
        [Description("BossManhandlaDigdoggerPatra")]
        BossDigdogger,
        [Description("BossDodongoGohma")]
        BossDodongo,
        [Description("BossDragonGanon")]
        BossDragon,
        [Description("BossDragonGanon")]
        BossGanon,
        [Description("BossDodongoGohma")]
        BossGohma,
        BossHurt,
        [Description("BossManhandlaDigdoggerPatra")]
        BossManhandla,
        DoorUnlock,
        EnemyDie,
        EnemyHurt,
        [Description("BossManhandlaDigdoggerPatra")]
        EnemyPatra,
        Fire,
        Flute,
        HealthLow,
        HeartPickup,
        ItemAppear,
        ItemFanfare,
        ItemPickup,
        Magic,
        Rupee,
        SecretAppear,
        Shield,
        Shore,
        Stairs,
        SwordShoot,
        SwordSlash,
        Text
    }

    public enum Music
    {
        Castle,
        Ending,
        FinalCastle,
        GameOver,
        GanonAppear,
        Intro,
        Overworld
    }
}