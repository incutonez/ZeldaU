using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// This one is a little tricky because the base version flashes between all 4 variants... I'm not sure what the other 2 flash between.
    /// The body for this is made up of 3 different colors... the body, which is white, unless specified, and the accent/base
    /// 
    /// In order to regain control over his sword, he must touch a Blue Bubble, visit a Fairy Fountain, drink a potion, or pick up a Triforce Shard. 
    /// </summary>
    public class Bubble : World.Enemy
    {
        // This is like a burnt orange one
        public static readonly Color BASE_1 = Utilities.HexToColor("b8f818");
        public static readonly Color ACCENT_1 = Utilities.HexToColor("ffe0a8");
        // This is a green one, which is used in the rotation of the 4... need to figure out how to keep swapping colors
        public static readonly Color BODY_4 = Utilities.HexToColor("58f898");
        public static readonly Color BASE_4 = EnemyHelper.COMMON_GREEN;
        public static readonly Color ACCENT_4 = Utilities.HexToColor("00a844");

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.Bubble,
                Enemies.BubbleBlue,
                Enemies.BubbleRed
            });
            colors.AddRange(new List<Color[]> {
                new Color[] { EnemyHelper.BODY_COLOR, EnemyHelper.COMMON_ORANGE_DARK, EnemyHelper.BASE_COLOR, BASE_1, EnemyHelper.ACCENT_COLOR, ACCENT_1 },
                new Color[] { EnemyHelper.BASE_COLOR, EnemyHelper.COMMON_BLUE, EnemyHelper.ACCENT_COLOR, EnemyHelper.COMMON_BLUE_LIGHT },
                new Color[] { EnemyHelper.BASE_COLOR, EnemyHelper.COMMON_RED, EnemyHelper.ACCENT_COLOR, EnemyHelper.COMMON_ORANGE }
            });
        }
    }
}