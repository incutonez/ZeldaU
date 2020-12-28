using System;
using UnityEngine;

public static class Utilities
{
    public static float HexToDec(string hex)
    {
        return Convert.ToInt32(hex, 16) / Constants.MAX_RGB;
    }

    /// <summary>
    /// This method takes in a hex string... meaning it looks like 000000, without the ampersand, and it returns
    /// the result Unity color.
    /// Idea taken from https://www.youtube.com/watch?v=CMGn2giYLc8
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color HexToColor(string hex)
    {
        return new Color(HexToDec(hex.Substring(0, 2)), HexToDec(hex.Substring(2, 2)), HexToDec(hex.Substring(4, 2)));
    }

    public static int GetRandomInt(int max = 0, int min = 0)
    {
        return Constants.RANDOM_GENERATOR.Next(min, max);
    }

    public static Vector3 GetRandomCoordinates()
    {
        return new Vector3(GetRandomInt(Constants.GRID_COLUMNS), GetRandomInt(Constants.GRID_ROWS));
    }
}