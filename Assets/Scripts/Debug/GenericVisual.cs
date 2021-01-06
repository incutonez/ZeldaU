using UnityEngine;

/// <summary>
/// Taken from https://www.youtube.com/watch?v=mZzZXfySeFQ
/// </summary>
public class GenericVisual : MonoBehaviour
{
    private Quaternion[] cachedQuaternionEulerArr;
    public World.Grid<HeatMapGridObject> Grid { get; set; }
    private Mesh Mesh { get; set; }
    public bool UpdateMesh { get; set; }

    private void Awake()
    {
        Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = Mesh;
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
                HeatMapGridObject gridValue = Grid.GetViewModel(i, j);
                float gridVal = gridValue.GetNormalizedValue();
                Vector2 gridValueUV = new Vector2(gridVal, 0f);
                AddToMesh(vertices, uvs, triangles, index, Grid.GetWorldPosition(i, j) + quadSize * 0.5f, 0f, quadSize, gridValueUV, gridValueUV);
            }
        }
        Mesh.vertices = vertices;
        Mesh.uv = uvs;
        Mesh.triangles = triangles;
    }

    public void SetGrid(World.Grid<HeatMapGridObject> grid)
    {
        Grid = grid;
        UpdateHeatmap();

        Grid.OnGridValueChanged += Grid_OnValueChanged;
    }

    private void Grid_OnValueChanged(object sender, World.Grid<HeatMapGridObject>.OnGridValueChangedEventArgs e)
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
