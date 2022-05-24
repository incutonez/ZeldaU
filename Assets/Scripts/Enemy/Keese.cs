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
                new[] { EnemyHelper.BodyColor, EnemyHelper.CommonBlueLight, EnemyHelper.BaseColor, EnemyHelper.CommonBlue },
                new[] { EnemyHelper.BodyColor, EnemyHelper.CommonOrange, EnemyHelper.BaseColor, EnemyHelper.CommonRed }
            });
        }
    }
}
