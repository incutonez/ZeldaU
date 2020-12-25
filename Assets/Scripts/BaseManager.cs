using System.Collections.Generic;
using UnityEngine;

public class BaseManager<T> : MonoBehaviour
{
    private List<Sprite> Sprites { get; set; }

    public void Awake()
    {
        Sprites = new List<Sprite>();
    }

    public RectTransform LoadPrefab(string prefabPath)
    {
        return Resources.Load<RectTransform>(prefabPath);
    }

    public void LoadSprites(string spriteLocation)
    {
        Sprites.AddRange(Resources.LoadAll<Sprite>(spriteLocation));
    }

    // Idea taken from https://answers.unity.com/questions/1417421/how-to-load-all-my-sprites-in-my-folder-and-put-th.html
    public Sprite LoadSprite(string spriteName)
    {
        Sprite sprite = null;
        foreach (Sprite item in Sprites)
        {
            if (item.name == spriteName)
            {
                sprite = item;
                break;
            }
        }
        return sprite;
    }
}
