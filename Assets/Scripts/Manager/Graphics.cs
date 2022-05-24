using Newtonsoft.Json;
using NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.String;

namespace Manager {
  public class Graphics {
    #region Sprites

    private List<Sprite> Items { get; set; }
    private List<Sprite> Tiles { get; set; } = new();
    public Material CurrentCastleMaterial { get; set; }
    public Material CastleMaterials { get; set; }
    public Material WorldMaterials { get; set; }
    public Dictionary<Characters, Dictionary<Animations, List<Sprite>>> NpcAnimations { get; set; } = new();
    public Dictionary<string, Dictionary<Animations, List<Sprite>>> EnemyAnimations { get; set; } = new();
    public Dictionary<Animations, List<Sprite>> PlayerAnimations { get; set; } = new();

    #endregion

    #region Prefabs

    public RectTransform WorldDoor { get; set; }
    public RectTransform DoorBlock { get; set; }
    public RectTransform WorldTransition { get; set; }
    public RectTransform WorldScreen { get; set; }
    public RectTransform WorldTile { get; set; }
    public RectTransform Enemy { get; set; }
    public RectTransform Npc { get; set; }
    public RectTransform Item { get; set; }
    public RectTransform Player { get; set; }
    public RectTransform UIHeart { get; set; }

    #endregion

    public Dictionary<ScreenTemplates, ViewModel.Grid> Templates { get; } = new();
    public Dictionary<string, string> Screens { get; } = new();
    private static readonly List<Animations> AnimationsCache = EnumExtensions.GetValues<Animations>();

    public Graphics() {
      LoadAllSprites();
      LoadAllPrefabs();
    }

    public void LoadAllSprites() {
      FileSystem.LoadSprites("character", (response) => {
        PlayerAnimations = GetAnimations(response, "");
        PlayerAnimations[Animations.Entering] = PlayerAnimations[Animations.WalkUp];
        PlayerAnimations[Animations.Exiting] = PlayerAnimations[Animations.WalkDown];
      });
      FileSystem.LoadSprites("tiles", (response) => Tiles.AddRange(response));
      FileSystem.LoadSprites("castle", (response) => Tiles.AddRange(response));
      FileSystem.LoadSprites("items", (response) => { Items = response; });
      FileSystem.LoadSprites("characters", (response) => {
        foreach (Characters character in EnumExtensions.GetValues<Characters>()) {
          string name = character.GetDescription() + "_";
          NpcAnimations.Add(character, GetAnimations(response.Where(x => x.name.Contains(name)).ToList(), name));
        }
      });
      FileSystem.LoadSpritesLabel("Enemies", (response) => {
        foreach (KeyValuePair<Enemies, List<Sprite>> enemy in response) {
          EnemyAnimations.Add(enemy.Key.ToString(), GetAnimations(enemy.Value, ""));
        }
      });
      FileSystem.LoadMaterial("Castle", (response) => { CastleMaterials = response; });
      FileSystem.LoadMaterial("OverworldTiles", (response) => { WorldMaterials = response; });
      FileSystem.LoadJsonByLabel("ScreenTemplates", (response) => {
        foreach (KeyValuePair<string, string> item in response) {
          Enum.TryParse(item.Key, out ScreenTemplates value);
          Templates.Add(value, JsonConvert.DeserializeObject<ViewModel.Grid>(item.Value));
        }
      });
      FileSystem.LoadJsonByLabel("Overworld", (response) => {
        foreach (KeyValuePair<string, string> item in response) {
          Screens.Add($"{Constants.PathOverworld}{item.Key}", item.Value);
        }
      });
    }

