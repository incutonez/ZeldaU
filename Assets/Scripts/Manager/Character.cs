using Enums;
using UnityEngine;
using ViewModel;

namespace Manager {
  public static class Character {
    public static RectTransform Spawn(Vector3 position, RectTransform prefab, Transform parent, bool active = true) {
      RectTransform transform = Object.Instantiate(prefab);
      transform.gameObject.SetActive(active);
      transform.SetParent(parent);
      transform.localPosition = position;
      transform.rotation = Quaternion.identity;
      return transform;
    }

    public static World.Player SpawnPlayer(Vector3 position, Transform parent, bool active = false) {
      RectTransform transform = Spawn(position, Game.Graphics.Player, parent, active);
      World.Player worldCharacter = transform.GetComponent<World.Player>();
      worldCharacter.Initialize(new Character<Characters>{Type = Characters.Link});
      return worldCharacter;
    }

    public static void SpawnCharacter(Vector3 position, Characters characterType, Transform parent, bool active = true) {
      RectTransform transform = Spawn(position, Game.Graphics.Npc, parent, active);

      World.Character<Characters> worldCharacter = transform.GetComponent<World.Character<Characters>>();
      worldCharacter.Initialize(new Character<Characters>{Type = characterType});
    }

    public static World.Enemy SpawnEnemy(Vector3 position, Character<Enemies> viewModel, Transform parent, bool active = false) {
      RectTransform transform = Spawn(position, Game.Graphics.Enemy, parent, active);
      // We grab the class's type based on the enum name, and the namespace we use for enemies
      World.Enemy enemy = (World.Enemy) transform.gameObject.AddComponent(EnemyHelper.GetEnemyClass(viewModel.Type));
      if (active) {
        enemy.Initialize(viewModel);
      }

      return enemy;
    }
  }
}
