/// <summary>
/// Each sprite has a Pixel Per Unit (PPU) of 16, so this is the base grid cell we use for the game
/// </summary>
public enum Tiles
{
    None = -2,
    // TODOJEF: Fix these values
    Bush = -1,
    Castle = 0,
    CastleTopAlt = 1,
    CastleTopLeftAlt = 2,
    CastleTopRightAlt = 3,
    CastleBottomLeft = 4,
    CastleBottomRight = 5,
    CastleTop = 6,
    CastleTopLeft = 7,
    CastleTopRight = 8,
    Dock = 9,
    Door = 10,
    Fire = 50,
    FireAlt = 51,
    Grave = 11,
    PondTopRight = 12,
    PondBottom = 13,
    PondBottomLeft = 14,
    PondBottomRight = 15,
    PondCenter = 16,
    PondCenterLeft = 17,
    PondCenterRight = 18,
    PondTop = 19,
    PondTopLeft = 20,
    Rock = 21,
    SandBottom = 22,
    SandBottomLeft = 23,
    SandBottomRight = 24,
    SandCenter = 25,
    SandCenterLeft = 26,
    SandCenterRight = 27,
    SandTop = 28,
    SandTopLeft = 29,
    SandTopRight = 30,
    StairsDown = 31,
    StairsUp = 32,
    Statue = 33,
    TreeBottomLeft = 34,
    TreeBottomRight = 35,
    TreeTop = 36,
    TreeTopLeft = 37,
    TreeTopRight = 38,
    WallBottom = 39,
    WallBottomLeft = 40,
    WallBottomRight = 41,
    WallTop = 42,
    WallTopLeft = 43,
    WallTopRight = 44,
    Water = 45,
    WaterBottomLeft = 46,
    WaterBottomRight = 47,
    WaterTopLeft = 48,
    WaterTopRight = 49,
    // Castle Items
    Block = 50,
    CastleWater = 51,
    DoorClosed = 52,
    DoorLocked = 53,
    DoorUnlocked = 54,
    GroundTile = 55,
    StairsKeep = 56,
    Statue1 = 57,
    Statue2 = 58,
    Wall = 59,
    WallHole = 60,
    WallHorizontalLeft = 61,
    WallHorizontalRight = 62,
    WallKeep = 63,
    WallVerticalBottom = 64,
    WallVerticalTop = 65,
    CastleSand = 66,
    Transition = 100
}