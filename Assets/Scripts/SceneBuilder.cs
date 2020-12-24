using Newtonsoft.Json;
using NPCs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneViewModel
{
    public int x { get; set; }
    public int y { get; set; }
    public WorldColors? accentColor { get; set; }
    public WorldColors? groundColor { get; set; }
    public List<SceneMatterViewModel> matters { get; set; }
    public List<SceneEnemyViewModel> enemies { get; set; }
}

public class SceneEnemyViewModel
{
    List<SceneEnemyChildViewModel> children { get; set; }
    public int count { get; set; }
    public Enemies type { get; set; }
}

public class SceneEnemyChildViewModel
{
    public List<int> coordinates { get; set; }
    public Enemy enemy { get; set; }
}

public class SceneMatterViewModel
{
    public WorldColors? accentColor { get; set; }
    public WorldColors? groundColor { get; set; }
    public List<SceneMatterChildViewModel> children { get; set; }
    public Matters type { get; set; }
}

public class SceneMatterChildViewModel
{
    /// <summary>
    /// This is a list of x, y coordinates, and if 4 values are specified, it becomes the max x, y range to keep adding this matter type
    /// </summary>
    public List<int> coordinates { get; set; }
    public Matter matter { get; set; }
}

/// <summary>
/// The camera is set to 7.5 for its size because it's Camera ortographic size = vertical resolution (240) / PPU (16) / 2 = 7.5
/// per https://hackernoon.com/making-your-pixel-art-game-look-pixel-perfect-in-unity3d-3534963cad1d
/// </summary>
public class SceneBuilder : BaseManager<SceneBuilder>
{
    public Transform screensContainer;

    private int currentX;
    private int currentY;
    private Animator animator;
    private const float TRANSITION_PADDING = 0.05f;
    // This is a 2D array of rows (y-axis) x columns (x-axis)
    private bool[,] screenGrid = new bool[Constants.GRID_ROWS, Constants.GRID_COLUMNS];
    private Color groundColor;

    public void Awake()
    {
        animator = transform.Find("Crossfade").GetComponent<Animator>();
        screensContainer = GameObject.Find("Screens").transform;
        LoadSprites($"{Constants.PATH_SPRITES}worldMatters");
        LoadPrefab($"{Constants.PATH_PREFABS}WorldMatter");
    }

    public Transform GetScreen(string screenId)
    {
        Transform parent = screensContainer.Find(screenId);
        if (parent == null)
        {
            parent = new GameObject().transform;
            parent.name = screenId;
            parent.SetParent(screensContainer);
            parent.localPosition = Vector3.zero;
        }
        return parent;
    }

    public void BuildScreen(SceneViewModel scene)
    {
        Transform parent = GetScreen($"{scene.x}{scene.y}");
        // Update to the latest ground color for this scene
        groundColor = HexToColor((scene.groundColor ?? WorldColors.Tan).GetDescription());
        Camera.main.backgroundColor = groundColor;
        foreach (SceneMatterViewModel viewModel in scene.matters)
        {
            Matters matterType = viewModel.type;
            // Order of priority
            WorldColors? color = viewModel.accentColor ?? scene.accentColor;
            foreach (SceneMatterChildViewModel child in viewModel.children)
            {
                List<int> coordinates = child.coordinates;
                int x = coordinates[0];
                int y = coordinates[1];
                int xMax = x;
                int yMax = y;
                if (coordinates.Count == 4)
                {
                    xMax = coordinates[2];
                    yMax = coordinates[3];
                }
                for (int i = x; i <= xMax; i++)
                {
                    for (int j = y; j <= yMax; j++)
                    {
                        Matter matter = child.matter ?? new Matter();
                        if (matter.type == Matters.None)
                        {
                            matter.type = matterType;
                        }
                        if (!matter.IsTransition())
                        {
                            if (!matter.color.HasValue)
                            {
                                matter.color = color;
                            }
                            screenGrid[j, i] = true;
                        }
                        // i and j here are simple indices into our parent game object, which is set at our "0, 0" origin,
                        // which is -8, -7.5
                        SpawnMatter(new Vector3(i + matter.transitionX, j + matter.transitionY), matter, parent);
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
                        if (screenGrid[yTemp, xTemp] == false)
                        {
                            found = true;
                        }
                    }
                    screenGrid[yTemp, xTemp] = true;
                    GameHandler.enemyManager.SpawnEnemy(new Vector3(xTemp, yTemp), enemyType, parent);
                }
            }
        }
    }

