using UnityEngine;

public static class Constants
{
    public const string PATH_SPRITES = "Sprites/";
    public const string PATH_MATERIALS = "Materials/";
    public const string PATH_PREFABS = "Prefabs/";
    public const string PATH_OVERWORLD = "Overworld/";
    public const string PATH_CASTLE = "Castles/";
    public const string TRANSITION_BACK = "Back";
    public const string PLAYER_TRANSITION = "TransitionCollider";
    public const string COUNT_CONTAINER_REF = "CountContainer";
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

    public static readonly System.Random RANDOM_GENERATOR = new System.Random();

    /// <summary>
    /// This represents the number of cells horizontally.
    /// </summary>
    public const int GRID_COLUMNS = 16;
    /// <summary>
    /// This represents the number of cells vertically.
    /// </summary>
    public const int GRID_ROWS = 11;
    /// <summary>
    /// This represents the number of cells horizontally, but the 0-based version
    /// </summary>
    public const int GRID_COLUMNS_ZERO = GRID_COLUMNS - 1;
    /// <summary>
    /// This represents the number of cells vertically, but the 0-based version
    /// </summary>
    public const int GRID_ROWS_ZERO = GRID_ROWS - 1;
    public const float GRID_CELL_SIZE = 1f;
    public static readonly Vector3 GRID_ORIGIN = new Vector3(-8f, -7.5f);

    public const float MAX_RGB = 255f;

    public const float ATTACK_DELAY = 0.2f;
    public const float ATTACK_DELAY_THRESHOLD = 0f;
    public const float ATTACK_LENGTH = 0.3f;

    /// <summary>
    /// The green sword is considered the base attack power for the game... it's 2 half hearts, so 1 whole heart is its damage
    /// Each sword then has a modifier based on this value... White Sword's modifier is 2x, and Magical Sword is 4x
    /// </summary>
    public const float BASE_SWORD_POWER = 2f;

    /// <summary>
    /// This is the amount of health a single heart will refill... it's 2 half hearts
    /// </summary>
    public const float HEART_REFILL = 2f;
    public const int MAX_HEARTS = 32;
    public const int MAX_RUPEES = 255;
    // We don't make this a const or readonly because it can increase to 16
    public static int MAX_BOMBS = 8;

    public static readonly Color COLOR_INVISIBLE = new Color(1f, 1f, 1f, 0f);

    public static readonly Vector3 STARTING_POSITION = new Vector3(-1, -2);
    public static readonly Vector2 SPRITE_DEFAULT_PIVOT = new Vector2(0.5f, 0.5f);

    public static readonly Vector3 SWORD_RIGHT = new Vector3(1.2f, 0.4f);
    public static readonly Vector3 SWORD_COLLIDER_POSITIVE = new Vector3(0f, 0.156f);
    public static readonly Vector3 SWORD_LEFT = new Vector3(-0.2f, 0.4f);
    public static readonly Vector3 SWORD_UP = new Vector3(0.407f, 1.25f);
    public static readonly Vector3 SWORD_DOWN = new Vector3(0.5937f, -0.2189f);
    public static readonly Vector3 SWORD_COLLIDER_NEGATIVE = new Vector3(0f, -0.156f);
    public static readonly Quaternion SWORD_X_ROTATION = Quaternion.Euler(new Vector3(0, 0, -90));
    public static readonly Quaternion SWORD_Y_ROTATION = Quaternion.Euler(Vector3.zero);
}