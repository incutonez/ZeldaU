using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Taken from https://www.youtube.com/watch?v=mZzZXfySeFQ
/// </summary>
public class WorldScreenTile : MonoBehaviour
{
    public Color GroundColor { get; set; }
    public ScreenGrid<ScreenTileViewModel> Grid { get; set; }
    public bool GridNeedsRefresh { get; set; }
    public string ScreenId { get; set; }

    private Quaternion[] CachedQuaternionEulerArr { get; set; }
    private Mesh Mesh { get; set; }
    // TODOJEF: Don't store in this class... store in Utilities?
    private Transform WorldDoorPrefab { get; set; }
    private Transform WorldTransitionPrefab { get; set; }
    private List<WorldDoor> WorldDoors { get; set; }

    public WorldScreenTile Initialize(string screenId, SceneViewModel transition)
    {
        Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = Mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float width = texture.width;
        float height = texture.height;

        WorldDoors = new List<WorldDoor>();
        WorldDoorPrefab = Resources.Load<Transform>($"{Constants.PATH_PREFABS}WorldDoor");
        WorldTransitionPrefab = Resources.Load<Transform>($"{Constants.PATH_PREFABS}WorldTransition");
        Grid = new ScreenGrid<ScreenTileViewModel>(Constants.GRID_COLUMNS, Constants.GRID_ROWS, 1f, new Vector3(-8f, -7.5f), (ScreenGrid<ScreenTileViewModel> grid, int x, int y) => new ScreenTileViewModel(grid, x, y));
        Grid.OnGridValueChanged += Grid_OnValueChanged;
        ScreenId = screenId;
        transform.name = screenId;
        SceneViewModel scene = JsonConvert.DeserializeObject<SceneViewModel>(Resources.Load<TextAsset>($"{Constants.PATH_OVERWORLD}{ScreenId}").text);
        GroundColor = Utilities.HexToColor((scene.GroundColor ?? WorldColors.Tan).GetDescription());
        Build(scene);
        if (transition != null)
        {
            Build(transition);
        }
        return this;
    }

    public void Build(SceneViewModel scene)
    {
        if (scene.Tiles != null)
        {
            foreach (SceneMatterViewModel sceneMatter in scene.Tiles)
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
                            Vector3 position = Grid.GetWorldPosition(i, j);
                            if (matterType == Matters.door)
                            {
                                AddDoor(position, child.Transition);
                            }
                            else if (matterType == Matters.Transition)
                            {
                                AddTransition(position, child.Transition);
                            }
                            else
                            {
                                SetTileType(position, matterType, color);
                            }
                        }
                    }
                }
            }
        }
        // TODOJEF: Add for enemies, characters, and items
    }

    public void ToggleActive(bool active = false)
    {
        gameObject.SetActive(active);
    }

    public void ToggleDoor(bool active)
    {
        WorldDoor door = GetDoor();
        if (door != null)
        {
            door.ToggleHiddenDoor(active);
        }
    }

    public WorldDoor GetDoor(int index = 0)
    {
        return WorldDoors[index];
    }

    public void AddDoor(Vector3 position, SceneViewModel transition)
    {
        // Because our world has each position as being centered, we have to apply the offset... same
        // as what we do in the AddToMesh method
        Transform door = Instantiate(WorldDoorPrefab, GetWorldPositionOffset(position, GetQuadSize()), Quaternion.identity, transform);
        if (door != null)
        {
            WorldDoor worldDoor = door.GetComponent<WorldDoor>();
            worldDoor.Initialize(GroundColor, transition);
            WorldDoors.Add(worldDoor);
        }
    }

    public void AddTransition(Vector3 position, SceneViewModel transition)
    {
        Transform item = Instantiate(WorldTransitionPrefab, GetWorldPositionOffset(position, GetQuadSize()), Quaternion.identity, transform);
        if (item != null)
        {
            WorldTransition worldItem = item.GetComponent<WorldTransition>();
            worldItem.Initialize(transition);
        }
    }

    public void SetTileType(Vector3 position, Matters matterType, WorldColors color)
    {
        ScreenTileViewModel viewModel = Grid.GetViewModel(position);
        if (viewModel != null)
        {
            viewModel.Initialize(matterType, color);
        }
    }

    public Vector3 GetQuadSize()
    {
        return Vector2.one * Grid.CellSize;
    }

    /// <summary>
    /// This method is a little confusing but because our meshes and sprites are pivoted in the center, but our grid positions
    /// them in the lower left of the cell, we need to adjust and center position the world position
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="quadSize"></param>
    /// <returns></returns>
    public Vector3 GetWorldPositionOffset(Vector3 worldPosition, Vector3 quadSize)
    {
        return worldPosition + quadSize * 0.5f;
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
                // Quads start on the center of each position, so we shift it by the quadSize multiplied by 0.5
                AddToMesh(i * height + j, Grid.GetViewModel(i, j), vertices, uvs, triangles, colors);
            }
        }
        Mesh.vertices = vertices;
        Mesh.uv = uvs;
        Mesh.triangles = triangles;
        Mesh.colors = colors;
        UpdatePolygonColliders();
    }

    public void UpdatePolygonColliders()
    {
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.pathCount = 0;
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                Vector2[] collider = Grid.GetViewModel(x, y).GetCollider();
                if (collider != null)
                {
                    polygonCollider.pathCount++;
                    polygonCollider.SetPath(polygonCollider.pathCount - 1, collider);
                }
            }
        }
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

    // Copied from CodeMonkey's MeshUtils and tweaked for colors
    public void AddToMesh(
        int index,
        ScreenTileViewModel tile,
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
        tile.GetCoordinates(out Vector2 uv00, out Vector2 uv11);

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
            for (int i = 0; i < CachedQuaternionEulerArr.Length; i++)
            {
                CachedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
            }
        }
        return CachedQuaternionEulerArr[rotation];
    }
}