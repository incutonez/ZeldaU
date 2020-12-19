using Audio;
using System;
using UnityEngine;

[Serializable]
public class Item
{
    public Items itemType;
    public int amount;

    public Sprite GetSprite()
    {
        return ItemManager.Instance.LoadSpriteByItemType(itemType);
    }

    public void PlaySound()
    {
        FX? sound = null;
        switch (itemType)
        {
            case Items.Heart:
            case Items.Key:
                sound = FX.HeartPickup;
                break;
            case Items.RupeeFive:
            case Items.RupeeOne:
                sound = FX.Rupee;
                break;
            case Items.Bomb:
                sound = FX.ItemPickup;
                break;
        }
        if (sound.HasValue)
        {
            AudioManager.Instance.PlayFX(sound.Value);
        }
    }

    // TODOJEF: Add GetPickupSound and play it when item is picked up

    public bool IsStackable()
    {
        switch(itemType)
        {
            case Items.Bomb:
            case Items.Key:
            case Items.RupeeOne:
            case Items.RupeeFive:
                return true;
            default:
                return false;
        }
    }

    public bool IsSword()
    {
        switch (itemType)
        {
            case Items.Sword:
            case Items.SwordMagical:
            case Items.SwordWhite:
                return true;
        }
        return false;
    }

    public bool IsRing()
    {
        switch(itemType)
        {
            case Items.RingGreen:
            case Items.RingBlue:
            case Items.RingRed:
                return true;
        }
        return false;
    }

    public bool IsShield()
    {
        switch(itemType)
        {
            case Items.Shield:
            case Items.ShieldMagical:
                return true;
        }
        return false;
    }
}