using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Zora : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 4f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
            WeaponDamage = 1f;
        }

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.Zora
            });
            colors.AddRange(new List<Color[]> {
                new[] { EnemyHelper.BaseColor, EnemyHelper.CommonTeal, EnemyHelper.AccentColor, EnemyHelper.CommonRed }
            });
        }
    }
}
