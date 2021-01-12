using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Peahat : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 4f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
        }

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.Peahat
            });
            colors.AddRange(new List<Color[]> {
                new Color[] { EnemyHelper.BASE_COLOR, EnemyHelper.COMMON_ORANGE, EnemyHelper.ACCENT_COLOR, EnemyHelper.COMMON_RED }
            });
        }
    }
}