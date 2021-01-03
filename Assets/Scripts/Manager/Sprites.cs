using NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Manager
{
    public class Sprites
    {
        public List<Sprite> Characters { get; set; }
        public List<Sprite> Enemies { get; set; }
        public List<Sprite> Items { get; set; }
        public List<Sprite> Tiles { get; set; }
        public List<Sprite> Player { get; set; }
        public Dictionary<Enemies, List<Sprite>> EnemyAnimations { get; set; }

        public Sprites()
        {
            LoadAll();
        }

        public void LoadAll()
        {
            Characters = LoadSprites("characters");
            Enemies = LoadSprites("enemies");
            Items = LoadSprites("items");
            Tiles = LoadSprites("tiles");
            Player = LoadSprites("character");
            EnemyAnimations = new Dictionary<Enemies, List<Sprite>>();
        }

        public List<Sprite> LoadSprites(string name)
        {
            return AssetDatabase.LoadAllAssetsAtPath($"Assets/Resources/{Constants.PATH_SPRITES}{name}.png").OfType<Sprite>().ToList();
            return Resources.LoadAll<Sprite>($"{Constants.PATH_SPRITES}{name}").ToList();
        }

        public Sprite GetCharacter(string name)
        {
            return Characters.Find(s => s.name == name);
        }

        //public Sprite GetEnemy(string name)
        //{
        //    return Enemies.Find(s => s.name == name);
        //}

        public Sprite GetItem(string name)
        {
            return Items.Find(s => s.name == name);
        }

        public Sprite GetItem(Items type)
        {
            return GetItem(type.GetCustomAttr("Resource"));
        }

        public List<Sprite> GetEnemySprites(Enemies enemy)
        {
            if (!EnemyAnimations.ContainsKey(enemy))
            {
                string enemyName = enemy.GetDescription();
                List<Sprite> items = Enemies.Where(x => x.name.Contains(enemyName)).Select(x => x).ToList();
                foreach (Sprite item in items)
                {
                    item.name = item.name.Replace(enemyName, "");
                }
                EnemyAnimations.Add(enemy, items);
            }
            return EnemyAnimations[enemy];
        }

        // TODOJEF: Need to fix this... there's some weird caching issue with Resources.LoadAll, and once I change the name,
        // it actually copies to the resource itself and saves even on next play... maybe need to use Texture2D instead of loading
        // sprites through the resources dir?  Might need to make a material?  Or potentially split the enemies into their own sprites?
        public void GetEnemySprites(
            Enemies enemy,
            List<Sprite> actionUp,
            List<Sprite> actionDown,
            List<Sprite> actionRight,
            List<Sprite> actionLeft,
            List<Sprite> idleUp,
            List<Sprite> idleDown,
            List<Sprite> idleRight,
            List<Sprite> idleLeft,
            List<Sprite> walkUp,
            List<Sprite> walkDown,
            List<Sprite> walkRight,
            List<Sprite> walkLeft
        )
        {
            GetCharacterSprites(
                GetEnemySprites(enemy),
                actionUp,
                actionDown,
                actionRight,
                actionLeft,
                idleUp,
                idleDown,
                idleRight,
                idleLeft,
                walkUp,
                walkDown,
                walkRight,
                walkLeft
            );
        }

        public void GetPlayerSprites(
            List<Sprite> actionUp,
            List<Sprite> actionDown,
            List<Sprite> actionRight,
            List<Sprite> actionLeft,
            List<Sprite> idleUp,
            List<Sprite> idleDown,
            List<Sprite> idleRight,
            List<Sprite> idleLeft,
            List<Sprite> walkUp,
            List<Sprite> walkDown,
            List<Sprite> walkRight,
            List<Sprite> walkLeft
        )
        {
            GetCharacterSprites(
                Player,
                actionUp,
                actionDown,
                actionRight,
                actionLeft,
                idleUp,
                idleDown,
                idleRight,
                idleLeft,
                walkUp,
                walkDown,
                walkRight,
                walkLeft
            );
        }

        public void GetCharacterSprites(
            List<Sprite> sprites,
            List<Sprite> actionUp,
            List<Sprite> actionDown,
            List<Sprite> actionRight,
            List<Sprite> actionLeft,
            List<Sprite> idleUp,
            List<Sprite> idleDown,
            List<Sprite> idleRight,
            List<Sprite> idleLeft,
            List<Sprite> walkUp,
            List<Sprite> walkDown,
            List<Sprite> walkRight,
            List<Sprite> walkLeft
        )
        {
            foreach (Sprite sprite in sprites)
            {
                Enum.TryParse(sprite.name, out Animations value);
                switch (value)
                {
                    case Animations.ActionUp:
                        actionUp.Add(sprite);
                        break;
                    case Animations.ActionDown:
                        actionDown.Add(sprite);
                        break;
                    case Animations.ActionRight:
                        actionRight.Add(sprite);
                        break;
                    case Animations.ActionLeft:
                        actionLeft.Add(sprite);
                        break;
                    case Animations.IdleUp:
                        walkUp.Add(sprite);
                        idleUp.Add(sprite);
                        break;
                    case Animations.IdleDown:
                        walkDown.Add(sprite);
                        idleDown.Add(sprite);
                        break;
                    case Animations.IdleRight:
                        walkRight.Add(sprite);
                        idleRight.Add(sprite);
                        break;
                    case Animations.IdleLeft:
                        walkLeft.Add(sprite);
                        idleLeft.Add(sprite);
                        break;
                    case Animations.WalkUp:
                        walkUp.Add(sprite);
                        break;
                    case Animations.WalkDown:
                        walkDown.Add(sprite);
                        break;
                    case Animations.WalkRight:
                        walkRight.Add(sprite);
                        break;
                    case Animations.WalkLeft:
                        walkLeft.Add(sprite);
                        break;
                }
            }
        }
    }
}