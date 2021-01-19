using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class Item : MonoBehaviour
    {
        public Base.Item BaseItem { get; set; }
        public SpriteRenderer Renderer { get; set; }
        public PolygonCollider2D Collider { get; set; }
        public RectTransform Transform { get; set; }

        public static Item Spawn(Vector3 position, Base.Item item, Transform parent)
        {
            RectTransform transform = Instantiate(Manager.Game.Graphics.Item);
            transform.SetParent(parent);
            transform.localPosition = position;
            transform.rotation = Quaternion.identity;

            Item itemWorld = transform.GetComponent<Item>();
            itemWorld.SetItem(item);

            return itemWorld;
        }

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<PolygonCollider2D>();
            Transform = GetComponent<RectTransform>();
        }

        public void SetItem(Base.Item item)
        {
            BaseItem = item;
            Sprite sprite = item.GetSprite();
            if (sprite != null)
            {
                Renderer.sprite = sprite;
                if (sprite != null)
                {
                    Transform.name = sprite.name;
                }
                // If we have a Heart or Triforce, we need to make it blink, so let's add the blink component
                if (item.Type == Items.Heart || item.Type == Items.TriforceShard)
                {
                    Animation.Blink blinkAnimation = gameObject.AddComponent<Animation.Blink>();
                    blinkAnimation.Initialize(Manager.Game.Graphics.GetAnimations(item.Type));
                }
            }
        }

        public Base.Item GetItem()
        {
            return BaseItem;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
