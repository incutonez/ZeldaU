using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Octorok : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 2f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
            WeaponDamage = 1f;
        }

        public override void SetFrameRates()
        {
            Animation.ActionFrameRate = 0.3f;
            Animation.IdleFrameRate = 0.33f;
            Animation.WalkFrameRate = 1f;
        }

        public static List<Enemies> GetSubTypes()
        {
            return new List<Enemies> {
                Enemies.Octorok,
                Enemies.OctorokBlue
            };
        }

        public static List<Color[]> GetSubTypeColors()
        {
            Color baseColor = Constants.ENEMY_BASE_COLOR;
            Color accentColor = Constants.ENEMY_ACCENT_COLOR;
            return new List<Color[]>
            {
                new Color[] { baseColor, Constants.OCTOROK_RED, accentColor, Constants.OCTOROK_RED_ACCENT },
                new Color[] { baseColor, Constants.OCTOROK_BLUE, accentColor, Constants.OCTOROK_BLUE_ACCENT }
            };
        }
    }
}
