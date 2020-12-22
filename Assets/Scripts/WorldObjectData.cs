using UnityEngine;

public class WorldObjectData : MonoBehaviour
{
    private new RectTransform transform;
    private new SpriteRenderer renderer;
    private new BoxCollider2D collider;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
    }

    public void SetObjectData(Sprite sprite, bool setSize = true)
    {
        renderer.sprite = sprite;
        if (sprite != null)
        {
            transform.name = sprite.name;
            if (setSize)
            {
                SetObjectSize(sprite.bounds.size);
            }
        }
    }

    public void SetObjectSize(Vector3 size)
    {
        SetTransformSize(size);
        SetBoxColliderSize(size);
    }

    public void SetTransformSize(Vector3 size)
    {
        if (transform != null)
        {
            transform.sizeDelta = size;
        }
    }

    public void SetBoxColliderSize(Vector3 size)
    {
        if (collider != null)
        {
            collider.offset = Vector2.zero;
            collider.size = new Vector3(size.x / transform.lossyScale.x,
                                    size.y / transform.lossyScale.y,
                                    size.z / transform.lossyScale.z);
        }
    }
}