    public void LoadAllPrefabs() {
      FileSystem.LoadPrefab("WorldDoor", (response) => { WorldDoor = response; });
      FileSystem.LoadPrefab("DoorBlock", (response) => { DoorBlock = response; });
      FileSystem.LoadPrefab("WorldTransition", (response) => { WorldTransition = response; });
      FileSystem.LoadPrefab("WorldScreen", (response) => { WorldScreen = response; });
      FileSystem.LoadPrefab("Enemy", (response) => { Enemy = response; });
      FileSystem.LoadPrefab("WorldTile", (response) => { WorldTile = response; });
      FileSystem.LoadPrefab("NPC", (response) => { Npc = response; });
      FileSystem.LoadPrefab("Item", (response) => { Item = response; });
      FileSystem.LoadPrefab("Player", (response) => { Player = response; });
      FileSystem.LoadPrefab("HeartTemplate", (response) => { UIHeart = response; });
    }

    private static Dictionary<Animations, List<Sprite>> GetAnimations(List<Sprite> animations, string name) {
      Dictionary<Animations, List<Sprite>> dict = new();
      if (animations.Count == 0) {
        return dict;
      }

      foreach (var anim in AnimationsCache) {
        dict[anim] = new List<Sprite>();
      }

      foreach (var sprite in animations) {
        var spriteName = sprite.name;
        if (name != Empty) {
          spriteName = spriteName.Replace(name, "");
        }

        Enum.TryParse(spriteName, out Animations value);
        switch (value) {
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
            break;
          case Animations.IdleDown:
            dict[value].Add(sprite);
            break;
          case Animations.IdleRight:
            dict[value].Add(sprite);
            break;
          case Animations.IdleLeft:
            dict[value].Add(sprite);
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

      var defaultAnimation = dict[Animations.IdleDown].FirstOrDefault();
      // If there's no defaultAnimation, then that means we have a special enemy, like a Leever, Lanmola, or Moldorm
      if (defaultAnimation != null) {
        if (dict[Animations.IdleUp].Count == 0) {
          dict[Animations.IdleUp].Add(defaultAnimation);
        }

        if (dict[Animations.IdleRight].Count == 0) {
          dict[Animations.IdleRight].Add(defaultAnimation);
        }

        if (dict[Animations.IdleLeft].Count == 0) {
          dict[Animations.IdleLeft].Add(defaultAnimation);
        }

        dict[Animations.WalkDown].Add(defaultAnimation);
        if (dict[Animations.WalkUp].Count == 0) {
          dict[Animations.WalkUp].AddRange(dict[Animations.WalkDown]);
        }
        else {
          dict[Animations.WalkUp].AddRange(dict[Animations.IdleUp]);
        }

        if (dict[Animations.WalkRight].Count == 0) {
          dict[Animations.WalkRight].AddRange(dict[Animations.WalkDown]);
        }
        else {
          dict[Animations.WalkRight].AddRange(dict[Animations.IdleRight]);
        }

        if (dict[Animations.WalkLeft].Count == 0) {
          dict[Animations.WalkLeft].AddRange(dict[Animations.WalkDown]);
        }
        else {
          dict[Animations.WalkLeft].AddRange(dict[Animations.IdleLeft]);
        }
      }

      return dict;
    }

    public Sprite GetItem(string name) {
      return Items.Find(s => s.name == name);
    }

    public Sprite GetTile(Tiles tile) {
      string name = tile.ToString();
      List<Sprite> source = Tiles;
      if (tile == global::Tiles.Block) {
        // source = CastleMaterials;
      }

      return Tiles.Find(s => s.name == name);
    }

    public Sprite GetItem(Items type) {
      return GetItem(type.GetCustomAttr("Resource"));
    }

    public List<Sprite> GetAnimations(Items type) {
      switch (type) {
        case global::Items.Heart:
        case global::Items.HeartAlt:
          return new List<Sprite> {
            GetItem(global::Items.Heart),
            GetItem(global::Items.HeartAlt)
          };
        case global::Items.TriforceShard:
        case global::Items.TriforceShardAlt:
          return new List<Sprite> {
            GetItem(global::Items.TriforceShard),
            GetItem(global::Items.TriforceShardAlt)
          };
        default:
          return new List<Sprite>();
      }
    }
  }
}
