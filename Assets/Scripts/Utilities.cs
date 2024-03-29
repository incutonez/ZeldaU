using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

public static class Utilities {
  private static Quaternion[] CachedQuaternionEulerArr { get; set; }
  private static Dictionary<string, Sprite> SpritesCache { get; } = new();

  private static float HexToDec(string hex) {
    return Convert.ToInt32(hex, 16) / Constants.MaxRGB;
  }

  public static void SetInterval(Action action, float timeout) {
    SetIntervalHook.Create(action, timeout);
  }

  /// <summary>
  /// This method takes in a hex string... meaning it looks like 000000, without the ampersand, and it returns
  /// the result Unity color.
  /// Idea taken from https://www.youtube.com/watch?v=CMGn2giYLc8
  /// </summary>
  /// <param name="hex"></param>
  /// <returns></returns>
  public static Color HexToColor(string hex) {
    return new Color(HexToDec(hex.Substring(0, 2)), HexToDec(hex.Substring(2, 2)), HexToDec(hex.Substring(4, 2)));
  }

  private static int GetRandomInt(int max = 0, int min = 0) {
    return Constants.RandomGenerator.Next(min, max);
  }

  public static Dictionary<Animations, List<Sprite>> ColorAnimations(
    Dictionary<Animations, List<Sprite>> animations,
    Color[] colors
  ) {
    Dictionary<Animations, List<Sprite>> colorAnimations = new();
    foreach (var (key, values) in animations) {
      colorAnimations[key] = new List<Sprite>();
      foreach (Sprite sprite in values) {
        colorAnimations[key].Add(CloneSprite(sprite, colors));
      }
    }

    return colorAnimations;
  }

  /// <summary>
  /// When passing in replaceColors, it needs to be an even length, and it has to be:
  /// even index contains the base color
  /// odd index contains the replacement color
  /// </summary>
  /// <param name="oldSprite"></param>
  /// <param name="replaceColors"></param>
  /// <param name="isTile"></param>
  /// If this is set, then we're going to use our cache of tiles to determine if we need to create a new one
  /// <returns></returns>
  public static Sprite CloneSprite(Sprite oldSprite, Color[] replaceColors = null, bool isTile = false) {
    var doCache = false;
    var key = oldSprite.name;
    if (replaceColors != null) {
      key += string.Join("", replaceColors);
    }

    Texture2D replacement;
    if (isTile) {
      if (SpritesCache.ContainsKey(key)) {
        replacement = UnityEngine.Object.Instantiate(SpritesCache[key].texture);
      }
      else {
        doCache = true;
        replacement = UnityEngine.Object.Instantiate(oldSprite.texture);
      }
    }
    else {
      replacement = UnityEngine.Object.Instantiate(oldSprite.texture);
    }

    if (doCache || !isTile) {
      ReplaceColors(replacement, replaceColors);
    }

    var sprite = Sprite.Create(replacement, oldSprite.rect, Constants.SpriteDefaultPivot, oldSprite.pixelsPerUnit);
    if (doCache) {
      SpritesCache.Add(key, sprite);
    }

    return sprite;
  }

  public static void ReplaceColors(Texture2D texture, Color[] replaceColors = null) {
    if (replaceColors != null && replaceColors.Any()) {
      Color[] colors = texture.GetPixels();
      // Loops through all of the colors in the sprite's texture
      for (int i = 0; i < colors.Length; i++) {
        Color color = colors[i];
        // Check to see if this color matches any replacement colors
        for (int j = 0; j < replaceColors.Length; j += 2) {
          if (replaceColors[j] == color) {
            colors[i] = replaceColors[j + 1];
            break;
          }
        }
      }

      texture.SetPixels(colors);
      texture.Apply();
    }
  }

  // TODO: Remove the code below here?
  public static Vector3 GetRandomCoordinates() {
    return new Vector3(GetRandomInt(Constants.GridColumns), GetRandomInt(Constants.GridRows));
  }

  public static Vector3 GetRandomDir() {
    return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
  }

  public static Vector3 GetMousePosition() {
    return Camera.main.ScreenToWorldPoint(Input.mousePosition);
  }

  // Copied from CodeMonkey's MeshUtils
  public static void CreateEmptyMesh(int quadCount, out Vector3[] verts, out Vector2[] uvs, out int[] triangles, out Color[] colors, out Vector3[] normals) {
    verts = new Vector3[4 * quadCount];
    uvs = new Vector2[verts.Length];
    triangles = new int[6 * quadCount];
    colors = new Color[verts.Length];
    normals = new Vector3[verts.Length];
  }

  // Copied from CodeMonkey's MeshUtils and tweaked for colors
  public static void AddToMesh(
    int index,
    World.GridCell tile,
    Vector3[] vertices,
    Vector2[] uvs,
    int[] triangles,
    Color[] colors,
    Vector3[] normals
  ) {
    Vector3 baseSize = tile.QuadSize;
    Vector3 position = tile.WorldPosition;
    float rotation = tile.Rotation;
    Vector2 uv00 = tile.UV00;
    Vector2 uv11 = tile.UV11;
    // TODO: How does this work if we have multiple colors?
    Color? color = tile.GetColor();

    //Relocate vertices
    int vIndex = index * 4;
    int vIndex0 = vIndex;
    int vIndex1 = vIndex + 1;
    int vIndex2 = vIndex + 2;
    int vIndex3 = vIndex + 3;
    baseSize *= 0.5f;

    int xMod = tile.FlipX ? -1 : 1;
    int yMod = tile.FlipY ? -1 : 1;
    // TODO: This really is just -0.5, 0.5... could cache it
    vertices[vIndex0] = position + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x * xMod, baseSize.y * yMod);
    vertices[vIndex1] = position + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x * xMod, -baseSize.y * yMod);
    vertices[vIndex2] = position + GetQuaternionEuler(rotation) * new Vector3(baseSize.x * xMod, -baseSize.y * yMod);
    vertices[vIndex3] = position + GetQuaternionEuler(rotation) * new Vector3(baseSize.x * xMod, baseSize.y * yMod);

    //Relocate UVs
    uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
    uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
    uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
    uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

    normals[vIndex0] = Vector3.up;
    normals[vIndex1] = Vector3.up;
    normals[vIndex2] = Vector3.up;
    normals[vIndex3] = Vector3.up;

    if (color.HasValue) {
      // Set vertex colors
      colors[vIndex0] = color.Value;
      colors[vIndex1] = color.Value;
      colors[vIndex2] = color.Value;
      colors[vIndex3] = color.Value;
    }

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
  private static Quaternion GetQuaternionEuler(float rotationF) {
    int rotation = Mathf.RoundToInt(rotationF) % 360;
    if (rotation < 0) {
      rotation += 360;
    }

    if (CachedQuaternionEulerArr == null) {
      CachedQuaternionEulerArr = new Quaternion[360];
      for (int i = 0; i < CachedQuaternionEulerArr.Length; i++) {
        CachedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
      }
    }

    return CachedQuaternionEulerArr[rotation];
  }
}
