using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Darknut : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 8f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 2f;
        }

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.Darknut,
                Enemies.DarknutBlue
            });
            colors.AddRange(new List<Color[]> {
                new Color[] { EnemyHelper.BaseColor, EnemyHelper.CommonOrange, EnemyHelper.AccentColor, EnemyHelper.CommonRed },
                new Color[] { EnemyHelper.BaseColor, EnemyHelper.CommonBlueLight, EnemyHelper.AccentColor, EnemyHelper.CommonBlue }
            });
        }
    }
}