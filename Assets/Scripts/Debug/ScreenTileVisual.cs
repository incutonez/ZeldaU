using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Taken from https://www.youtube.com/watch?v=mZzZXfySeFQ
/// </summary>
public class ScreenTileVisual : MonoBehaviour
{
    private struct MatterCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }
    public Color GroundColor { get; set; }
    public ScreenGrid<ScreenTileViewModel> Grid { get; set; }
    public bool GridNeedsRefresh { get; set; }
    public string ScreenId { get; set; }

    private Dictionary<Matters, MatterCoords> Dictionary { get; set; }
    private Quaternion[] CachedQuaternionEulerArr { get; set; }
    private Mesh Mesh { get; set; }
    private ScreenTile ScreenTile { get; set; }

    public void LoadWorld()
    {
        SceneViewModel scene = JsonConvert.DeserializeObject<SceneViewModel>(Resources.Load<TextAsset>($"{Constants.PATH_OVERWORLD}{ScreenId}").text);
        GroundColor = Utilities.HexToColor((scene.GroundColor ?? WorldColors.Tan).GetDescription());
        if (scene.Matters != null)
        {
            foreach (SceneMatterViewModel sceneMatter in scene.Matters)
            {
                Matters matterType = sceneMatter.Type;
                // Order of priority
                WorldColors color = sceneMatter.AccentColor ?? scene.AccentColor ?? WorldColors.White;
                foreach (SceneMatterChildViewModel child in sceneMatter.Children)
                {
                    List<float> coordinates = child.Coordinates;
                    float x = coordinates[0];
                    float y = coordinates[1];
                    float xMax = x;
                    float yMax = y;
                    if (child.Matter != null)
                    {
                        if (child.Matter.type != Matters.None)
                        {
                            matterType = child.Matter.type;
                        }
                    }
                    if (coordinates.Count == 4)
                    {
                        xMax = coordinates[2];
                        yMax = coordinates[3];
                    }
                    for (float i = x; i <= xMax; i++)
                    {
                        for (float j = y; j <= yMax; j++)
                        {
                            Vector3 position = ScreenTile.Grid.GetWorldPosition(i, j);
                            ScreenTile.SetTileMatterType(position, matterType, color);
                            // TODOJEF: Use composite colliders?
                        }
                    }
                }
            }
        }
    }

    public void RefreshGrid()
    {
        int width = Grid.Width;
        int height = Grid.Height;

        CreateEmptyMesh(width * height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles, out Color[] colors);
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
                if (Dictionary.ContainsKey(matterType))
                {
                    MatterCoords coords = Dictionary[matterType];
                    uv00 = coords.uv00;
                    uv11 = coords.uv11;
                }
                else
                {
                    uv00 = Vector2.zero;
                    uv11 = Vector2.zero;
                    quadSize = Vector3.zero;
                }
                AddToMesh(vertices, uvs, triangles, index, Grid.GetWorldPosition(i, j) + quadSize * 0.5f, 0f, quadSize, uv00, uv11, gridValue.GetColor(), colors);
            }
        }
        Mesh.vertices = vertices;
        Mesh.uv = uvs;
        Mesh.triangles = triangles;
        Mesh.colors = colors;
        UpdateMeshCollider();
    }

    // TODOJEF: Need to refine this a bit... the diagonal corners don't necessarily work
    public void UpdateMeshCollider()
    {
        // Get triangles and vertices from mesh
        int[] triangles = Mesh.triangles;
        Vector3[] vertices = Mesh.vertices;

        // Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
        Dictionary<string, KeyValuePair<int, int>> edges = new Dictionary<string, KeyValuePair<int, int>>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            for (int e = 0; e < 3; e++)
            {
                int vert1 = triangles[i + e];
                int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
                string edge = Mathf.Min(vert1, vert2) + ":" + Mathf.Max(vert1, vert2);
                if (edges.ContainsKey(edge))
                {
                    edges.Remove(edge);
                }
                else
                {
                    edges.Add(edge, new KeyValuePair<int, int>(vert1, vert2));
                }
            }
        }

        // Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
        Dictionary<int, int> lookup = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> edge in edges.Values)
        {
            if (lookup.ContainsKey(edge.Key) == false)
            {
                lookup.Add(edge.Key, edge.Value);
            }
        }

        // Get polygon collider
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.pathCount = 0;

        // Loop through edge vertices in order
        int startVert = 0;
        int nextVert = startVert;
        int highestVert = startVert;
        List<Vector2> colliderPath = new List<Vector2>();
        while (true)
        {
            // Add vertex to collider path
            colliderPath.Add(vertices[nextVert]);

            // Get next vertex
            nextVert = lookup[nextVert];

            // Store highest vertex (to know what shape to move to next)
            if (nextVert > highestVert)
            {
                highestVert = nextVert;
            }

            // Shape complete
            if (nextVert == startVert)
            {
                // Add path to polygon collider
                polygonCollider.pathCount++;
                polygonCollider.SetPath(polygonCollider.pathCount - 1, colliderPath.ToArray());
                colliderPath.Clear();

                // Go to next shape if one exists
                if (lookup.ContainsKey(highestVert + 1))
                {

                    // Set starting and next vertices
                    startVert = highestVert + 1;
                    nextVert = startVert;

                    // Continue to next loop
                    continue;
                }

                // No more verts
                break;
            }
        }
    }

    public ScreenTileVisual Initialize(string screenId)
    {
        Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = Mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float width = texture.width;
        float height = texture.height;

        Dictionary = new Dictionary<Matters, MatterCoords>();
        Sprite[] sprites = Resources.LoadAll<Sprite>($"{Constants.PATH_SPRITES}worldMatters");
        foreach (Sprite sprite in sprites)
        {
            Rect rect = sprite.rect;
            Enum.TryParse(sprite.name, out Matters matterType);
            if (!Dictionary.ContainsKey(matterType))
            {
                Dictionary.Add(matterType, new MatterCoords
                {
                    uv00 = new Vector2(rect.min.x / width, rect.min.y / height),
                    uv11 = new Vector2(rect.max.x / width, rect.max.y / height)
                });
            }
        }
        ScreenTile = new ScreenTile(Constants.GRID_COLUMNS, Constants.GRID_ROWS, 1f, new Vector3(-8f, -7.5f));
        Grid = ScreenTile.Grid;
        Grid.OnGridValueChanged += Grid_OnValueChanged;
        ScreenId = screenId;
        LoadWorld();
        return this;
    }

    private void Grid_OnValueChanged(object sender, ScreenGrid<ScreenTileViewModel>.OnGridValueChangedEventArgs e)
    {
        GridNeedsRefresh = true;
    }

    private void LateUpdate()
    {
        if (GridNeedsRefresh)
        {
            RefreshGrid();
            GridNeedsRefresh = false;
        }
    }

    // Copied from CodeMonkey's MeshUtils
    public void CreateEmptyMesh(int quadCount, out Vector3[] verts, out Vector2[] uvs, out int[] triangles, out Color[] colors)
    {
        verts = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];
        colors = new Color[verts.Length];
    }

    // Copied from CodeMonkey's MeshUtils
    public void AddToMesh(
        Vector3[] vertices,
        Vector2[] uvs,
        int[] triangles,
        int index,
        Vector3 position,
        float rotation,
        Vector3 baseSize,
        Vector2 uv00,
        Vector2 uv11,
        Color color,
        Color[] colors
    )
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
    private Quaternion GetQuaternionEuler(float rotationF)
    {
        int rotation = Mathf.RoundToInt(rotationF) % 360;
        if (rotation < 0)
        {
            rotation += 360;
        }
        if (CachedQuaternionEulerArr == null)
        {
            CachedQuaternionEulerArr = new Quaternion[360];
            for (int i = 0; i < 360; i++)
            {
                CachedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
            }
        }
        return CachedQuaternionEulerArr[rotation];
    }
}
