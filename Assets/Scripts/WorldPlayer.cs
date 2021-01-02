using System;
using UnityEngine;

public class WorldPlayer : WorldCharacter<BaseCharacter>
{
    public UIInventory uiInventory;
    public const float SPEED = 5f;
    public event EventHandler OnInitialize;
    public event EventHandler OnTakeDamage;

    private Inventory inventory;

    private void Start()
    {
        // TODOJEF: Pick up here, have to get sword rendering
        CanAttack = false;
        SpriteRenderer shield = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        shield.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WorldItem itemWorld = collision.GetComponent<WorldItem>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameHandler.IsTransitioning)
        {
            return;
        }
        WorldEnemy worldEnemy = collision.collider.GetComponent<WorldEnemy>();
        if (worldEnemy != null)
        {
            character.TakeDamage(worldEnemy.GetTouchDamage() * inventory.damageModifier);
            OnTakeDamage?.Invoke(this, EventArgs.Empty);
            if (character.IsDead())
            {
                DestroySelf();
            }
        }
    }

    private void UseItem(Item item)
    {
        switch (item.Type)
        {
            case Items.PotionBlue:
                // TODOJEF: Flash character red
                inventory.RemoveItem(new Item { Type = item.Type, Amount = 1 });
                break;
        }
    }

    public void InitializedCharacter()
    {
        inventory = new Inventory(UseItem);
        uiInventory = GameHandler.Inventory;
        uiInventory.SetInventory(inventory);
        uiInventory.SetPlayer(this);
        OnInitialize?.Invoke(this, EventArgs.Empty);
    }
}
