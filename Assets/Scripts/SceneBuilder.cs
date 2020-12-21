using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class SceneViewModel
{
    public float x;
    public float y;
    [SerializeField]
    public Matter matter;
}

// TODO: A lot of this is redundant from ItemManager... might be able to use BaseManager somehow?
public class SceneBuilder : BaseManager<SceneBuilder>
{
    private const float xOrigin = -1.255f;
    private const float yOrigin = -0.84f;

    public void Awake()
    {
        LoadSprites("Sprites/worldMatters");
        LoadPrefab("Prefabs/WorldMatter");
    }

    public void BuildScene(string path)
    {
        SceneViewModel[] matters = JsonConvert.DeserializeObject<SceneViewModel[]>(Resources.Load<TextAsset>(path).text);
        foreach (SceneViewModel viewModel in matters)
        {
            SpawnObject(new Vector3(viewModel.x * Constants.MATTER_SIZE + xOrigin, viewModel.y * Constants.MATTER_SIZE + yOrigin), viewModel.matter);
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
