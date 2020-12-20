using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public Transform playerOverview;
    private const int HEART_WIDTH = 36;
    private const int HEART_HEIGHT = 38;
    private Inventory inventory;
    private WorldPlayer player;
    private Transform heartTemplate;
    private Sprite heartSprite;
    private Sprite heartHalfSprite;
    private Sprite heartEmptySprite;

    private void Awake()
    {
        heartTemplate = playerOverview.Find("HeartTemplate");
        heartSprite = ItemManager.Instance.LoadSprite("Heart");
        heartHalfSprite = ItemManager.Instance.LoadSprite("HeartHalf");
        heartEmptySprite = ItemManager.Instance.LoadSprite("HeartEmpty");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    public void SetPlayer(WorldPlayer player)
    {
        this.player = player;
        player.OnInitialize += Player_OnInitialize;
        player.OnTakeDamage += Player_OnTakeDamage;
    }

    private void Player_OnInitialize(object sender, EventArgs args)
    {
        RefreshUI();
    }

    private void Player_OnTakeDamage(object sender, EventArgs args)
    {
        RefreshUI();
    }

    private void Inventory_OnItemListChanged(object sender, InventoryChangeArgs args)
    {
        Item item = args.item;
        if (item != null)
        {
            if (item.IsRing())
            {
                GameHandler.suitHandler.SetSuitColor(item.itemType);
            }
            else if (item.IsShield())
            {
                GameHandler.shieldHandler.SetShield(item.itemType);
            }
            else if (item.IsSword())
            {
                GameHandler.swordHandler.SetSword(item);
            }
        }
    }

    private void RefreshUI()
    {
        int x = 0;
        int y = 0;
        var health = player.GetHealth();
        var maxHealth = player.GetMaxHealth();

        for (int i = 0; i < maxHealth / 2; i++)
        {
            RectTransform heart = Instantiate(heartTemplate, playerOverview).GetComponent<RectTransform>();
            heart.gameObject.SetActive(true);
            heart.anchoredPosition = new Vector2(x * HEART_WIDTH, y * HEART_HEIGHT);
            Image image = heart.GetComponent<Image>();
            var heartCount = (i + 1) * 2;
            if (heartCount > health)
            {
                // 2 - 0.5 = 1.5 => Half heart + 1/4 heart
                if (heartCount - health >= 2)
                {
                    image.sprite = heartEmptySprite;
                }
                else if (heartCount - health >= 1)
                {
                    image.sprite = heartHalfSprite;
                }
            }
            x++;
            if (x > 8)
            {
                x = 0;
                y++;
            }
        }
    }
}
