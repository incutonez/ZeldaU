using Audio;
using System;
using UnityEngine;

[Serializable]
public class Item
{
    public Items Type { get; set; }
    public int Amount { get; set; }

    public Sprite GetSprite()
    {
        return Manager.Game.Sprites.GetItem(Type);
    }

    public void PlaySound()
    {
        FX? sound = null;
        switch (Type)
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
            Manager.Game.Audio.PlayFX(sound.Value);
        }
    }

    // TODOJEF: Add GetPickupSound and play it when item is picked up

    public bool IsStackable()
    {
        switch(Type)
        {
            case Items.Bomb:
            case Items.Key:
            case Items.RupeeOne:
            case Items.RupeeFive:
                return true;
        }
        return false;
    }

    public bool CanAddToInventory()
    {
        switch (Type)
        {
            case Items.RupeeOne:
            case Items.RupeeFive:
            case Items.Heart:
            case Items.HeartContainer:
                return false;
        }
        return true;
    }

    public bool IsSword()
    {
        switch (Type)
        {
            case Items.Sword:
            case Items.SwordMagical:
            case Items.SwordWhite:
                return true;
        }
        return false;
    }

    public bool IsRupee()
    {
        switch (Type)
        {
            case Items.RupeeOne:
            case Items.RupeeFive:
                return true;
        }
        return false;
    }

    public bool IsRing()
    {
        switch(Type)
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
        switch(Type)
        {
            case Items.Shield:
            case Items.ShieldMagical:
                return true;
        }
        return false;
    }

    public int GetPickupAmount()
    {
        switch (Type)
        {
            case Items.Bomb:
                return 4;
            case Items.Key:
                return 1;
            case Items.Heart:
                return 1;
            case Items.HeartContainer:
                return 1;
            case Items.RupeeOne:
                return 1;
            case Items.RupeeFive:
                return 5;
        }
        return 0;
    }
}