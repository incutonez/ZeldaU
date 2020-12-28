using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Taken from https://www.youtube.com/watch?v=mZzZXfySeFQ
/// </summary>
public class ScreenTileVisual : MonoBehaviour
{
    [Serializable]
    public struct MatterUV
    {
        public Matters MatterType;
        public Vector2Int UV00Pixels;
        public Vector2Int UV11Pixels;
    }

    private struct MatterCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    private Dictionary<Matters, MatterCoords> dictionary;

    public MatterUV[] MatterUVs;

    private Quaternion[] cachedQuaternionEulerArr;
    public ScreenGrid<ScreenTileViewModel> Grid { get; set; }
    private Mesh Mesh { get; set; }
    public bool UpdateMesh { get; set; }

    private void Awake()
    {
        Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = Mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float width = texture.width;
        float height = texture.height;

        dictionary = new Dictionary<Matters, MatterCoords>();
        Sprite[] sprites = Resources.LoadAll<Sprite>($"{Constants.PATH_SPRITES}worldMatters");
        foreach (Sprite sprite in sprites)
        {
            Rect rect = sprite.rect;
            Enum.TryParse(sprite.name, out Matters matterType);
            if (!dictionary.ContainsKey(matterType))
            {
                dictionary.Add(matterType, new MatterCoords
                {
                    uv00 = new Vector2(rect.min.x / width, rect.min.y / height),
                    uv11 = new Vector2(rect.max.x / width, rect.max.y / height)
                });
            }
        }
        //foreach (MatterUV matterUV in MatterUVs)
        //{
        //    dictionary.Add(matterUV.MatterType, new MatterCoords
        //    {
        //        uv00 = new Vector2(matterUV.UV00Pixels.x / width, matterUV.UV00Pixels.y / height),
        //        uv11 = new Vector2(matterUV.UV11Pixels.x / width, matterUV.UV11Pixels.y / height)
        //    });
        //}
    }

    public void UpdateHeatmap()
    {
        int width = Grid.Width;
        int height = Grid.Height;

        CreateEmptyMesh(width * height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int index = i * height + j;
                Vector3 quadSize = new Vector3(1, 1) * Grid.CellSize;
                ScreenTileViewModel gridValue = Grid.GetValue(i, j);
                Matters matterType = gridValue.MatterType;
                Vector2 uv00;
                Vector2 uv11;
                if (matterType == Matters.castle)
                {
                    uv00 = Vector2.zero;
                    uv11 = Vector2.zero;
                    quadSize = Vector3.zero;
                }
                else
                {
                    MatterCoords coords = dictionary[matterType];
                    uv00 = coords.uv00;
                    uv11 = coords.uv11;
                }
                AddToMesh(vertices, uvs, triangles, index, Grid.GetWorldPosition(i, j) + quadSize * 0.5f, 0f, quadSize, uv00, uv11);
            }
        }
        Mesh.vertices = vertices;
        Mesh.uv = uvs;
        Mesh.triangles = triangles;
    }

    public void SetGrid(ScreenGrid<ScreenTileViewModel> grid)
    {
        Grid = grid;
        UpdateHeatmap();

        Grid.OnGridValueChanged += Grid_OnValueChanged;
    }

    private void Grid_OnValueChanged(object sender, ScreenGrid<ScreenTileViewModel>.OnGridValueChangedEventArgs e)
    {
        UpdateMesh = true;
    }

    private void LateUpdate()
    {
        if (UpdateMesh)
        {
            UpdateHeatmap();
            UpdateMesh = false;
        }
    }

    // Copied from CodeMonkey's MeshUtils
    public void CreateEmptyMesh(int quadCount, out Vector3[] verts, out Vector2[] uvs, out int[] triangles)
    {
        verts = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];
    }

    // Copied from CodeMonkey's MeshUtils
    public void AddToMesh(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 position, float rotation, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {
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
    private Quaternion GetQuaternionEuler(float rotationF)
    {
        int rotation = Mathf.RoundToInt(rotationF) % 360;
        if (rotation < 0)
        {
            rotation += 360;
        }
        if (cachedQuaternionEulerArr == null)
        {
            cachedQuaternionEulerArr = new Quaternion[360];
            for (int i = 0; i < 360; i++)
            {
                cachedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
            }
        }
        return cachedQuaternionEulerArr[rotation];
    }
}
