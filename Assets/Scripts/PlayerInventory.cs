using System;
using System.Collections;
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
    public RectTransform InventoryUI { get; set; }
    public RectTransform Hud { get; set; }
    public bool IsMenuActive { get; set; }

    private Base.Inventory Inventory { get; set; }
    private World.Player Player { get; set; }
    private Sprite HeartSprite { get; set; }
    private Sprite HeartHalfSprite { get; set; }
    private Sprite HeartEmptySprite { get; set; }

    private void Awake()
    {
        Transform mainCanvas = Manager.Game.MainCanvas.transform;
        Hud = mainCanvas.Find("Hud").GetComponent<RectTransform>();
        InventoryUI = mainCanvas.Find("Inventory").GetComponent<RectTransform>();

        HeartSprite = Manager.Game.Graphics.GetItem(Items.Heart);
        HeartHalfSprite = Manager.Game.Graphics.GetItem(Items.HeartHalf);
        HeartEmptySprite = Manager.Game.Graphics.GetItem(Items.HeartEmpty);
        LifeContainer = Hud.Find("LifeContainer").Find("Life");
        Transform countContainer = Hud.Find("CountContainer").transform;
        RupeeCount = countContainer.Find("Rupees").transform.Find("Amount").GetComponent<Text>();
        KeyCount = countContainer.Find("Keys").transform.Find("Amount").GetComponent<Text>();
        BombCount = countContainer.Find("Bombs").transform.Find("Amount").GetComponent<Text>();
        ItemSlotSprite = Hud.Find("BSlot").GetChild(2).GetComponent<Image>();
        SwordSlotSprite = Hud.Find("ASlot").GetChild(2).GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            StartCoroutine(PanMenu());
        }
    }

    public IEnumerator PanMenu()
    {
        Manager.Game.IsPaused = true;
        bool showMenu = !Manager.Game.IsMenuShowing;
        // Set right away to pause any movements
        if (showMenu)
        {
            Manager.Game.IsMenuShowing = showMenu;
        }
        Vector2 inventoryDestination = showMenu ? new Vector2(InventoryUI.anchoredPosition.x, -176) : new Vector2(InventoryUI.anchoredPosition.x, 0);
        Vector2 hudDestination = showMenu ? new Vector2(Hud.anchoredPosition.x, -240) : new Vector2(Hud.anchoredPosition.x, -64);
        while (InventoryUI.anchoredPosition != inventoryDestination)
        {
            InventoryUI.anchoredPosition = Vector2.MoveTowards(InventoryUI.anchoredPosition, inventoryDestination, 0.5f);
            Hud.anchoredPosition = Vector2.MoveTowards(Hud.anchoredPosition, hudDestination, 0.5f);
            yield return null;
        }
        // If the menu is currently active, we want to keep IsTransitioning, so the player and enemies can't move
        if (!showMenu)
        {
            Manager.Game.IsPaused = false;
            Manager.Game.IsMenuShowing = false;
        }
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
        float? health = Player.Health;
        // 0-based value
        float? maxHealth = (Player.MaxHealth / 2) - 1;
        for (int i = 0; i < LifeContainer.childCount; i++)
        {
            RectTransform heart = LifeContainer.GetChild(i).GetComponent<RectTransform>();
            if (i > maxHealth)
            {
                heart.gameObject.SetActive(false);
            }
            else
            {
                heart.gameObject.SetActive(true);
                Image image = heart.GetComponent<Image>();
                int heartCount = (i + 1) * 2;
                // This means we should either show a half heart or empty heart
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
                // Otherwise, we're still at a full heart
                else
                {
                    image.sprite = HeartSprite;
                }
            }
        }
    }
}
