using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public bool IsTransitioning { get; set; }

        private Base.Inventory Inventory { get; set; }
        private RectTransform Renderer { get; set; }
        private RectTransform Cursor { get; set; }
        private RectTransform ItemsGrid { get; set; }
        private int CurrentIndex { get; set; }
        private float Timer { get; set; }
        private const float DELAY = 0.4f;
        private int ActiveItems { get; set; }
        private List<Items> Items { get; set; } = new List<Items> {
            global::Items.Boomerang,
            global::Items.Bomb,
            global::Items.Bow,
            global::Items.Candle,
            global::Items.Flute,
            global::Items.Food,
            global::Items.PotionBlue,
            global::Items.Wand
        };
        private GameObject[] ItemRefs { get; set; }
        private int MAX_ITEMS { get; set; }

        private void Awake()
        {
            Renderer = GetComponent<RectTransform>();
            MAX_ITEMS = Items.Count - 1;
            MoveCursor(0, false);
        }

        private void Update()
        {
            if (ActiveItems < 2 || IsTransitioning)
            {
                return;
            }
            if (gameObject.activeSelf)
            {
                if (Timer > 0)
                {
                    Timer -= Time.deltaTime;
                    return;
                }
                if (Controls.IsRightKey())
                {
                    MoveCursor(1);
                }
                else if (Controls.IsLeftKey())
                {
                    MoveCursor(-1);
                }
            }
        }

        public void Initialize(Base.Inventory inventory)
        {
            Inventory = inventory;
            ItemsGrid = Manager.Game.MainCanvas.transform.Find("Inventory/ItemsContainer/ItemsGrid").GetComponent<RectTransform>();
            Cursor = ItemsGrid.Find("Selection").GetComponent<RectTransform>();
            ItemRefs = new GameObject[Items.Count];
            foreach (Items item in Items)
            {
                SetItemActive(item);
            }
        }

        public void MoveCursor(int direction, bool playSound = true)
        {
            CurrentIndex += direction;
            if (CurrentIndex < 0)
            {
                CurrentIndex = MAX_ITEMS;
            }
            else if (CurrentIndex > MAX_ITEMS)
            {
                CurrentIndex = 0;
            }
            if (playSound)
            {
                Manager.Game.Audio.PlayFX(Audio.FX.Rupee);
            }
            Cursor.localPosition = ItemRefs[CurrentIndex].transform.parent.localPosition;
            Timer = DELAY;
        }

        public void SetItemActive(Items item)
        {
            int index = Items.IndexOf(item);
            if (index != -1)
            {
                GameObject go = ItemRefs[index];
                if (go == null)
                {
                    go = ItemRefs[index] = ItemsGrid.GetChild(index).transform.Find("Image").gameObject;
                }
                bool isActive = Inventory.GetItemCount(item) != 0;
                go.SetActive(isActive);
                if (isActive)
                {
                    ActiveItems++;
                }
                else if (ActiveItems > 0)
                {
                    ActiveItems--;
                }
            }
        }

        public IEnumerator Pan(RectTransform hud)
        {
            Manager.Game.IsPaused = true;
            IsTransitioning = true;
            bool showMenu = !gameObject.activeSelf;
            // Set right away to pause any movements
            if (showMenu)
            {
                gameObject.SetActive(true);
            }
            Vector2 inventoryDestination = showMenu ? new Vector2(Renderer.anchoredPosition.x, -176) : new Vector2(Renderer.anchoredPosition.x, 0);
            Vector2 hudDestination = showMenu ? new Vector2(hud.anchoredPosition.x, -240) : new Vector2(hud.anchoredPosition.x, -64);
            while (Renderer.anchoredPosition != inventoryDestination)
            {
                Renderer.anchoredPosition = Vector2.MoveTowards(Renderer.anchoredPosition, inventoryDestination, 0.5f);
                hud.anchoredPosition = Vector2.MoveTowards(hud.anchoredPosition, hudDestination, 0.5f);
                yield return null;
            }
            // If the menu is currently active, we want to keep IsTransitioning, so the player and enemies can't move
            if (!showMenu)
            {
                Manager.Game.IsPaused = false;
                gameObject.SetActive(false);
            }
            IsTransitioning = false;
        }
    }
}
