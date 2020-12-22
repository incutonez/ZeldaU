using UnityEngine;

public class WorldMatter : MonoBehaviour
{
    public Matter matter;

    private new SpriteRenderer renderer;
    private WorldObjectData worldObjectData;

    public void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        worldObjectData = GetComponent<WorldObjectData>();
    }

    public void SetMatter(Matter matter)
    {
        this.matter = matter;
        renderer.sprite = matter.GetSprite();
        renderer.color = matter.GetColor();
        worldObjectData.SetObjectData(renderer.sprite);
        if (matter.CanEnter())
        {
            GetComponent<PolygonCollider2D>().enabled = false;
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
