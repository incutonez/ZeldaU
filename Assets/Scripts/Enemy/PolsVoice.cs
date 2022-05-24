using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class PolsVoice : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 20f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 4f;
        }

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.PolsVoice
            });
            colors.AddRange(new List<Color[]> {
                // Accent color depends on the suit that's being worn... will have to figure out how to update it when the suit changes
                // TODOJEF: Somehow use Manager.Game.Suit.CurrentColor for ACCENT_COLOR
                new[] { EnemyHelper.BodyColor, EnemyHelper.CommonOrange, EnemyHelper.BaseColor, EnemyHelper.CommonOrangeDark, EnemyHelper.AccentColor, EnemyHelper.CommonOrange }
            });
        }
    }
}