    public void ClearScreen(string screenId)
    {
        Transform screen = GetScreen(screenId);
        if (screen != null)
        {
            Destroy(screen.gameObject);
        }
        screenGrid = new bool[Constants.GRID_ROWS, Constants.GRID_COLUMNS];
    }

    public RectTransform SpawnMatter(Vector3 position, Matter matter, Transform parent)
    {
        RectTransform transform = Instantiate(GameHandler.sceneBuilder.prefab);
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.rotation = Quaternion.identity;

        WorldMatter worldMatter = transform.Find("Image").GetComponent<WorldMatter>();
        worldMatter.SetMatter(matter, groundColor);
        // TODOJEF: Better way of doing this?
        transform.name = worldMatter.GetSpriteName();

        return transform;
    }

    public float HexToDec(string hex)
    {
        return Convert.ToInt32(hex, 16) / Constants.MAX_RGB;
    }

    // Idea taken from https://www.youtube.com/watch?v=CMGn2giYLc8
    public Color HexToColor(string hex)
    {
        return new Color(HexToDec(hex.Substring(0, 2)), HexToDec(hex.Substring(2, 2)), HexToDec(hex.Substring(4, 2)));
    }

    public void LoadScreen(WorldMatter worldMatter)
    {
        Matter matter = worldMatter.GetMatter();
        StartCoroutine(LoadScreenRoutine(GetTransitionX(matter), GetTransitionY(matter), GetPlayerTransitionPosition(GameHandler.player.GetPosition(), matter)));
    }

    public int GetTransitionX(Matter matter)
    {
        return currentX + matter.transitionX;
    }

    public int GetTransitionY(Matter matter)
    {
        return currentY + matter.transitionY;
    }

    public void LoadScreen(int x, int y)
    {
        StartCoroutine(LoadScreenRoutine(x, y, Constants.STARTING_POSITION));
    }

    public IEnumerator LoadScreenRoutine(int x, int y, Vector3 playerPosition)
    {
        GameHandler.isTransitioning = true;
        animator.SetBool("Start", true);

        yield return new WaitForSeconds(0.5f);

        // Clear the previous scene
        ClearScreen($"{currentX}{currentY}");
        BuildScreen(JsonConvert.DeserializeObject<SceneViewModel>(Resources.Load<TextAsset>($"{Constants.PATH_OVERWORLD}{x}{y}").text));
        GameHandler.player.transform.localPosition = playerPosition;
        currentX = x;
        currentY = y;

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Start", false);
        GameHandler.isTransitioning = false;
    }

    public Vector3 GetPlayerTransitionPosition(Vector3 position, Matter matter)
    {
        // Moving to the left screen
        if (matter.transitionX == -1)
        {
            position.x = Constants.GRID_COLUMNS_ZERO - TRANSITION_PADDING;
        }
        // Moving to the right screen
        else if (matter.transitionX == 1)
        {
            position.x = TRANSITION_PADDING;
        }
        // Moving to the bottom screen
        if (matter.transitionY == -1)
        {
            position.y = Constants.GRID_ROWS_ZERO - TRANSITION_PADDING;
        }
        // Moving to the top screen
        else if (matter.transitionY == 1)
        {
            position.y = TRANSITION_PADDING;
        }
        return position;
    }
}
