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

    public void SetMatter(Matter matter, Color color)
    {
        this.matter = matter;
        renderer.sprite = matter.GetSprite();
        renderer.color = color;
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
