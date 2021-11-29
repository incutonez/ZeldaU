using UnityEngine;

public static class Constants {
  public const string PathSprites = "Sprites/";
  public const string PathMaterials = "Materials/";
  public const string PathPrefabs = "Prefabs/";
  public const string PathOverworld = "Overworld_";
  public const string PathCastle = "Castles_";
  public const string TransitionBack = "Back";
  public const string PlayerTransition = "TransitionCollider";
  public const string CountContainerRef = "CountContainer";

  public static Items[] SelectableItems { get; set; } = new Items[] {
    Items.Boomerang,
    Items.Bomb,
    Items.Bow,
    Items.Candle,
    Items.Flute,
    Items.Food,
    Items.PotionBlue,
    Items.Wand
  };

  public static readonly System.Random RandomGenerator = new System.Random();

  /// <summary>
  /// This represents the number of cells horizontally.
  /// </summary>
  public const int GridColumns = 16;

  /// <summary>
  /// This represents the number of cells vertically.
  /// </summary>
  public const int GridRows = 11;

  /// <summary>
  /// This represents the number of cells horizontally, but the 0-based version
  /// </summary>
  public const int GridColumnsZero = GridColumns - 1;

  /// <summary>
  /// This represents the number of cells vertically, but the 0-based version
  /// </summary>
  public const int GridRowsZero = GridRows - 1;

  public const float GridCellSize = 1f;
  public static readonly Vector3 GridOrigin = new Vector3(-8f, -7.5f);

  public const float MaxRGB = 255f;

  public const float AttackDelay = 0.2f;
  public const float AttackDelayThreshold = 0f;
  public const float AttackLength = 0.3f;

  /// <summary>
  /// The green sword is considered the base attack power for the game... it's 2 half hearts, so 1 whole heart is its damage
  /// Each sword then has a modifier based on this value... White Sword's modifier is 2x, and Magical Sword is 4x
  /// </summary>
  public const float BaseSwordPower = 2f;

  /// <summary>
  /// This is the amount of health a single heart will refill... it's 2 half hearts
  /// </summary>
  public const float HeartRefill = 2f;

  public const int MaxHearts = 32;

  public const int MaxRupees = 255;

  // We don't make this a const or readonly because it can increase to 16
  public static int MaxBombs = 8;

  public static string StartingTile = "";
  public static int[] StartingTiles = new int[] {7, 0};

  public static readonly Color ColorInvisible = new Color(1f, 1f, 1f, 0f);

  public static readonly Vector3 StartingPosition = new Vector3(-1, -2);
  public static readonly Vector2 SpriteDefaultPivot = new Vector2(0.5f, 0.5f);

  public static readonly Vector3 SwordRight = new Vector3(1.2f, 0.4f);
  public static readonly Vector3 SwordColliderPositive = new Vector3(0f, 0.156f);
  public static readonly Vector3 SwordLeft = new Vector3(-0.2f, 0.4f);
  public static readonly Vector3 SwordUp = new Vector3(0.407f, 1.25f);
  public static readonly Vector3 SwordDown = new Vector3(0.5937f, -0.2189f);
  public static readonly Vector3 SwordColliderNegative = new Vector3(0f, -0.156f);
  public static readonly Quaternion SwordXRotation = Quaternion.Euler(new Vector3(0, 0, -90));
  public static readonly Quaternion SwordYRotation = Quaternion.Euler(Vector3.zero);
}