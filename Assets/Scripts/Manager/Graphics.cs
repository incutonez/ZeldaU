using NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
    public class Graphics
    {
        #region Sprites
        public List<Sprite> Items { get; set; }
        public Dictionary<Tiles, World.TileUVs> TileCoordinates { get; set; } = new Dictionary<Tiles, World.TileUVs>();
        public Dictionary<Characters, Dictionary<Animations, List<Sprite>>> NPCAnimations { get; set; } = new Dictionary<Characters, Dictionary<Animations, List<Sprite>>>();
        public Dictionary<Enemies, Dictionary<Animations, List<Sprite>>> EnemyAnimations { get; set; } = new Dictionary<Enemies, Dictionary<Animations, List<Sprite>>>();
        public Dictionary<Animations, List<Sprite>> PlayerAnimations { get; set; } = new Dictionary<Animations, List<Sprite>>();
        #endregion

        #region Prefabs
        public RectTransform WorldDoor { get; set; }
        public RectTransform DoorBlock { get; set; }
        public RectTransform WorldTransition { get; set; }
        public RectTransform WorldScreen { get; set; }
        public RectTransform Enemy { get; set; }
        public RectTransform NPC { get; set; }
        public RectTransform Item { get; set; }
        public RectTransform Player { get; set; }
        public RectTransform UIHeart { get; set; }
        #endregion

        public Graphics()
        {
            LoadAllSprites();
            LoadAllPrefabs();
        }

        public void LoadAllSprites()
        {
            FileSystem.LoadSprites("character", (response) =>
            {
                PlayerAnimations = GetAnimations(response, "");
                PlayerAnimations[Animations.Entering] = PlayerAnimations[Animations.WalkUp];
                PlayerAnimations[Animations.Exiting] = PlayerAnimations[Animations.WalkDown];
            });
            FileSystem.LoadSprites("tiles", (response) =>
            {
                Texture2D parentTexture = response.First().texture;
                int width = parentTexture.width;
                int height = parentTexture.height;
                foreach (Sprite sprite in response)
                {
                    Rect rect = sprite.rect;
                    Enum.TryParse(sprite.name, out Tiles tileType);
                    if (!TileCoordinates.ContainsKey(tileType))
                    {
                        TileCoordinates.Add(tileType, new World.TileUVs
                        {
                            uv00 = new Vector2(rect.min.x / width, rect.min.y / height),
                            uv11 = new Vector2(rect.max.x / width, rect.max.y / height)
                        });
                    }
                }
            });
            FileSystem.LoadSprites("items", (response) =>
            {
                Items = response;
            });
            FileSystem.LoadSprites("characters", (response) =>
            {
                foreach (Characters character in EnumExtensions.GetValues<Characters>())
                {
                    string name = character.GetDescription() + "_";
                    NPCAnimations.Add(character, GetAnimations(response.Where(x => x.name.Contains(name)).ToList(), name));
                }
            });
            // TODOJEF: Potentially load the dir instead and loop through the enemies enum?  We'd have to get the class
            FileSystem.LoadSprites("Enemies/Octorok", (response) =>
            {
                EnemyHelper.GetSubTypes(Enemies.Octorok, GetAnimations(response, ""), EnemyAnimations);
            });
        }

        public void LoadAllPrefabs()
        {
            FileSystem.LoadPrefab("WorldDoor", (response) => {
                WorldDoor = response;
            });
            FileSystem.LoadPrefab("DoorBlock", (response) =>
            {
                DoorBlock = response;
            });
            FileSystem.LoadPrefab("WorldTransition", (response) =>
            {
                WorldTransition = response;
            });
            FileSystem.LoadPrefab("WorldScreen", (response) =>
            {
                WorldScreen = response;
            });
            FileSystem.LoadPrefab("Enemy", (response) =>
            {
                Enemy = response;
            });
            FileSystem.LoadPrefab("NPC", (response) =>
            {
                NPC = response;
            });
            FileSystem.LoadPrefab("Item", (response) =>
            {
                Item = response;
            });
            FileSystem.LoadPrefab("Player", (response) =>
            {
                Player = response;
            });
            FileSystem.LoadPrefab("HeartTemplate", (response) =>
            {
                UIHeart = response;
            });
        }

        public Dictionary<Animations, List<Sprite>> GetAnimations(List<Sprite> animations, string name)
        {
            Dictionary<Animations, List<Sprite>> dict = new Dictionary<Animations, List<Sprite>>();
            foreach (Animations anim in EnumExtensions.GetValues<Animations>())
            {
                dict[anim] = new List<Sprite>();
            }
            foreach (Sprite sprite in animations)
            {
                string spriteName = sprite.name;
                if (name != String.Empty)
                {
                    spriteName = spriteName.Replace(name, "");
                }
                Enum.TryParse(spriteName, out Animations value);
                switch (value)
                {
                    case Animations.ActionUp:
                        dict[value].Add(sprite);
                        break;
                    case Animations.ActionDown:
                        dict[value].Add(sprite);
                        break;
                    case Animations.ActionRight:
                        dict[value].Add(sprite);
                        break;
                    case Animations.ActionLeft:
                        dict[value].Add(sprite);
                        break;
                    case Animations.IdleUp:
                        dict[value].Add(sprite);
                        dict[Animations.WalkUp].Add(sprite);
                        break;
                    case Animations.IdleDown:
                        dict[value].Add(sprite);
                        dict[Animations.Default].Add(sprite);
                        dict[Animations.WalkDown].Add(sprite);
                        break;
                    case Animations.IdleRight:
                        dict[value].Add(sprite);
                        dict[Animations.WalkRight].Add(sprite);
                        break;
                    case Animations.IdleLeft:
                        dict[value].Add(sprite);
                        dict[Animations.WalkLeft].Add(sprite);
                        break;
                    case Animations.WalkUp:
                        dict[value].Add(sprite);
                        break;
                    case Animations.WalkDown:
                        dict[value].Add(sprite);
                        break;
                    case Animations.WalkRight:
                        dict[value].Add(sprite);
                        break;
                    case Animations.WalkLeft:
                        dict[value].Add(sprite);
                        break;
                }
            }
            return dict;
        }

        public Sprite GetItem(string name)
        {
            return Items.Find(s => s.name == name);
        }

        public Sprite GetItem(Items type)
        {
            return GetItem(type.GetCustomAttr("Resource"));
        }
    }
}