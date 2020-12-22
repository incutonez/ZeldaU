using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
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
    private Vector3 bl;
    private List<WorldMatter> activeMatters = new List<WorldMatter>();

    public void Awake()
    {
        bl = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        LoadSprites("Sprites/worldMatters");
        LoadPrefab("Prefabs/WorldMatter");
    }

    public void BuildScene(int xId, int yId)
    {
        foreach (WorldMatter worldMatter in activeMatters)
        {
            worldMatter.DestroySelf();
        }

        activeMatters.Clear();

        SceneViewModel scene = JsonConvert.DeserializeObject<SceneViewModel>(Resources.Load<TextAsset>($"Screens\\{xId}{yId}").text);
        foreach (SceneMatterViewModel viewModel in scene.matters)
        {
            BuildMatter(viewModel, scene);
        }
    }

    public void BuildMatter(SceneMatterViewModel viewModel, SceneViewModel scene)
    {
        Matters matterType = viewModel.type;
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
                        // Order of priority
                        matter.color = viewModel.accentColor ?? scene.accentColor;
                    }
                    activeMatters.Add(SpawnObject(new Vector3(i + bl.x, j + 1 + bl.y), matter));
                }
            }
        }
    }

    public WorldMatter SpawnObject(Vector3 position, Matter matter)
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
}
