using System;
using System.Collections.Generic;

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
    public event Action<Inventory, InventoryChangeArgs> OnItemListChanged;
    public bool hasSword = false;
    public float damageModifier = 1f;

    private Action<Item> _useItemAction;
    private List<Item> items;

    public Inventory(Action<Item> useItemAction)
    {
        _useItemAction = useItemAction;
        items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (item != null)
        {
            if (item.IsStackable())
            {
                bool isInInventory = false;
                foreach (Item it in items)
                {
                    if (it.itemType == item.itemType)
                    {
                        isInInventory = true;
                        it.amount += item.amount;
                        break;
                    }
                }
                if (!isInInventory)
                {
                    items.Add(item);
                }
            }
            else
            {
                items.Add(item);
            }
            if (item.IsSword())
            {
                hasSword = true;
            }
            if (item.IsRing())
            {
                damageModifier = item.itemType.GetAttribute<DamageAttribute>().TouchDamage;
            }
            item.PlaySound();
            OnItemListChanged(this, new InventoryChangeArgs(item));
        }
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item inventoryItem = null;
            foreach (Item it in items)
            {
                if (it.itemType == item.itemType)
                {
                    inventoryItem = it;
                    it.amount -= item.amount;
                    break;
                }
            }
            if (inventoryItem != null && inventoryItem.amount <= 0)
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
            hasSword = false;
        }
        OnItemListChanged(this, new InventoryChangeArgs(item, true));
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
