using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneViewModel
{
    public int x;
    public int y;
    public WorldColors? accentColor;
    public WorldColors? groundColor;
    public List<SceneMatterViewModel> matters;
}

public class SceneMatterViewModel
{
    public WorldColors? accentColor;
    public WorldColors? groundColor;
    public List<SceneMatterChildViewModel> children;
    public Matters type;
}

public class SceneMatterChildViewModel
{
    /// <summary>
    /// This is a list of x, y coordinates, and if 4 values are specified, it becomes the max x, y range to keep adding this matter type
    /// </summary>
    public List<int> coordinates;
    public Matter matter;
}

/// <summary>
/// The camera is set to 7.5 for its size because it's Camera ortographic size = vertical resolution (240) / PPU (16) / 2 = 7.5
/// per https://hackernoon.com/making-your-pixel-art-game-look-pixel-perfect-in-unity3d-3534963cad1d
/// </summary>
public class SceneBuilder : BaseManager<SceneBuilder>
{
    private int currentX;
    private int currentY;
    private Animator animator;
    private Vector3 bottomLeftCoords;
    private Vector3 bottomRightCoords;
    private Vector3 topLeftCoords;
    private Vector3 topRightCoords;
    private List<WorldMatter> activeMatters = new List<WorldMatter>();
    private const float TRANSITION_PADDING = 0.5f;

    public void Awake()
    {
        bottomLeftCoords = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        bottomRightCoords = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));
        topLeftCoords = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        topRightCoords = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        animator = transform.Find("Crossfade").GetComponent<Animator>();
        LoadSprites($"{Constants.PATH_SPRITES}worldMatters");
        LoadPrefab($"{Constants.PATH_PREFABS}WorldMatter");
    }

    public void BuildScene(SceneViewModel scene)
    {
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
                        matter.type = matterType;
                        if (!matter.color.HasValue)
                        {
                            matter.color = color;
                        }
                        activeMatters.Add(SpawnMatter(new Vector3(i + bottomLeftCoords.x, j + 1 + bottomLeftCoords.y), matter));
                    }
                }
            }
        }
    }

    public void ClearScene()
    {
        foreach (WorldMatter worldMatter in activeMatters)
        {
            worldMatter.DestroySelf();
        }

        activeMatters.Clear();
    }

    public WorldMatter SpawnMatter(Vector3 position, Matter matter)
    {
        RectTransform transform = Instantiate(GameHandler.sceneBuilder.prefab, position, Quaternion.identity);

        WorldMatter worldMatter = transform.GetComponent<WorldMatter>();
        worldMatter.SetMatter(matter);

        return worldMatter;
    }

    public float HexToDec(string hex)
    {
        return Convert.ToInt32(hex, 16) / 255f;
    }

    // Idea taken from https://www.youtube.com/watch?v=CMGn2giYLc8
    public Color HexToColor(string hex)
    {
        return new Color(HexToDec(hex.Substring(0, 2)), HexToDec(hex.Substring(2, 2)), HexToDec(hex.Substring(4, 2)));
    }

    public void LoadScreen(WorldMatter worldMatter)
    {
        StartCoroutine(LoadScreenRoutine(GetTransitionX(worldMatter.GetMatter()), GetTransitionY(worldMatter.GetMatter()), GetPlayerTransitionPosition(worldMatter)));
    }

    public int GetTransitionX(Matter matter)
    {
        return currentX + matter.transitionX ?? 0;
    }

    public int GetTransitionY(Matter matter)
    {
        return currentY + matter.transitionY ?? 0;
    }

    public void LoadScreen(int x, int y)
    {
        StartCoroutine(LoadScreenRoutine(x, y, Vector3.zero));
    }

    public IEnumerator LoadScreenRoutine(int x, int y, Vector3 playerPosition)
    {
        GameHandler.isTransitioning = true;
        animator.SetBool("Start", true);

        yield return new WaitForSeconds(0.5f);

        // Clear the previous scene
        ClearScene();
        BuildScene(JsonConvert.DeserializeObject<SceneViewModel>(Resources.Load<TextAsset>($"{Constants.PATH_OVERWORLD}{x}{y}").text));
        GameHandler.player.transform.position = playerPosition;
        currentX = x;
        currentY = y;

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Start", false);
        GameHandler.isTransitioning = false;
    }

    public Vector3 GetPlayerTransitionPosition(WorldMatter worldMatter)
    {
        float x = worldMatter.GetPositionX();
        float y = worldMatter.GetPositionY();
        Matter matter = worldMatter.GetMatter();
        // Moving to the left screen
        if (matter.transitionX == -1)
        {
            x = bottomRightCoords.x - TRANSITION_PADDING;
        }
        // Moving to the right screen
        else if (matter.transitionX == 1)
        {
            x = bottomLeftCoords.x + TRANSITION_PADDING;
        }
        // Moving to the bottom screen
        if (matter.transitionY == -1)
        {
            y = topLeftCoords.y - TRANSITION_PADDING;
        }
        // Moving to the top screen
        else if (matter.transitionY == 1)
        {
            y = topRightCoords.y + TRANSITION_PADDING;
        }
        return new Vector3(x, y);
    }
}
