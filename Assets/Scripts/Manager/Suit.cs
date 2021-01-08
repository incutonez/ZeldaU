using System.Linq;
using UnityEngine;

namespace Manager
{
    public class Suit : MonoBehaviour
    {
        public Texture2D texture;
        public Color CurrentColor { get; set; }

        private int TextureWidth { get; set; }
        private int TextureHeight { get; set; }
        // This is a reference to the green color in the baseTexture that we'll be searching for and replacing
        private readonly static Color baseSuitColor = new Color(0f, 0f, 1f);
        private readonly static Color greenSuit = new Color(184 / Constants.MAX_RGB, 248 / Constants.MAX_RGB, 24 / Constants.MAX_RGB);
        private readonly static Color blueSuit = new Color(184 / Constants.MAX_RGB, 184 / Constants.MAX_RGB, 248 / Constants.MAX_RGB);
        private readonly static Color redSuit = new Color(248 / Constants.MAX_RGB, 56 / Constants.MAX_RGB, 0 / Constants.MAX_RGB);
        private Color[] baseColors;

        private void Start()
        {
            Texture2D texture = Game.Sprites.PlayerBase[0].texture;
            TextureWidth = texture.width;
            TextureHeight = texture.height;
            baseColors = texture.GetPixels(0, 0, TextureWidth, TextureHeight);
            SetSuitColor(Items.RingGreen);
        }

        public Color GetSuitColor(Items itemType)
        {
            switch (itemType)
            {
                case Items.RingBlue:
                    return blueSuit;
                case Items.RingRed:
                    return redSuit;
                default:
                    return greenSuit;
            }
        }

        // TODOJEF: Use Utilities method instead?
        public void SetSuitColor(Items itemType)
        {
            CurrentColor = GetSuitColor(itemType);
            Color[] colors = baseColors.ToArray();
            for (int i = 0; i < colors.Length; i++)
            {
                Color color = colors[i];
                if (color == baseSuitColor)
                {
                    colors[i] = CurrentColor;
                }
            }
            texture.SetPixels(0, 0, TextureWidth, TextureHeight, colors);
            texture.Apply();
        }
    }
}