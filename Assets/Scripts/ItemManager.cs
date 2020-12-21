using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Sprite[] sprites;

    public static ItemManager Instance { get; private set; }

    public RectTransform prefab;

    private void Awake()
    {
        Instance = this;
        sprites = Resources.LoadAll<Sprite>("Sprites/items");
    }

    public Sprite LoadSpriteByItemType(Items itemType)
    {
        return LoadSprite(itemType.GetCustomAttr("Resource"));
    }

    // Idea taken from https://answers.unity.com/questions/1417421/how-to-load-all-my-sprites-in-my-folder-and-put-th.html
    public Sprite LoadSprite(string spriteName)
    {
        Sprite sprite = null;
        foreach (Sprite item in sprites)
        {
            if (item.name == spriteName)
            {
                sprite = item;
                break;
            }
        }
        return sprite;

    }

    #region Debug
    /// <summary>
    /// This method will load all of the available items from the enum and place them in the world
    /// in a sequential order... it's used for debugging purposes only
    /// </summary>
    public void LoadAllItems()
    {
        const float xMax = 1.3f;
        var x = -xMax;
        var y = 0.7f;
        List<Items> items = EnumExtensions.GetValues<Items>();
        foreach (Items item in items)
        {
            WorldItem.SpawnItem(new Vector3(x, y), new Item { itemType = item });
            x += .2f;
            if (x > xMax)
            {
                x = -xMax;
                y -= 0.2f;
            }
        }
    }
    #endregion
}
