using UnityEngine;

public static class Constants
{
    public const string PATH_SPRITES = "Sprites/";
    public const string PATH_PREFABS = "Prefabs/";
    public const string PATH_DATA = "Data/";
    public const string PATH_OVERWORLD = PATH_DATA + "Overworld/";

    public const float MAX_RGB = 255;

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

    public const float PPU = 100f;
    public const float MATTER_SIZE = 16f;

    public static readonly Color COLOR_INVISIBLE = new Color(1f, 1f, 1f, 0f);

    public static readonly Vector3 SWORD_RIGHT = new Vector3(0.11f, -0.01f, 0f);
    public static readonly Vector3 SWORD_LEFT = new Vector3(-0.11f, -0.01f, 0f);
    public static readonly Vector3 SWORD_UP = new Vector3(-0.017f, 0.1f, 0f);
    public static readonly Vector3 SWORD_DOWN = new Vector3(0.01f, -0.113f, 0f);
    public static readonly Quaternion SWORD_X_ROTATION = Quaternion.Euler(new Vector3(0, 0, -90));
    public static readonly Quaternion SWORD_Y_ROTATION = Quaternion.Euler(Vector3.zero);
}