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
        renderer.color = matter.GetColor();
        //transform.name = matter.type.GetDescription();
        if (IsTransition())
        {
            gameObject.layer = 9;
        }
        else
        {
            renderer.sprite = matter.GetSprite();
            worldObjectData.UpdatePolygonCollider2D();
            // TODOJEF: Need to access parent and set name
            //transform.name = renderer.sprite.name;
            if (matter.CanEnter())
            {
                GetComponent<PolygonCollider2D>().enabled = false;
            }
        }
    }

    public bool IsTransition()
    {
        return matter.IsTransition();
    }

    public float GetPositionX()
    {
        return transform.localPosition.x;
    }

    public float GetPositionY()
    {
        return transform.localPosition.y;
    }

    public string GetSpriteName()
    {
        return matter.type.GetDescription();
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
