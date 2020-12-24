using UnityEngine;

public static class Constants
{
    public const string PATH_SPRITES = "Sprites/";
    public const string PATH_PREFABS = "Prefabs/";
    public const string PATH_DATA = "Data/";
    public const string PATH_OVERWORLD = PATH_DATA + "Overworld/";

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

    public static readonly Color COLOR_INVISIBLE = new Color(1f, 1f, 1f, 0f);

    public static readonly Vector3 STARTING_POSITION = new Vector3(7, 5);

    public static readonly Vector3 SWORD_RIGHT = new Vector3(0.11f, -0.01f, 0f);
    public static readonly Vector3 SWORD_LEFT = new Vector3(-0.11f, -0.01f, 0f);
    public static readonly Vector3 SWORD_UP = new Vector3(-0.017f, 0.1f, 0f);
    public static readonly Vector3 SWORD_DOWN = new Vector3(0.01f, -0.113f, 0f);
    public static readonly Quaternion SWORD_X_ROTATION = Quaternion.Euler(new Vector3(0, 0, -90));
    public static readonly Quaternion SWORD_Y_ROTATION = Quaternion.Euler(Vector3.zero);
}