using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Trap : World.Enemy
    {
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
                Enemies.Trap
            });
            colors.AddRange(new List<Color[]> {
                new Color[] { EnemyHelper.BaseColor, EnemyHelper.CommonBlue, EnemyHelper.AccentColor, EnemyHelper.CommonBlueLight }
            });
        }
    }
}