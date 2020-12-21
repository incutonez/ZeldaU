using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneViewModel
{
    public int x;
    public int y;
    public WorldColors accentColor;
    public WorldColors groundColor;
    public List<SceneMatterViewModel> matters;
}

public class SceneMatterViewModel
{
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

public class SceneBuilder : BaseManager<SceneBuilder>
{
    private Vector3 tl;
    private Vector3 tr;
    private Vector3 bl;
    private Vector3 br;
    // 0-based value, this represents the height for the number of cells we can have
    private const int cellY = 10;
    // 0-based value, this represents the width for the number of cells we can have
    private const int cellX = 15;

    public void Awake()
    {
        tl = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, 0));
        bl = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        tr = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, Screen.width));
        br = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.width));
        LoadSprites("Sprites/worldMatters");
        LoadPrefab("Prefabs/WorldMatter");
    }

    public void BuildScene(int xId, int yId)
    {
        SceneViewModel scene = JsonConvert.DeserializeObject<SceneViewModel>(Resources.Load<TextAsset>($"Screens\\{xId}{yId}").text);
        foreach (SceneMatterViewModel viewModel in scene.matters)
        {
            Matters matterType = viewModel.type;
            foreach (SceneMatterChildViewModel child in viewModel.children)
            {
                BuildMatter(child, matterType);
            }
        }
    }

    public void BuildMatter(SceneMatterChildViewModel child, Matters matterType)
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
                SpawnObject(new Vector3(i * Constants.MATTER_SIZE + bl.x, (j + 1) * Constants.MATTER_SIZE + bl.y), matter);
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
}
