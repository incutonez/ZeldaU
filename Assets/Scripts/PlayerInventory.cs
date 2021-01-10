using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    private const float padding = 0.05f;

    public Transform LifeContainer { get; set; }
    public Text RupeeCount { get; set; }
    public Text KeyCount { get; set; }
    public Text BombCount { get; set; }
    public Image SwordSlotSprite { get; set; }
    public Image ItemSlotSprite { get; set; }
    public Transform HUD { get; set; }

    private Base.Inventory Inventory { get; set; }
    private World.Player Player { get; set; }
    private Sprite HeartHalfSprite { get; set; }
    private Sprite HeartEmptySprite { get; set; }

    private void Awake()
    {
        HeartHalfSprite = Manager.Game.Graphics.GetItem("HeartHalf");
        HeartEmptySprite = Manager.Game.Graphics.GetItem("HeartEmpty");
        HUD = Manager.Game.MainCanvas.transform.Find("Hud");
        LifeContainer = HUD.transform.Find("LifeContainer");
        var countContainer = HUD.transform.Find("CountContainer").transform;
        RupeeCount = countContainer.Find("RupeeContainer").transform.Find("Count").GetComponent<Text>();
        KeyCount = countContainer.Find("KeyContainer").transform.Find("Count").GetComponent<Text>();
        BombCount = countContainer.Find("BombContainer").transform.Find("Count").GetComponent<Text>();
        ItemSlotSprite = HUD.transform.Find("BSlot").GetChild(2).GetComponent<Image>();
        SwordSlotSprite = HUD.transform.Find("ASlot").GetChild(2).GetComponent<Image>();
    }

    public void SetInventory(Base.Inventory inventory)
    {
        Inventory = inventory;
        Inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    public void SetPlayer(World.Player player)
    {
        Player = player;
        Player.OnInitialize += Player_OnInitialize;
        Player.OnTakeDamage += Player_OnTakeDamage;
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

    private void Inventory_OnItemListChanged(object sender, Base.InventoryChangeArgs args)
    {
        Base.Item item = args.item;
        if (item != null)
        {
            if (item.IsRing())
            {
                Manager.Game.Suit.SetSuitColor(item.Type);
            }
            else if (item.IsShield())
            {
                Manager.Game.Shield.SetShield(item.Type);
            }
            else if (item.IsSword())
            {
                Manager.Game.Sword.SetSword(item);
                Player.Animation.CanAttack = true;
                RefreshSwordUI();
            }
            else if (item.IsRupee())
            {
                RefreshRupeeUI();
            }
            else if (item.Type == Items.Bomb)
            {
                RefreshBombUI();
            }
            else if (item.Type == Items.Key)
            {
                RefreshKeyUI();
            }
            else if (item.Type == Items.HeartContainer)
            {
                Player.AddHealth(Constants.HEART_REFILL, true);
                RefreshLifeUI();
            }
            else if (item.Type == Items.Heart)
            {
                Player.AddHealth(Constants.HEART_REFILL);
                RefreshLifeUI();
            }
        }
    }

    private void RefreshSwordUI()
    {
        Sprite swordSprite = Inventory.sword?.GetSprite();
        SwordSlotSprite.enabled = true;
        SwordSlotSprite.sprite = swordSprite;
        SwordSlotSprite.color = new Color(255, 255, 255, swordSprite != null ? 1 : 0);
    }

    private void RefreshRupeeUI()
    {
        RupeeCount.text = Inventory.rupees.ToString();
    }

    private void RefreshKeyUI()
    {
        KeyCount.text = Inventory.keys.ToString();
    }

    private void RefreshBombUI()
    {
        BombCount.text = Inventory.GetBombCount().ToString();
    }

    private void RefreshLifeUI()
    {
        int x = 0;
        int y = 0;
        var health = Player.Health;
        var maxHealth = Player.MaxHealth;
        for (int i = 0; i < maxHealth / 2; i++)
        {
            RectTransform heart = Instantiate(Manager.Game.Graphics.UIHeart, LifeContainer).GetComponent<RectTransform>();
            heart.gameObject.SetActive(true);
            // Need to use the position of where the template is and add to it
            heart.localPosition += new Vector3(
                x * (heart.sizeDelta.x + padding),
                y * (heart.sizeDelta.y + padding)
            );
            Image image = heart.GetComponent<Image>();
            var heartCount = (i + 1) * 2;
            if (heartCount > health)
            {
                if (heartCount - health >= 2)
                {
                    image.sprite = HeartEmptySprite;
                }
                else if (heartCount - health >= 1)
                {
                    image.sprite = HeartHalfSprite;
                }
            }
            x++;
            if (x > 7)
            {
                x = 0;
                y++;
            }
        }
    }
}
