using UnityEngine;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;
    private WorldPlayer player;

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    public void SetPlayer(WorldPlayer player)
    {
        this.player = player;
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
}
