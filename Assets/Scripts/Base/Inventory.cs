using System;
using System.Collections.Generic;

namespace Base
{
    public class InventoryChangeArgs : EventArgs
    {
        public readonly Item item;
        public readonly bool removed;

        public InventoryChangeArgs(Item item, bool removed = false)
        {
            this.item = item;
            this.removed = removed;
        }
    }

    public class Inventory
    {
        public event Action<Items, EventArgs> ChangeSelection;
        public event Action<Inventory, InventoryChangeArgs> OnChanged;
        public Item Sword { get; set; } = null;
        public float DamageModifier { get; set; } = 1f;
        public int Rupees { get; set; } = 0;
        public int Keys { get; set; } = 0;
        public Items SelectedItem { get; set; } = Items.None;

        private Action<Item> _useItemAction;
        private List<Item> items;

        public Inventory(Action<Item> useItemAction)
        {
            _useItemAction = useItemAction;
            items = new List<Item>();
        }

        public int GetItemCount(Items it)
        {
            int count = 0;
            foreach (Item item in items)
            {
                if (item.Type == it)
                {
                    count = item.Amount;
                    break;
                }
            }
            return count;
        }

        public void UpdateSelectedItem(Items item)
        {
            if (SelectedItem != item)
            {
                SelectedItem = item;
                ChangeSelection?.Invoke(item, EventArgs.Empty);
            }
        }

        public void AddItem(Item item)
        {
            if (item != null)
            {
                if (item.IsRupee())
                {
                    Rupees += item.GetPickupAmount();
                }
                else if (item.Type == Items.Key)
                {
                    Keys += item.GetPickupAmount();
                }
                else if (item.IsStackable())
                {
                    bool isInInventory = false;
                    foreach (Item it in items)
                    {
                        if (it.Type == item.Type)
                        {
                            isInInventory = true;
                            it.Amount += item.GetPickupAmount();
                            break;
                        }
                    }
                    if (!isInInventory)
                    {
                        // Seed the amount
                        item.Amount = item.GetPickupAmount();
                        items.Add(item);
                    }
                }
                else if (item.CanAddToInventory())
                {
                    items.Add(item);
                }
                if (item.IsSword())
                {
                    Sword = item;
                }
                else if (item.IsRing())
                {
                    DamageModifier = item.Type.GetAttribute<DamageAttribute>().TouchDamage;
                }
                item.PlaySound();
                OnChanged(this, new InventoryChangeArgs(item));
            }
        }

        public void RemoveItem(Item item)
        {
            if (item.IsStackable())
            {
                Item inventoryItem = null;
                foreach (Item it in items)
                {
                    if (it.Type == item.Type)
                    {
                        inventoryItem = it;
                        it.Amount -= item.Amount;
                        break;
                    }
                }
                if (inventoryItem != null && inventoryItem.Amount <= 0)
                {
                    items.Remove(inventoryItem);
                }
            }
            else
            {
                items.Remove(item);
            }
            if (item.IsSword())
            {
                Sword = null;
            }
            OnChanged(this, new InventoryChangeArgs(item, true));
        }

        public void UseItem(Item item)
        {
            _useItemAction(item);
        }

        public List<Item> GetItems()
        {
            return items;
        }
    }
}
