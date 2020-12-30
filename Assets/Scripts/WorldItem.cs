using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item item;
    public new SpriteRenderer renderer;
    public new RectTransform transform;

    private WorldObjectData worldObjectData;

    public static WorldItem SpawnItem(Vector3 position, Item item, Transform parent)
    {
        RectTransform transform = Instantiate(PrefabsManager.Item);
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.rotation = Quaternion.identity;

        WorldItem itemWorld = transform.GetComponent<WorldItem>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    private void Awake()
    {
        worldObjectData = GetComponent<WorldObjectData>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        Sprite sprite = item.GetSprite();
        if (sprite != null)
        {
            worldObjectData.SetObjectData(sprite);
            // If we have a Heart, we need to make it blink, so let's add that animation
            if (item.Type == Items.Heart)
            {
                Animator anim = gameObject.AddComponent<Animator>();
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/HeartBlinkController");
            }
            else if (item.Type == Items.TriforceShard)
            {
                Animator anim = gameObject.AddComponent<Animator>();
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/TriforceBlinkController");
            }
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
