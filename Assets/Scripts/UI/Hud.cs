using Base;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Hud : MonoBehaviour
    {
        public Transform LifeContainer { get; set; }
        public Text RupeeCount { get; set; }
        public Text KeyCount { get; set; }
        public Text BombCount { get; set; }
        public Image SwordSlotSprite { get; set; }
        public Image ItemSlotSprite { get; set; }
        public Menu Menu { get; set; }
        public RectTransform Renderer { get; set; }
        public Items BItem { get; set; }

        private Base.Inventory Inventory { get; set; }
        private World.Player Player { get; set; }
        private Sprite HeartSprite { get; set; }
        private Sprite HeartHalfSprite { get; set; }
        private Sprite HeartEmptySprite { get; set; }

        private void Awake()
        {
            Transform mainCanvas = Manager.Game.MainCanvas.transform;
            Renderer = mainCanvas.Find("Hud").GetComponent<RectTransform>();
            Menu = mainCanvas.Find("Inventory").GetComponent<Menu>();

            HeartSprite = Manager.Game.Graphics.GetItem(Items.Heart);
            HeartHalfSprite = Manager.Game.Graphics.GetItem(Items.HeartHalf);
            HeartEmptySprite = Manager.Game.Graphics.GetItem(Items.HeartEmpty);
            LifeContainer = Renderer.Find("LifeContainer/Life");
            RupeeCount = Renderer.Find($"{Constants.CountContainerRef}/Rupees/Amount").GetComponent<Text>();
            KeyCount = Renderer.Find($"{Constants.CountContainerRef}/Keys/Amount").GetComponent<Text>();
            BombCount = Renderer.Find($"{Constants.CountContainerRef}/Bombs/Amount").GetComponent<Text>();
            ItemSlotSprite = Renderer.Find("BSlot/BImage").GetComponent<Image>();
            SwordSlotSprite = Renderer.Find("ASlot/AImage").GetComponent<Image>();
        }

        private void Update()
        {
            if (!Menu.IsTransitioning && Controls.IsInventoryKeyDown())
            {
                StartCoroutine(Menu.Pan(this));
            }
        }

        public void Initialize(Inventory inventory, World.Player player)
        {
            Inventory = inventory;
            Inventory.OnChanged += Inventory_OnChanged;
            Inventory.ChangeSelection += Inventory_OnChangeSelection;
            Player = player;
            Player.OnInitialize += Player_OnInitialize;
            Player.OnTakeDamage += Player_OnTakeDamage;
            Menu.Initialize(inventory);
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

        private void Inventory_OnChangeSelection(Items item, EventArgs arg2)
        {
            ItemSlotSprite.sprite = Manager.Game.Graphics.GetItem(item);
            ItemSlotSprite.rectTransform.sizeDelta = ItemSlotSprite.sprite.rect.size / 2;
        }

        private void Inventory_OnChanged(Inventory inventory, InventoryChangeArgs args)
        {
            Item item = args.item;
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
                else if (item.IsKey())
                {
                    RefreshKeyUI();
                }
                else if (item.Type == Items.HeartContainer)
                {
                    Player.AddHealth(Constants.HeartRefill, true);
                    RefreshLifeUI();
                }
                else if (item.Type == Items.Heart)
                {
                    Player.AddHealth(Constants.HeartRefill);
                    RefreshLifeUI();
                }
                else if (item.Type == Items.TriforceShard)
                {
                    Menu.SetTriforceActive(item.Amount);
                }
                if (item.IsSelectable())
                {
                    Menu.SetItemActive(item.Type);
                }
            }
        }

        private void RefreshSwordUI()
        {
            Sprite swordSprite = Inventory.Sword?.GetSprite();
            SwordSlotSprite.enabled = true;
            SwordSlotSprite.sprite = swordSprite;
            SwordSlotSprite.color = new Color(255, 255, 255, swordSprite != null ? 1 : 0);
        }

        public void ToggleSlots(bool enabled)
        {
            ItemSlotSprite.enabled = enabled;
            SwordSlotSprite.gameObject.SetActive(enabled);
        }

        private void RefreshRupeeUI()
        {
            RupeeCount.text = Inventory.Rupees.ToString();
        }

        private void RefreshKeyUI()
        {
            KeyCount.text = Inventory.Keys.ToString();
        }

        private void RefreshBombUI()
        {
            int count = Inventory.GetItemCount(Items.Bomb);
            BombCount.text = count.ToString();
            Menu.SetItemActive(Items.Bomb);
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
}
