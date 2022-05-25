using System;
using System.Linq;
using Enums;
using UnityEngine;

namespace Base {
  [Serializable]
  public class Item {
    public Items Type { get; set; }

    /// <summary>
    /// This is a slightly overloaded property... it typically means the number of this particularly item, but
    /// in the case of Triforce Shards, it means which castle it came from, so we can use that proper position
    /// in the Menu's Triforce
    /// </summary>
    public int Amount { get; set; } = 0;

    public Sprite GetSprite() {
      return Manager.Game.Graphics.GetItem(Type);
    }

    public void PlaySound() {
      FX? sound = null;
      switch (Type) {
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

      if (sound.HasValue) {
        Manager.Game.Audio.PlayFX(sound.Value);
      }
    }

    // TODO: Add GetPickupSound and play it when item is picked up
    public bool IsStackable() {
      switch (Type) {
        case Items.Bomb:
        case Items.Key:
        case Items.RupeeOne:
        case Items.RupeeFive:
          return true;
      }

      return false;
    }

    public bool IsSelectable() {
      return Constants.SelectableItems.Contains(Type);
    }

    public static int GetItemIndex(Items item) {
      return Array.IndexOf(Constants.SelectableItems, item);
    }

    public bool CanAddToInventory() {
      switch (Type) {
        case Items.RupeeOne:
        case Items.RupeeFive:
        case Items.Heart:
        case Items.HeartContainer:
          return false;
      }

      return true;
    }

    public bool IsSword() {
      switch (Type) {
        case Items.Sword:
        case Items.SwordMagical:
        case Items.SwordWhite:
          return true;
      }

      return false;
    }

    public bool IsRupee() {
      switch (Type) {
        case Items.RupeeOne:
        case Items.RupeeFive:
          return true;
      }

      return false;
    }

    public bool IsRing() {
      switch (Type) {
        case Items.RingGreen:
        case Items.RingBlue:
        case Items.RingRed:
          return true;
      }

      return false;
    }

    public bool IsShield() {
      switch (Type) {
        case Items.Shield:
        case Items.ShieldMagical:
          return true;
      }

      return false;
    }

    public bool IsKey() {
      switch (Type) {
        case Items.Key:
        case Items.KeySkeleton:
          return true;
      }

      return false;
    }

    public int GetPickupAmount() {
      if (Amount == 0) {
        switch (Type) {
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
      }

      return 0;
    }
  }
}
