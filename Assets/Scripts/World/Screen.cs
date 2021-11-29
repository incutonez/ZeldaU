using Newtonsoft.Json;
using NPCs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World {
  /// <summary>
  /// Idea taken from https://www.youtube.com/watch?v=mZzZXfySeFQ
  /// </summary>
  /// TODO: Use https://www.zeldadungeon.net/the-legend-of-zelda-walkthrough/level-1-the-eagle/ to generate enemies in levels
  public class Screen : MonoBehaviour {
    public Color? GroundColor { get; set; }
    public Grid<GridCell> Grid { get; set; }
    public bool GridNeedsRefresh { get; set; }
    public string ScreenId { get; set; }
    public ViewModel.Grid ViewModel { get; set; }

    private Mesh Mesh { get; set; }
    private List<Door> WorldDoors { get; set; }
    private List<Enemy> Enemies { get; set; } = new List<Enemy>();

    public IEnumerator Initialize(string screenId, ViewModel.Grid transition) {
      SetGrid(new Grid<GridCell>(Constants.GridColumns, Constants.GridRows, Constants.GridCellSize, Constants.GridOrigin, (Grid<GridCell> grid, int x, int y) => new GridCell(grid, x, y)));
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

    public void SetGrid(Grid<GridCell> grid, bool refreshGrid = false) {
      Mesh = new Mesh();
      GetComponent<MeshFilter>().mesh = Mesh;
      GetComponent<MeshRenderer>().material = Manager.Game.Scene.InCastle ? Manager.Game.Graphics.CurrentCastleMaterial : Manager.Game.Graphics.WorldMaterials;
      Grid = grid;
      Grid.OnGridValueChanged += Grid_OnValueChanged;
      if (refreshGrid) {
        RefreshGrid();
      }
    }

    public List<Vector3> GetRandomPositions(int totalCount) {
      System.Random random = new System.Random();
      List<Vector3> randomPositions = new List<Vector3>();
      List<Vector3> openTiles = GetOpenTiles();
      for (int i = 0; i < totalCount; i++) {
        int index = random.Next(0, openTiles.Count);
        randomPositions.Add(openTiles[index]);
        openTiles.RemoveAt(index);
      }

      return randomPositions;
    }

    public List<Vector3> GetOpenTiles() {
      List<Vector3> result = new List<Vector3>();
      Grid.EachCell((viewModel, x, y) => {
        if (!viewModel.IsTile()) {
          result.Add(Grid.GetWorldPosition(x, y));
        }
      });
      return result;
    }

    public void Build(ViewModel.Grid scene) {
      if (scene.Tiles != null) {
        // Order of priority
        WorldColors? worldAccentColor = scene.AccentColor;
        foreach (ViewModel.Tile screenTile in scene.Tiles) {
          Tiles tileType = screenTile.Type;
          foreach (ViewModel.TileChild tileChild in screenTile.Children) {
            List<float> coordinates = tileChild.Coordinates;
            float x = coordinates[0];
            float y = coordinates[1];
            // int rotation = tileChild.Rotation;
            // bool flipY = tileChild.FlipY;
            // bool flipX = tileChild.FlipX;
            float xMax = x;
            float yMax = y;
            if (tileChild.TileType != Tiles.None) {
              tileType = tileChild.TileType;
            }

            // If we have an array of 4, then we want to duplicate this tile across that range
            if (coordinates.Count == 4) {
              xMax = coordinates[2];
              yMax = coordinates[3];
            }

            for (float i = x; i <= xMax; i++) {
              for (float j = y; j <= yMax; j++) {
                Vector3 position = Grid.GetWorldPosition(i, j);
                if (tileType == Tiles.Door) {
                  AddDoor(position, tileChild.Transition);
                }
                else if (tileType == Tiles.Transition) {
                  AddTransition(position, tileChild.Transition);
                }
                else {
                  GridCell gridCell = Grid.GetViewModel(position);
                  if (gridCell != null) {
                    Tile.Spawn(position, tileType, transform, tileChild, gridCell, worldAccentColor);
                    /* TODOJEF: Revisit this... basically, I removed the mesh based generation and replaced with
                     * adding sprites directly in... this allows us to change the sprite colors, whereas the meshes
                     * wouldn't allow it... maybe I just don't know enough about shaders? */
                    // viewModel.Initialize(tileType, child.ReplaceColors, position, rotation, flipX, flipY);
                  }
                }
              }
            }
          }
        }
      }

      if (scene.Enemies != null) {
        foreach (ViewModel.Enemy viewModel in scene.Enemies) {
          Enemies enemyType = viewModel.Type;
          for (int i = 0; i < viewModel.Count; i++) {
            Enemy enemy = Manager.Character.SpawnEnemy(Vector3.zero, enemyType, transform);
            enemy.SetSpeed(viewModel.Speed);
            enemy.Initialize(enemyType);
            enemy.OnDestroy += Enemy_OnDestroy;
            Enemies.Add(enemy);
          }
        }
      }

      if (scene.Items != null) {
        foreach (ViewModel.ItemViewModel item in scene.Items) {
          Item.Spawn(Grid.GetWorldPosition(item.Coordinates[0], item.Coordinates[1]), item.Item, transform);
        }
      }

      if (scene.Characters != null) {
        foreach (ViewModel.Character character in scene.Characters) {
          Manager.Character.SpawnCharacter(Grid.GetWorldPosition(character.Coordinates[0], character.Coordinates[1]), character.Type, transform);
        }
      }
    }

    // Randomly spawn enemies when the scene is shown
    public void SpawnEnemies() {
      List<Vector3> positions = GetRandomPositions(Enemies.Count);
      for (int i = 0; i < positions.Count; i++) {
        Enemy enemy = Enemies[i];
        enemy.transform.position = positions[i];
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
      foreach (Enemy enemy in Enemies) {
        enemy.Disable();
      }
    }

    public void ToggleDoor(bool active) {
      Door door = GetDoor();
      if (door != null) {
        door.ToggleHiddenDoor(active);
      }
    }

    public Door GetDoor(int index = 0) {
      return WorldDoors[index];
    }

    // TODOJEF: Can potentially move this to the Tile.cs code and add the Door.cs script if it's a Door?
    public void AddDoor(Vector3 position, ViewModel.Grid transition) {
      // Because our world has each position as being centered, we have to apply the offset... same
      // as what we do in the AddToMesh method
      Transform door = Instantiate(Manager.Game.Graphics.WorldDoor, GetWorldPositionOffset(position, GetQuadSize()), Quaternion.identity, transform);
      if (door != null) {
        Door worldDoor = door.GetComponent<Door>();
        worldDoor.Initialize(GroundColor.Value, transition);
        WorldDoors.Add(worldDoor);
      }
    }

    // TODOJEF: Can potentially move this to the Tile.cs code and add the Transition.cs script if it's a transition?
    public void AddTransition(Vector3 position, ViewModel.Grid transition) {
      Transform item = Instantiate(Manager.Game.Graphics.WorldTransition, position, Quaternion.identity, transform);
      if (item != null) {
        Transition worldItem = item.GetComponent<Transition>();
        worldItem.Initialize(transition);
      }
    }

    public Vector3 GetQuadSize() {
      return Vector2.one * Grid.CellSize;
    }

    /// <summary>
    /// This method is a little confusing but because our meshes and sprites are pivoted in the center, but our grid positions
    /// them in the lower left of the cell, we need to adjust and center position the world position
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="quadSize"></param>
    /// <returns></returns>
    public Vector3 GetWorldPositionOffset(Vector3 worldPosition, Vector3 quadSize) {
      return worldPosition + quadSize * 0.5f;
    }

    public void RefreshGrid() {
      // int width = Grid.Width;
      // int height = Grid.Height;
      PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
      polygonCollider.pathCount = 0;
      // TODOJEF: This is no longer used, right?  Check if AI can still walk
      // Utilities.CreateEmptyMesh(width * height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles, out Color[] colors, out Vector3[] normals);
      Grid.EachCell((viewModel, x, y) => {
        // TODO: Potentially don't generate meshes for non-view models?
        // Quads start on the center of each position, so we shift it by the quadSize multiplied by 0.5
        // Utilities.AddToMesh(x * height + y, viewModel, vertices, uvs, triangles, colors, normals);
        Vector2[] colliderShape = viewModel.GetColliderShape();
        if (colliderShape != null) {
          polygonCollider.SetPath(++polygonCollider.pathCount - 1, colliderShape);
        }
      });
      // Mesh.Clear();
      // Mesh.vertices = vertices;
      // Mesh.uv = uvs;
      // Mesh.triangles = triangles;
      // Mesh.colors = colors;
      // Mesh.normals = normals;
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