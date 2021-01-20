using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public bool IsTransitioning { get; set; }
        public int CurrentIndex { get; set; }

        private Base.Inventory Inventory { get; set; }
        private RectTransform Renderer { get; set; }
        private RectTransform Cursor { get; set; }
        private RectTransform ItemsGrid { get; set; }
        private float Timer { get; set; }
        private const float DELAY = 0.4f;
        private Image BSlotImage { get; set; }
        private bool[] ActiveItems { get; set; }
        private GameObject[] ItemRefs { get; set; }
        private int MAX_ITEMS { get; set; }

        private void Awake()
        {
            Renderer = GetComponent<RectTransform>();
            MAX_ITEMS = Constants.SelectableItems.Length - 1;
        }

        private void Update()
        {
            if (Constants.SelectableItems.Length < 2 || IsTransitioning)
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
            BSlotImage = Manager.Game.MainCanvas.transform.Find("Inventory/BSlot/Image").GetComponent<Image>();
            ItemsGrid = Manager.Game.MainCanvas.transform.Find("Inventory/ItemsContainer/ItemsGrid").GetComponent<RectTransform>();
            Cursor = ItemsGrid.Find("Selection").GetComponent<RectTransform>();
            int count = Constants.SelectableItems.Length;
            ItemRefs = new GameObject[count];
            ActiveItems = new bool[count];
            for (int i = 0; i < count; i++)
            {
                SetItemActive(Constants.SelectableItems[i], i);
            }
        }

        public int GetActiveCount()
        {
            return ActiveItems.Count(x => x);
        }

        public int GetFirstActiveItem()
        {
            return ActiveItems.ToList().FindIndex(x => x);
        }

        public void MoveCursor(int direction, bool playSound = true)
        {
            bool isFound = false;
            int count = GetActiveCount();
            // We have no items
            if (count == 0)
            {
                CurrentIndex = 0;
            }
            else if (count == 1)
            {
                isFound = true;
                CurrentIndex = GetFirstActiveItem();
            }
            else
            {
                // Find the next slot that our cursor can go to
                while (!isFound)
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
                    isFound = ActiveItems[CurrentIndex];
                }
                if (playSound)
                {
                    Manager.Game.Audio.PlayFX(Audio.FX.Rupee);
                }
            }
            GameObject go = ItemRefs[CurrentIndex];
            Cursor.localPosition = go.transform.parent.localPosition;
            BSlotImage.gameObject.SetActive(isFound);
            if (isFound)
            {
                BSlotImage.sprite = go.GetComponent<Image>().sprite;
                BSlotImage.rectTransform.sizeDelta = go.GetComponent<RectTransform>().sizeDelta;
            }
            Timer = DELAY;
        }

        public void SetItemActive(Items itemType)
        {
            SetItemActive(itemType, Base.Item.GetItemIndex(itemType));
        }

        public void SetItemActive(Items itemType, int index)
        {
            if (index != -1)
            {
                GameObject go = ItemRefs[index];
                if (go == null)
                {
                    go = ItemRefs[index] = ItemsGrid.GetChild(index).transform.Find("Image").gameObject;
                }
                bool isActive = Inventory.GetItemCount(itemType) != 0;
                go.SetActive(isActive);
                ActiveItems[index] = isActive;
                // First item in here, so let's select it
                if (isActive && GetActiveCount() == 1)
                {
                    MoveCursor(0);
                    Inventory.UpdateSelectedItem(Constants.SelectableItems[CurrentIndex]);
                }
            }
        }

        public IEnumerator Pan(Hud hud)
        {
            Manager.Game.IsPaused = true;
            IsTransitioning = true;
            bool showMenu = !gameObject.activeSelf;
            RectTransform hudRenderer = hud.Renderer;
            // Set right away to pause any movements
            if (showMenu)
            {
                gameObject.SetActive(true);
                hud.ToggleSlots(false);
            }
            Vector2 inventoryDestination = showMenu ? new Vector2(Renderer.anchoredPosition.x, -176) : new Vector2(Renderer.anchoredPosition.x, 0);
            Vector2 hudDestination = showMenu ? new Vector2(hudRenderer.anchoredPosition.x, -240) : new Vector2(hudRenderer.anchoredPosition.x, -64);
            while (Renderer.anchoredPosition != inventoryDestination)
            {
                Renderer.anchoredPosition = Vector2.MoveTowards(Renderer.anchoredPosition, inventoryDestination, 0.5f);
                hudRenderer.anchoredPosition = Vector2.MoveTowards(hudRenderer.anchoredPosition, hudDestination, 0.5f);
                yield return null;
            }
            // If the menu is currently active, we want to keep IsTransitioning, so the player and enemies can't move
            if (!showMenu)
            {
                Manager.Game.IsPaused = false;
                gameObject.SetActive(false);
                Inventory.UpdateSelectedItem(Constants.SelectableItems[CurrentIndex]);
                hud.ToggleSlots(true);
            }
            IsTransitioning = false;
        }

        private void OnEnable()
        {
            MoveCursor(0, false);
        }
    }
}
