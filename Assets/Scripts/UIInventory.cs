using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public Transform lifeContainer;
    public Text rupeeCount;
    public Text keyCount;
    public Text bombCount;
    public Image swordSlotSprite;
    public Image itemSlotSprite;

    private const int padding = 4;
    private const int HEART_WIDTH = 32 + padding;
    private const int HEART_HEIGHT = 34 + padding;
    private Inventory inventory;
    private WorldPlayer player;
    private Transform heartTemplate;
    private Sprite heartHalfSprite;
    private Sprite heartEmptySprite;

    private void Awake()
    {
        heartTemplate = lifeContainer.Find("HeartTemplate");
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
        RefreshLifeUI();
        RefreshRupeeUI();
        RefreshKeyUI();
        RefreshBombUI();
    }

    private void Player_OnTakeDamage(object sender, EventArgs args)
    {
        RefreshLifeUI();
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
                RefreshSwordUI();
            }
            else if (item.IsRupee())
            {
                RefreshRupeeUI();
            }
            else if (item.itemType == Items.Bomb)
            {
                RefreshBombUI();
            }
            else if (item.itemType == Items.Key)
            {
                RefreshKeyUI();
            }
            else if (item.itemType == Items.HeartContainer)
            {
                player.character.AddHealth(Constants.HEART_REFILL, true);
                RefreshLifeUI();
            }
            else if (item.itemType == Items.Heart)
            {
                player.character.AddHealth(Constants.HEART_REFILL);
                RefreshLifeUI();
            }
        }
    }

    private void RefreshSwordUI()
    {
        Sprite swordSprite = inventory.sword?.GetSprite();
        swordSlotSprite.sprite = swordSprite;
        swordSlotSprite.color = new Color(255, 255, 255, swordSprite != null ? 1 : 0);
    }

    private void RefreshRupeeUI()
    {
        rupeeCount.text = inventory.rupees.ToString();
    }

    private void RefreshKeyUI()
    {
        keyCount.text = inventory.keys.ToString();
    }

    private void RefreshBombUI()
    {
        bombCount.text = inventory.GetBombCount().ToString();
    }

    private void RefreshLifeUI()
    {
        int x = 0;
        int y = 0;
        var health = player.GetHealth();
        var maxHealth = player.GetMaxHealth();

        for (int i = 0; i < maxHealth / 2; i++)
        {
            RectTransform heart = Instantiate(heartTemplate, lifeContainer).GetComponent<RectTransform>();
            heart.gameObject.SetActive(true);
            // Need to use the position of where the template is and add to it
            Vector3 position = heartTemplate.position;
            position.x += x * HEART_WIDTH;
            position.y += y * HEART_HEIGHT;
            heart.position = position;
            Image image = heart.GetComponent<Image>();
            var heartCount = (i + 1) * 2;
            if (heartCount > health)
            {
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
