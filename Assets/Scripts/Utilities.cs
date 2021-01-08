using NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utilities
{
    private static Quaternion[] CachedQuaternionEulerArr { get; set; }

    public static float HexToDec(string hex)
    {
        return Convert.ToInt32(hex, 16) / Constants.MAX_RGB;
    }

    public static void SetInterval(Action action, float timeout)
    {
        SetIntervalHook.Create(action, timeout);
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

    /// <summary>
    /// When passing in replaceColors, it needs to be an even length, and it has to be:
    /// even index contains the base color
    /// odd index contains the replacement color
    /// </summary>
    /// <param name="oldSprite"></param>
    /// <param name="replaceColors"></param>
    /// <returns></returns>
    public static Sprite CloneSprite(Sprite oldSprite, Color[] replaceColors = null)
    {
        Texture2D replacement = UnityEngine.Object.Instantiate(oldSprite.texture);
        if (replaceColors != null && replaceColors.Any())
        {
            Color[] colors = replacement.GetPixels();
            // Loops through all of the colors in the sprite's texture
            for (int i = 0; i < colors.Length; i++)
            {
                Color color = colors[i];
                // Check to see if this color matches any replacement colors
                for (int j = 0; j < replaceColors.Length; j += 2)
                {
                    if (replaceColors[j] == color)
                    {
                        colors[i] = replaceColors[j + 1];
                        break;
                    }
                }
            }
            replacement.SetPixels(colors);
            replacement.Apply();
        }
        return Sprite.Create(replacement, oldSprite.rect, Constants.SPRITE_DEFAULT_PIVOT, oldSprite.pixelsPerUnit);
    }

    public static Vector3 GetRandomCoordinates()
    {
        return new Vector3(GetRandomInt(Constants.GRID_COLUMNS), GetRandomInt(Constants.GRID_ROWS));
    }

    public static Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    public static Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Copied from CodeMonkey's MeshUtils
    public static void CreateEmptyMesh(int quadCount, out Vector3[] verts, out Vector2[] uvs, out int[] triangles, out Color[] colors)
    {
        verts = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];
        colors = new Color[verts.Length];
    }

    // Copied from CodeMonkey's MeshUtils and tweaked for colors
    public static void AddToMesh(
        int index,
        World.GridNode tile,
        Vector3[] vertices,
        Vector2[] uvs,
        int[] triangles,
        Color[] colors
    )
    {
        Vector3 baseSize = tile.GetQuadSize();
        Vector3 position = tile.GetWorldPosition();
        float rotation = tile.GetRotation();
        Color color = tile.GetColor();
        tile.GetUVCoordinates(out Vector2 uv00, out Vector2 uv11);

        //Relocate vertices
        int vIndex = index * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;
        baseSize *= 0.5f;

        bool skewed = baseSize.x != baseSize.y;
        if (skewed)
        {
            vertices[vIndex0] = position + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, baseSize.y);
            vertices[vIndex1] = position + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, -baseSize.y);
            vertices[vIndex2] = position + GetQuaternionEuler(rotation) * new Vector3(baseSize.x, -baseSize.y);
            vertices[vIndex3] = position + GetQuaternionEuler(rotation) * baseSize;
        }
        else
        {
            vertices[vIndex0] = position + GetQuaternionEuler(rotation - 270) * baseSize;
            vertices[vIndex1] = position + GetQuaternionEuler(rotation - 180) * baseSize;
            vertices[vIndex2] = position + GetQuaternionEuler(rotation - 90) * baseSize;
            vertices[vIndex3] = position + GetQuaternionEuler(rotation - 0) * baseSize;
        }

        //Relocate UVs
        uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
        uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
        uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
        uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

        // Set vertex colors
        colors[vIndex0] = color;
        colors[vIndex1] = color;
        colors[vIndex2] = color;
        colors[vIndex3] = color;

        //Create triangles
        int tIndex = index * 6;

        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex3;
        triangles[tIndex + 2] = vIndex1;

        triangles[tIndex + 3] = vIndex1;
        triangles[tIndex + 4] = vIndex3;
        triangles[tIndex + 5] = vIndex2;
    }

    // Copied from CodeMonkey's MeshUtils
    private static Quaternion GetQuaternionEuler(float rotationF)
    {
        int rotation = Mathf.RoundToInt(rotationF) % 360;
        if (rotation < 0)
        {
            rotation += 360;
        }
        if (CachedQuaternionEulerArr == null)
        {
            CachedQuaternionEulerArr = new Quaternion[360];
            for (int i = 0; i < CachedQuaternionEulerArr.Length; i++)
            {
                CachedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
            }
        }
        return CachedQuaternionEulerArr[rotation];
    }
}