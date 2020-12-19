﻿using UnityEngine;

public static class Constants
{
    public const float MAX_RGB = 255;

    public const float ATTACK_DELAY = 0.2f;
    public const float ATTACK_DELAY_THRESHOLD = 0f;
    public const float ATTACK_LENGTH = 0.3f;

    /// <summary>
    /// The green sword is considered the base attack power for the game... it's 2 half hearts, so 1 whole heart is its damage
    /// Each sword then has a modifier based on this value... White Sword's modifier is 2x, and Magical Sword is 4x
    /// </summary>
    public const int BASE_SWORD_POWER = 2;

    public static readonly Vector3 SWORD_RIGHT = new Vector3(0.11f, -0.01f, 0f);
    public static readonly Vector3 SWORD_LEFT = new Vector3(-0.11f, -0.01f, 0f);
    public static readonly Vector3 SWORD_UP = new Vector3(-0.017f, 0.1f, 0f);
    public static readonly Vector3 SWORD_DOWN = new Vector3(0.01f, -0.113f, 0f);
    public static readonly Quaternion SWORD_X_ROTATION = Quaternion.Euler(new Vector3(0, 0, -90));
    public static readonly Quaternion SWORD_Y_ROTATION = Quaternion.Euler(new Vector3(0, 0, 0));
}