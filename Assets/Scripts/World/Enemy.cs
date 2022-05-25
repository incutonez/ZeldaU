using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace World {
  /// <summary>
  /// This class represents the Enemy class in the world
  /// </summary>
  public class Enemy : Character<Enemies> {
    private Base.AI AI { get; set; }
    protected Base.Movement Movement { get; set; }
    public WorldColors[] Colors { get; set; } = { };

    private void Awake() {
      AI = GetComponent<Base.AI>();
    }

    public void SetSpeed(float speed = 3f) {
      if (Movement == null) {
        Movement = GetComponent<Base.Movement>();
      }

      Movement.Target = transform.position;
      Movement.Speed = speed;
    }

    // This is called from Initialize in World.Character
    public override void SetAnimationBase() {
      Animation = gameObject.AddComponent<Base.Animation>();
      SetFrameRates();
      List<Color> colors = new();
      var animationKey = CharacterType.ToString();
      // TODOJEF: Need to use default colors when creating the animationKey
      if (Colors != null) {
        for (var i = 0; i < Colors.Length; i += 2) {
          var color2 = Colors[i + 1];
          colors.Add(Colors[i].GetColor());
          colors.Add(color2.GetColor());
          animationKey += color2.ToString();
        }
      }

      Animation.AnimationSprites = EnemyHelper.GetAnimations(CharacterType, animationKey, colors.ToArray());
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      var sword = collision.gameObject.GetComponent<Manager.Sword>();
      if (sword != null) {
        TakeDamage(sword.GetDamage(), sword.GetDamageModifier());
        if (IsDead()) {
          DestroySelf();
        }
      }
    }

    public override void Enable() {
      base.Enable();
      // TODO: Potentially generate random speeds here?
      AI.Reset();
    }

    protected virtual void SetFrameRates() {
      Animation.ActionFrameRate = 0.33f / Movement.Speed;
      Animation.IdleFrameRate = 1f;
      Animation.WalkFrameRate = 0.3f / Movement.Speed;
    }

    // TODOJEF: Come back to this... create statics for these 2 different arrays
    private List<Color> GetDefaultColors() {
      var defaultColors = new List<Color> {WorldColors.RedPure.GetColor(), WorldColors.Black.GetColor()};

      if (CharacterType != Enemies.Gel && CharacterType != Enemies.Keese && CharacterType != Enemies.Zol) {
        defaultColors.Add(WorldColors.RedPure.GetColor());
      }

      return defaultColors;
    }
  }
}
