using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Taken from https://www.youtube.com/watch?v=mZzZXfySeFQ
/// </summary>
public class WorldScreen : MonoBehaviour
{
    public Color GroundColor { get; set; }
    public ScreenGrid<ScreenGridTile> Grid { get; set; }
    public bool GridNeedsRefresh { get; set; }
    public string ScreenId { get; set; }

    private Mesh Mesh { get; set; }
    private List<WorldDoor> WorldDoors { get; set; }

    public WorldScreen Initialize(string screenId, SceneViewModel transition)
    {
        Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = Mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float width = texture.width;
        float height = texture.height;

        WorldDoors = new List<WorldDoor>();
        Grid = new ScreenGrid<ScreenGridTile>(Constants.GRID_COLUMNS, Constants.GRID_ROWS, 1f, new Vector3(-8f, -7.5f), (ScreenGrid<ScreenGridTile> grid, int x, int y) => new ScreenGridTile(grid, x, y));
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

    public List<Vector3> GetOpenTiles()
    {
        List<Vector3> result = new List<Vector3>();
        Grid.EachCell((viewModel, x, y) =>
        {
            if (!viewModel.IsTile())
            {
                result.Add(Grid.GetWorldPosition(x, y));
            }
        });
        return result;
    }

    public void Build(SceneViewModel scene)
    {
        if (scene.Tiles != null)
        {
            foreach (ScreenTileViewModel screenTile in scene.Tiles)
            {
                Matters tileType = screenTile.Type;
                // Order of priority
                WorldColors color = screenTile.AccentColor ?? scene.AccentColor ?? WorldColors.White;
                foreach (ScreenTileChildViewModel child in screenTile.Children)
                {
                    List<float> coordinates = child.Coordinates;
                    float x = coordinates[0];
                    float y = coordinates[1];
                    float xMax = x;
                    float yMax = y;
                    if (child.TileType != Matters.None)
                    {
                        tileType = child.TileType;
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
                            if (tileType == Matters.door)
                            {
                                AddDoor(position, child.Transition);
                            }
                            else if (tileType == Matters.Transition)
                            {
                                AddTransition(position, child.Transition);
                            }
                            else
                            {
                                SetTileType(position, tileType, color);
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
        Transform door = Instantiate(PrefabsManager.WorldDoor, GetWorldPositionOffset(position, GetQuadSize()), Quaternion.identity, transform);
        if (door != null)
        {
            WorldDoor worldDoor = door.GetComponent<WorldDoor>();
            worldDoor.Initialize(GroundColor, transition);
            WorldDoors.Add(worldDoor);
        }
    }

    public void AddTransition(Vector3 position, SceneViewModel transition)
    {
        Transform item = Instantiate(PrefabsManager.WorldTransition, GetWorldPositionOffset(position, GetQuadSize()), Quaternion.identity, transform);
        if (item != null)
        {
            WorldTransition worldItem = item.GetComponent<WorldTransition>();
            worldItem.Initialize(transition);
        }
    }

    public void SetTileType(Vector3 position, Matters matterType, WorldColors color)
    {
        ScreenGridTile viewModel = Grid.GetViewModel(position);
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
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.pathCount = 0;
        Utilities.CreateEmptyMesh(width * height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles, out Color[] colors);
        Grid.EachCell((viewModel, x, y) =>
        {
            // Quads start on the center of each position, so we shift it by the quadSize multiplied by 0.5
            Utilities.AddToMesh(x * height + y, viewModel, vertices, uvs, triangles, colors);
            Vector2[] colliderShape = viewModel.GetColliderShape();
            if (colliderShape != null)
            {
                polygonCollider.pathCount++;
                polygonCollider.SetPath(polygonCollider.pathCount - 1, colliderShape);
            }
        });
        Mesh.vertices = vertices;
        Mesh.uv = uvs;
        Mesh.triangles = triangles;
        Mesh.colors = colors;
    }

    private void Grid_OnValueChanged(object sender, ScreenGrid<ScreenGridTile>.OnGridValueChangedEventArgs e)
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
}