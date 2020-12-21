using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class SceneViewModel
{
    public float x;
    public float y;
    public Matter matter;
}

// TODO: A lot of this is redundant from ItemManager... might be able to use BaseManager somehow?
public class SceneBuilder : BaseManager<SceneBuilder>
{
    private Vector3 tl;
    private Vector3 tr;
    private Vector3 bl;
    private Vector3 br;
    private const float xOrigin = -1.255f;
    private const float yOrigin = -0.84f;

    public void Awake()
    {
        tl = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, 0));
        bl = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        tr = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, Screen.width));
        br = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.width));
        LoadSprites("Sprites/worldMatters");
        LoadPrefab("Prefabs/WorldMatter");
    }

    public void BuildScene(string path)
    {
        SceneViewModel[] matters = JsonConvert.DeserializeObject<SceneViewModel[]>(Resources.Load<TextAsset>(path).text);
        foreach (SceneViewModel viewModel in matters)
        {
            // If we don't increment by 1 for the y, then it's actually right on the edge of the screen
            SpawnObject(new Vector3(viewModel.x * Constants.MATTER_SIZE + bl.x, (viewModel.y + 1) * Constants.MATTER_SIZE + bl.y), viewModel.matter);
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
