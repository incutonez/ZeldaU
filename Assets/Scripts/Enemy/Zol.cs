using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// If hit with the regular sword, it breaks into 2 gels
    /// </summary>
    public class Zol : World.Enemy
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
                Enemies.Zol,
                Enemies.ZolGray,
                Enemies.ZolGreen
            });
            colors.AddRange(new List<Color[]> {
                null,
                new Color[] { EnemyHelper.BaseColor, EnemyHelper.CommonGray },
                new Color[] { EnemyHelper.BaseColor, EnemyHelper.CommonGreen }
            });
        }
    }
}