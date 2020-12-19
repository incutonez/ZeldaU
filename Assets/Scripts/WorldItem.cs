﻿using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item item;
    public new SpriteRenderer renderer;
    public new RectTransform transform;
    public new BoxCollider2D collider;

    private WorldObjectData worldObjectData;

    public static WorldItem SpawnItem(Vector3 position, Item item)
    {
        RectTransform transform = Instantiate(ItemManager.Instance.prefab, position, Quaternion.identity);

        WorldItem itemWorld = transform.GetComponent<WorldItem>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    private void Awake()
    {
        worldObjectData = GetComponent<WorldObjectData>();
    }

    public static WorldItem DropItem(Vector3 dropPosition, Item item)
    {
        return SpawnItem(dropPosition, item);
    }

    public void SetItem(Item item)
    {
        this.item = item;
        Sprite sprite = item.GetSprite();
        if (sprite != null)
        {
            worldObjectData.SetObjectData(sprite);
            // If we have a Heart, we need to make it blink, so let's add that animation
            if (item.itemType == Items.Heart)
            {
                Animator anim = gameObject.AddComponent<Animator>();
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/HeartBlinkController");
            }
            else if (item.itemType == Items.TriforceShard)
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
