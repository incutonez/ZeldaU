using System.Collections.Generic;
using UnityEngine;

namespace Manager {
  public class Suit : MonoBehaviour {
    // This is a reference to the green color in the baseTexture that we'll be searching for and replacing
    private static readonly Color BaseColor = Color.blue;
    private static readonly Color GreenSuit = new(184 / Constants.MaxRGB, 248 / Constants.MaxRGB, 24 / Constants.MaxRGB);
    private static readonly Color BlueSuit = new(184 / Constants.MaxRGB, 184 / Constants.MaxRGB, 248 / Constants.MaxRGB);
    private static readonly Color RedSuit = new(248 / Constants.MaxRGB, 56 / Constants.MaxRGB, 0 / Constants.MaxRGB);

    private Color CurrentColor { get; set; }

    private void Start() {
      // TODOJEF: Need to load this from file
      SetSuitColor(Items.RingGreen);
    }

    private static Color GetSuitColor(Items itemType) {
      return itemType switch {
        Items.RingBlue => BlueSuit, Items.RingRed => RedSuit, _ => GreenSuit
      };
    }

    // TODOJEF: When this updates, also update raft, ladder, and PolsVoice?
    public void SetSuitColor(Items itemType) {
      CurrentColor = GetSuitColor(itemType);
      var animationSprites = Utilities.ColorAnimations(
        Game.Graphics.PlayerAnimations,
        new[] {BaseColor, CurrentColor}
      );
      // TODOJEF: Pick up here, there's some initial lag when walking... not sure why
      // Overwrite current sprites as we just updated their color
      Game.Player.Animation.AnimationSprites = animationSprites;
      // Update default sprite if not moving
      Game.Player.Renderer.sprite = animationSprites[Animations.IdleDown][0];
    }
  }
}
