using System.Collections.Generic;
using UnityEngine;

public class WorldObjectData : MonoBehaviour
{
    private new RectTransform transform;
    private new SpriteRenderer renderer;
    private new PolygonCollider2D collider;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<PolygonCollider2D>();
    }

    public void SetObjectData(Sprite sprite, bool setSize = true)
    {
        renderer.sprite = sprite;
        if (sprite != null)
        {
            transform.name = sprite.name;
            if (setSize)
            {
                if (transform != null)
                {
                    transform.sizeDelta = sprite.bounds.size;
                }
                UpdatePolygonCollider2D();
            }
        }
    }

    // Taken from http://answers.unity.com/answers/1771248/view.html
    public void UpdatePolygonCollider2D(float tolerance = 0.5f)
    {
        List<Vector2> points = new List<Vector2>();
        List<Vector2> simplifiedPoints = new List<Vector2>();
        if (renderer == null)
        {
            renderer = GetComponent<SpriteRenderer>();
        }
        if (collider == null)
        {
            collider = GetComponent<PolygonCollider2D>();
        }
        var sprite = renderer.sprite;
        collider.pathCount = sprite.GetPhysicsShapeCount();
        for (int i = 0; i < collider.pathCount; i++)
        {
            sprite.GetPhysicsShape(i, points);
            LineUtility.Simplify(points, tolerance, simplifiedPoints);
            collider.SetPath(i, simplifiedPoints);
        }
    }
}
