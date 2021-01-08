using NPCs;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyHelper
{
    public static void GetSubTypes(Enemies baseType, Dictionary<Animations, List<Sprite>> animations, Dictionary<Enemies, Dictionary<Animations, List<Sprite>>> allEnemies)
    {
        List<Enemies> subTypes = new List<Enemies>();
        List<Color[]> colors = new List<Color[]>();
        switch (baseType)
        {
            case Enemies.Octorok:
                subTypes = Enemy.Octorok.GetSubTypes();
                colors = Enemy.Octorok.GetSubTypeColors();
                break;
        }
        InitAnimations(
            animations,
            allEnemies,
            subTypes,
            colors
        );
    }

    public static void InitAnimations(Dictionary<Animations, List<Sprite>> animationSprites, Dictionary<Enemies, Dictionary<Animations, List<Sprite>>> enemyAnimations, List<Enemies> enemyTypes, List<Color[]> colors)
    {
        List<Dictionary<Animations, List<Sprite>>> variants = new List<Dictionary<Animations, List<Sprite>>>();
        foreach (Enemies enemyType in enemyTypes)
        {
            Dictionary<Animations, List<Sprite>> animations = new Dictionary<Animations, List<Sprite>>();
            enemyAnimations.Add(enemyType, animations);
            variants.Add(animations);
        }
        foreach (KeyValuePair<Animations, List<Sprite>> entry in animationSprites)
        {
            Animations key = entry.Key;
            List<Sprite> values = entry.Value;
            for (int i = 0; i < variants.Count; i++)
            {
                Dictionary<Animations, List<Sprite>> variant = variants[i];
                List<Sprite> temp = variant[key] = new List<Sprite>();
                foreach (Sprite sprite in values)
                {
                    temp.Add(Utilities.CloneSprite(sprite, colors[i]));
                }
            }
        }
    }
}