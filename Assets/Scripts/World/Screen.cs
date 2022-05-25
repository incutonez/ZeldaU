using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace World {
  /// <summary>
  /// Idea taken from https://www.youtube.com/watch?v=mZzZXfySeFQ
  /// </summary>
  /// TODO: Use https://www.zeldadungeon.net/the-legend-of-zelda-walkthrough/level-1-the-eagle/ to generate enemies in levels
  public class Screen : MonoBehaviour {
    public Color? GroundColor { get; private set; }
    public Grid<GridCell> Grid { get; private set; }
    private bool GridNeedsRefresh { get; set; }
    private string ScreenId { get; set; }
    public ViewModel.Grid ViewModel { get; private set; }
    private Mesh Mesh { get; set; }
    private List<Door> WorldDoors { get; set; }
    private List<Enemy> Enemies { get; } = new();

    public IEnumerator Initialize(string screenId, ViewModel.Grid transition) {
      SetGrid(new Grid<GridCell>(Constants.GridColumns, Constants.GridRows, Constants.GridCellSize, Constants.GridOrigin, (grid, x, y) => new GridCell(grid, x, y)));
      WorldDoors = new List<Door>();
      ScreenId = screenId;
      transform.name = screenId;
      if (Manager.Game.Graphics.Screens.ContainsKey(screenId)) {
        ViewModel = JsonConvert.DeserializeObject<ViewModel.Grid>(Manager.Game.Graphics.Screens[screenId]);
      }
      // If we don't have a transition OR the transition isn't floating, then we've got a scene to load
      else if (transition == null || !transition.IsFloating) {
        yield return Manager.FileSystem.LoadJson(ScreenId, (response) => { ViewModel = JsonConvert.DeserializeObject<ViewModel.Grid>(response); });
      }
      /* Otherwise, let's use the transition's template as our base scene... this is for things like shops and castles...
       * they're disconnected from the rest of the world and floating in the nebula */
      else if (transition.Template.HasValue) {
        ViewModel = Manager.Game.Graphics.Templates[transition.Template.Value];
        ViewModel.AccentColor = transition.AccentColor ?? ViewModel.AccentColor;
        ViewModel.GroundColor = transition.GroundColor ?? ViewModel.GroundColor;
      }

      if (ViewModel.Template.HasValue && ViewModel.Template.Value != ScreenTemplates.Plain) {
        ViewModel.Tiles.AddRange(Manager.Game.Graphics.Templates[ViewModel.Template.Value].Tiles);
      }

      if (Manager.Game.Scene.InCastle) {
        // We add the templates at the beginning because it's possible we want to override some of the template tiles
        ViewModel.Tiles.InsertRange(0, Manager.Game.Graphics.Templates[ScreenTemplates.Base].Tiles);
      }

      if (ViewModel.GroundColor.HasValue) {
        GroundColor = ViewModel.GroundColor.GetColor();
      }

      Build(ViewModel);
      if (transition != null) {
        ViewModel.X = transition.X;
        ViewModel.Y = transition.Y;
        ViewModel.IsFloating = transition.IsFloating;
        Build(transition);
      }
    }

    private void SetGrid(Grid<GridCell> grid, bool refreshGrid = false) {
      Mesh = new Mesh();
      GetComponent<MeshFilter>().mesh = Mesh;
      GetComponent<MeshRenderer>().material = Manager.Game.Scene.InCastle ? Manager.Game.Graphics.CurrentCastleMaterial : Manager.Game.Graphics.WorldMaterials;
      Grid = grid;
      Grid.OnGridValueChanged += Grid_OnValueChanged;
      if (refreshGrid) {
        RefreshGrid();
      }
    }

    private IEnumerable<Vector3> GetRandomPositions(int totalCount) {
      var random = new System.Random();
      var randomPositions = new List<Vector3>();
      var openTiles = GetOpenTiles();
      for (var i = 0; i < totalCount; i++) {
        var index = random.Next(0, openTiles.Count);
        randomPositions.Add(openTiles[index]);
        openTiles.RemoveAt(index);
      }

      return randomPositions;
    }

    private List<Vector3> GetOpenTiles() {
      List<Vector3> result = new();
      Grid.EachCell((viewModel, x, y) => {
        if (!viewModel.IsTile()) {
          result.Add(Grid.GetWorldPosition(x, y));
        }
      });
      return result;
    }

    private void Build(ViewModel.Grid scene) {
      if (scene.Tiles != null) {
        // Order of priority
        var worldAccentColor = scene.AccentColor;
        foreach (var screenTile in scene.Tiles) {
          var tileType = screenTile.Type;
          foreach (var tileChild in screenTile.Children) {
            var position = Grid.GetWorldPosition(tileChild.X, tileChild.Y);
            var gridCell = Grid.GetViewModel(position);
            if (tileType == Tiles.Door) {
              AddDoor(gridCell.CenterPosition, tileChild.Transition);
            }
            else if (tileType == Tiles.Transition) {
              AddTransition(position, tileChild.Transition);
            }
            else {
              Tile.Spawn(tileType, transform, tileChild, gridCell, worldAccentColor);
              /* TODO: https://forum.unity.com/threads/replace-multiple-colors-in-mesh-uvs.1205110/#post-7729752 should help
               * Revisit this... basically, I removed the mesh based generation and replaced with
               * adding sprites directly in... this allows us to change the sprite colors, whereas the meshes
               * wouldn't allow it... maybe I just don't know enough about shaders? */
              // viewModel.Initialize(tileType, child.ReplaceColors, position, rotation, flipX, flipY);
            }
          }
        }
      }

      if (scene.Enemies != null) {
        foreach (var viewModel in scene.Enemies) {
          var enemyType = viewModel.Type;
          for (var i = 0; i < viewModel.Count; i++) {
            var enemy = Manager.Character.SpawnEnemy(Grid.GetWorldPosition(viewModel.X, viewModel.Y), enemyType, transform);
            enemy.Colors = viewModel.Colors;
            enemy.SetSpeed(viewModel.Speed);
            enemy.Initialize(enemyType);
            enemy.OnDestroy += Enemy_OnDestroy;
            Enemies.Add(enemy);
          }
        }
      }

      if (scene.Items != null) {
        foreach (var item in scene.Items) {
          Item.Spawn(Grid.GetWorldPosition(item.X, item.Y), item.Config, transform);
        }
      }

      if (scene.Characters != null) {
        foreach (var character in scene.Characters) {
          Manager.Character.SpawnCharacter(Grid.GetWorldPosition(character.Coordinates[0], character.Coordinates[1]), character.Type, transform);
        }
      }
    }

    // Spawn enemies when the scene is shown
    public void SpawnEnemies() {
      foreach (var enemy in Enemies) {
        if (enemy.transform.position == Vector3.zero) {
          enemy.transform.position = GetRandomPositions(Enemies.Count).First();
        }

        enemy.Enable();
      }
    }

    /// <summary>
    /// When the enemy is destroyed, we have to remove it from the world, so we don't try to re-spawn it when the screen is loaded
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Enemy_OnDestroy(object sender, EventArgs e) {
      Enemies.Remove((Enemy) sender);
    }

    public void ToggleActive(bool active = false) {
      gameObject.SetActive(active);
      foreach (var enemy in Enemies) {
        enemy.Disable();
      }
    }

    public void ToggleDoor(bool active) {
      var door = GetDoor();
      if (door != null) {
        door.ToggleHiddenDoor(active);
      }
    }

    private Door GetDoor(int index = 0) {
      return WorldDoors[index];
    }

    // TODO: Can potentially move this to the Tile.cs code and add the Door.cs script if it's a Door?
    private void AddDoor(Vector3 position, ViewModel.Grid transition) {
      // Because our world has each position as being centered, we have to apply the offset... same
      // as what we do in the AddToMesh method
      Transform door = Instantiate(Manager.Game.Graphics.WorldDoor, position, Quaternion.identity, transform);
      if (door != null) {
        var worldDoor = door.GetComponent<Door>();
        if (GroundColor.HasValue) {
          worldDoor.Initialize(GroundColor.Value, transition);
        }

        WorldDoors.Add(worldDoor);
      }
    }

    // TODO: Can potentially move this to the Tile.cs code and add the Transition.cs script if it's a transition?
    private void AddTransition(Vector3 position, ViewModel.Grid transition) {
      Transform item = Instantiate(Manager.Game.Graphics.WorldTransition, position, Quaternion.identity, transform);
      if (item != null) {
        var worldItem = item.GetComponent<Transition>();
        worldItem.Initialize(transition);
      }
    }

    private void RefreshGrid() {
      var polygonCollider = GetComponent<PolygonCollider2D>();
      polygonCollider.pathCount = 0;
      Grid.EachCell((viewModel, x, y) => {
        var colliderShape = viewModel.GetColliderShape();
        if (colliderShape != null) {
          polygonCollider.SetPath(++polygonCollider.pathCount - 1, colliderShape);
        }
      });
    }

    private void Grid_OnValueChanged(object sender, Grid<GridCell>.OnGridValueChangedEventArgs e) {
      GridNeedsRefresh = true;
    }

    private void LateUpdate() {
      if (GridNeedsRefresh) {
        GridNeedsRefresh = false;
        RefreshGrid();
      }
    }
  }
}
