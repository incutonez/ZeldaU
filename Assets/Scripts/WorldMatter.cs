using UnityEngine;

public class WorldMatter : MonoBehaviour
{
    public Matter matter;

    private new SpriteRenderer renderer;
    private WorldObjectData worldObjectData;

    public static WorldMatter SpawnObject(Vector3 position, Matter matter)
    {
        RectTransform transform = Instantiate(GameHandler.sceneBuilder.prefab, position, Quaternion.identity);

        WorldMatter wObject = transform.GetComponent<WorldMatter>();
        wObject.SetMatter(matter);

        return wObject;
    }

    public void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        worldObjectData = GetComponent<WorldObjectData>();
    }

    public void SetMatter(Matter matter)
    {
        this.matter = matter;
        renderer.sprite = matter.GetSprite();
        worldObjectData.SetObjectData(renderer.sprite);
        if (matter.CanEnter())
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public Matter GetMatter()
    {
        return matter;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
