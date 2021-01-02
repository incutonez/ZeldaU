using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class Item : MonoBehaviour
    {
        public global::Item item;
        public SpriteRenderer Renderer { get; set; }
        public PolygonCollider2D Collider { get; set; }
        public RectTransform Transform { get; set; }

        public static Item Spawn(Vector3 position, global::Item item, Transform parent)
        {
            RectTransform transform = Instantiate(Manager.Prefabs.Item);
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

        public void SetItem(global::Item item)
        {
            this.item = item;
            Sprite sprite = item.GetSprite();
            if (sprite != null)
            {
                Renderer.sprite = sprite;
                if (sprite != null)
                {
                    Transform.name = sprite.name;
                    UpdatePolygonCollider2D();
                }
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

        public global::Item GetItem()
        {
            return item;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        // Taken from http://answers.unity.com/answers/1771248/view.html
        public void UpdatePolygonCollider2D(float tolerance = 0.5f)
        {
            List<Vector2> points = new List<Vector2>();
            List<Vector2> simplifiedPoints = new List<Vector2>();
            var sprite = Renderer.sprite;
            Collider.pathCount = sprite.GetPhysicsShapeCount();
            for (int i = 0; i < Collider.pathCount; i++)
            {
                sprite.GetPhysicsShape(i, points);
                LineUtility.Simplify(points, tolerance, simplifiedPoints);
                Collider.SetPath(i, simplifiedPoints);
            }
        }
    }
}
