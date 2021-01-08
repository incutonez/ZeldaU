using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Suit : MonoBehaviour
    {
        // This is a reference to the green color in the baseTexture that we'll be searching for and replacing
        public static readonly Color BaseColor = Color.blue;
        public static readonly Color GreenSuit = new Color(184 / Constants.MAX_RGB, 248 / Constants.MAX_RGB, 24 / Constants.MAX_RGB);
        public static readonly Color BlueSuit = new Color(184 / Constants.MAX_RGB, 184 / Constants.MAX_RGB, 248 / Constants.MAX_RGB);
        public static readonly Color RedSuit = new Color(248 / Constants.MAX_RGB, 56 / Constants.MAX_RGB, 0 / Constants.MAX_RGB);

        public Color CurrentColor { get; set; }

        private void Start()
        {
            // TODOJEF: Need to load this from file
            SetSuitColor(Items.RingGreen);
        }

        public Color GetSuitColor(Items itemType)
        {
            switch (itemType)
            {
                case Items.RingBlue:
                    return BlueSuit;
                case Items.RingRed:
                    return RedSuit;
                default:
                    return GreenSuit;
            }
        }

        // TODOJEF: When this updates, also update raft, ladder, and PolsVoice?
        public void SetSuitColor(Items itemType)
        {
            CurrentColor = GetSuitColor(itemType);
            List<Dictionary<Animations, List<Sprite>>> animationSprites = Utilities.ColorAnimations(
                Game.Sprites.PlayerAnimations,
                new List<Dictionary<Animations, List<Sprite>>> { new Dictionary<Animations, List<Sprite>>() },
                new List<Color[]> {
                    new Color[] { BaseColor, CurrentColor }
                }
            );
            // Overwrite current sprites as we just updated their color
            Game.Player.Animation.AnimationSprites = animationSprites[0];
            // Update default sprite if not moving
            Game.Player.Renderer.sprite = animationSprites[0][Animations.Default][0];
        }
    }
}