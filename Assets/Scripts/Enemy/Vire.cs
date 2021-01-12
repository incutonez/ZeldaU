using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// When it's hit without magical sword, it turns into 2 Keese
    /// </summary>
    public class Vire : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 2f;
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
                Enemies.Vire
            });
            colors.AddRange(new List<Color[]> {
                new Color[] { EnemyHelper.BASE_COLOR, EnemyHelper.COMMON_BLUE_LIGHT, EnemyHelper.ACCENT_COLOR, EnemyHelper.COMMON_BLUE }
            });
        }
    }
}