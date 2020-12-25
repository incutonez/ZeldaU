using Newtonsoft.Json;
using NPCs;
using System.Collections.Generic;
using UnityEngine;

public class WorldScreen : MonoBehaviour
{
    public SceneViewModel ViewModel { get; set; }
    public Transform Parent { get; set; }
    public Color GroundColor { get; set; }
    // This is a 2D array of rows (y-axis) x columns (x-axis)
    public bool[,] Grid = new bool[Constants.GRID_ROWS, Constants.GRID_COLUMNS];

    public WorldMatter GetDoor()
    {
        Transform door = transform.Find("door");
        if (door == null)
        {
            return null;
        }
        Transform image = door.Find("Image");
        return image == null ? null : image.gameObject.GetComponent<WorldMatter>();
    }

    public void SetDoorColor(Color color)
    {
        WorldMatter door = GetDoor();
        if (door != null)
        {
            door.hiddenDoor.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void ToggleDoor(bool active = false)
    {
        WorldMatter door = GetDoor();
        if (door != null)
        {
            door.hiddenDoor.gameObject.SetActive(active);
        }
    }

    public WorldScreen Initialize(string screenId, Transform container)
    {
        transform.name = screenId;
        transform.SetParent(container);
        transform.localPosition = Vector3.zero;
        ViewModel = JsonConvert.DeserializeObject<SceneViewModel>(Resources.Load<TextAsset>($"{Constants.PATH_OVERWORLD}{screenId}").text);
        GroundColor = Utilities.HexToColor((ViewModel.groundColor ?? WorldColors.Tan).GetDescription());
        Build(ViewModel);
        SetDoorColor(GroundColor);
        return this;
    }

    public void Build(SceneViewModel scene)
    {
        if (scene == null)
        {
            return;
        }
        if (scene.matters != null)
        {
            foreach (SceneMatterViewModel viewModel in scene.matters)
            {
                Matters matterType = viewModel.type;
                // Order of priority
                WorldColors? color = viewModel.accentColor ?? scene.accentColor;
                foreach (SceneMatterChildViewModel child in viewModel.children)
                {
                    List<float> coordinates = child.coordinates;
                    float x = coordinates[0];
                    float y = coordinates[1];
                    float xMax = x;
                    float yMax = y;
                    if (coordinates.Count == 4)
                    {
                        xMax = coordinates[2];
                        yMax = coordinates[3];
                    }
                    for (float i = x; i <= xMax; i++)
                    {
                        for (float j = y; j <= yMax; j++)
                        {
                            // i and j here are simple indices into our parent game object, which is set at our "0, 0" origin,
                            // which is -8, -7.5
                            SpawnMatter(child, i, j, matterType, color);
                        }
                    }
                }
            }
        }
        if (scene.enemies != null)
        {
            System.Random r = new System.Random();
            foreach (SceneEnemyViewModel viewModel in scene.enemies)
            {
                Enemies enemyType = viewModel.type;
                // Randomly spawn enemies
                for (int i = 0; i < viewModel.count; i++)
                {
                    int xTemp = 0;
                    int yTemp = 0;
                    bool found = false;
                    while (!found)
                    {
                        xTemp = r.Next(0, 16);
                        yTemp = r.Next(0, 11);
                        if (Grid[yTemp, xTemp] == false)
                        {
                            found = true;
                        }
                    }
                    Grid[yTemp, xTemp] = true;
                    GameHandler.CharacterManager.SpawnEnemy(new Vector3(xTemp, yTemp), enemyType, transform);
                }
            }
        }
    }

    public RectTransform SpawnMatter(SceneMatterChildViewModel child, float column, float row, Matters matterType, WorldColors? color)
    {
        SceneViewModel transition = child.transition;
        Matter matter = child.matter ?? new Matter();
        if (matter.type == Matters.None)
        {
            matter.type = matterType;
        }
        if (transition == null)
        {
            if (!matter.color.HasValue)
            {
                matter.color = color;
            }
            // We need to generate which cells the floating value takes up... this produces a 2x2 grid potentially, but it's
            // usually a 1x1 grid
            int rowUp = Mathf.CeilToInt(row);
            int rowDown = Mathf.FloorToInt(row);
            int colUp = Mathf.CeilToInt(column);
            int colDown = Mathf.FloorToInt(column);
            for (int i = rowDown; i <= rowUp; i++)
            {
                for (int j = colDown; j <= colUp; j++)
                {
                    Grid[i, j] = true;
                }
            }
        }
        else
        {
            column += transition.x;
            row += transition.y;
        }
        RectTransform transform = Instantiate(GameHandler.SceneBuilder.worldMatterPrefab);
        transform.SetParent(this.transform);
        transform.localPosition = new Vector3(column, row);
        transform.rotation = Quaternion.identity;

        WorldMatter worldMatter = transform.Find("Image").GetComponent<WorldMatter>();
        worldMatter.SetMatter(matter);
        worldMatter.transition = transition;
        // TODOJEF: Better way of doing this?
        transform.name = worldMatter.GetSpriteName();

        return transform;
    }
}
