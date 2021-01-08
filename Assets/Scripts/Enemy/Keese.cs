using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Keese : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 2f;
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
                Enemies.Keese,
                Enemies.KeeseBlue,
                Enemies.KeeseRed
            });
            colors.AddRange(new List<Color[]> {
                null,
                new Color[] { EnemyHelper.BODY_COLOR, EnemyHelper.COMMON_BLUE_LIGHT, EnemyHelper.BASE_COLOR, EnemyHelper.COMMON_BLUE },
                new Color[] { EnemyHelper.BODY_COLOR, EnemyHelper.COMMON_ORANGE, EnemyHelper.BASE_COLOR, EnemyHelper.COMMON_RED }
            });
        }
    }
}