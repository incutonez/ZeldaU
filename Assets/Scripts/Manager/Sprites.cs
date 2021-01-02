using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
    public static class Sprites
    {
        public static List<Sprite> Characters { get; set; }
        public static List<Sprite> Enemies { get; set; }
        public static List<Sprite> Items { get; set; }
        public static List<Sprite> Tiles { get; set; }
        public static List<Sprite> Player { get; set; }

        public static void LoadAll()
        {
            Characters = LoadSprites("characters");
            Enemies = LoadSprites("enemies");
            Items = LoadSprites("items");
            Tiles = LoadSprites("tiles");
            Player = LoadSprites("character");
        }

        public static List<Sprite> LoadSprites(string name)
        {
            return Resources.LoadAll<Sprite>($"{Constants.PATH_SPRITES}{name}").ToList();
        }

        public static Sprite GetCharacter(string name)
        {
            return Characters.Find(s => s.name == name);
        }

        public static Sprite GetEnemy(string name)
        {
            return Enemies.Find(s => s.name == name);
        }

        public static Sprite GetItem(string name)
        {
            return Items.Find(s => s.name == name);
        }

        public static Sprite GetItem(Items type)
        {
            return GetItem(type.GetCustomAttr("Resource"));
        }

        public static void GetPlayerSprites(
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

        public static void GetCharacterSprites(
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