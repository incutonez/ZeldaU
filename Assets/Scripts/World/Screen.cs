using Newtonsoft.Json;
using NPCs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    /// <summary>
    /// Idea taken from https://www.youtube.com/watch?v=mZzZXfySeFQ
    /// </summary>
    /// TODO: Use https://www.zeldadungeon.net/the-legend-of-zelda-walkthrough/level-1-the-eagle/ to generate enemies in levels
    public class Screen : MonoBehaviour
    {
        public Color GroundColor { get; set; }
        public Grid<GridNode> Grid { get; set; }
        public bool GridNeedsRefresh { get; set; }
        public string ScreenId { get; set; }

        private Mesh Mesh { get; set; }
        private List<Door> WorldDoors { get; set; }
        private List<Enemy> Enemies { get; set; } = new List<Enemy>();

        public IEnumerator Initialize(string screenId, ViewModel.Grid transition)
        {
            SetGrid(new Grid<GridNode>(Constants.GRID_COLUMNS, Constants.GRID_ROWS, Constants.GRID_CELL_SIZE, Constants.GRID_ORIGIN, (Grid<GridNode> grid, int x, int y) => new GridNode(grid, x, y)));
            WorldDoors = new List<Door>();
            ScreenId = screenId;
            transform.name = screenId;
            ViewModel.Grid scene = null;
            yield return Manager.FileSystem.LoadJson(ScreenId, (response) =>
            {
                scene = JsonConvert.DeserializeObject<ViewModel.Grid>(response);
            });
            if (scene.Template.HasValue && scene.Template.Value != ScreenTemplates.Plain)
            {
                scene.Tiles.AddRange(Manager.Game.Graphics.Templates[scene.Template.Value].Tiles);
            }
            if (Manager.Game.Scene.InCastle)
            {
                scene.Tiles.InsertRange(0, Manager.Game.Graphics.Templates[ScreenTemplates.Base].Tiles);
            }
            GroundColor = (scene.GroundColor ?? WorldColors.Tan).GetColor();
            Build(scene);
            if (transition != null)
            {
                Build(transition);
            }
        }

        public void SetGrid(Grid<GridNode> grid, bool refreshGrid = false)
        {
            Mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = Mesh;
            GetComponent<MeshRenderer>().material = Manager.Game.Scene.InCastle ? Manager.Game.Graphics.CurrentCastleMaterial : Manager.Game.Graphics.WorldMaterials;
            Grid = grid;
            Grid.OnGridValueChanged += Grid_OnValueChanged;
            if (refreshGrid)
            {
                RefreshGrid();
            }
        }

        public List<Vector3> GetRandomPositions(int totalCount)
        {
            System.Random random = new System.Random();
            List<Vector3> randomPositions = new List<Vector3>();
            List<Vector3> openTiles = GetOpenTiles();
            for (int i = 0; i < totalCount; i++)
            {
                int index = random.Next(0, openTiles.Count);
                randomPositions.Add(openTiles[index]);
                openTiles.RemoveAt(index);
            }
            return randomPositions;
        }

        public List<Vector3> GetOpenTiles()
        {
            List<Vector3> result = new List<Vector3>();
            Grid.EachCell((viewModel, x, y) =>
            {
                if (!viewModel.IsTile())
                {
                    result.Add(Grid.GetWorldPosition(x, y));
                }
            });
            return result;
        }

        public void Build(ViewModel.Grid scene)
        {
            if (scene.Tiles != null)
            {
                foreach (ViewModel.Tile screenTile in scene.Tiles)
                {
                    Tiles tileType = screenTile.Type;
                    // Order of priority
                    WorldColors color = screenTile.AccentColor ?? scene.AccentColor ?? WorldColors.White;
                    foreach (ViewModel.TileChild child in screenTile.Children)
                    {
                        List<float> coordinates = child.Coordinates;
                        float x = coordinates[0];
                        float y = coordinates[1];
                        int rotation = child.Rotation;
                        bool flipY = child.FlipY;
                        bool flipX = child.FlipX;
                        float xMax = x;
                        float yMax = y;
                        if (child.TileType != Tiles.None)
                        {
                            tileType = child.TileType;
                        }
                        // If we have an array of 4, then we want to duplicate this tile across that range
                        if (coordinates.Count == 4)
                        {
                            xMax = coordinates[2];
                            yMax = coordinates[3];
                        }
                        for (float i = x; i <= xMax; i++)
                        {
                            for (float j = y; j <= yMax; j++)
                            {
                                Vector3 position = Grid.GetWorldPosition(i, j);
                                if (tileType == Tiles.Door)
                                {
                                    AddDoor(position, child.Transition);
                                }
                                else if (tileType == Tiles.Transition)
                                {
                                    AddTransition(position, child.Transition);
                                }
                                else
                                {
                                    GridNode viewModel = Grid.GetViewModel(position);
                                    if (viewModel != null)
                                    {
                                        // TODO: Will I ever need to pass in a color?
                                        viewModel.Initialize(tileType, color, position, rotation, flipX, flipY);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (scene.Enemies != null)
            {
                foreach (ViewModel.Enemy viewModel in scene.Enemies)
                {
                    Enemies enemyType = viewModel.Type;
                    for (int i = 0; i < viewModel.Count; i++)
                    {
                        Enemy enemy = Manager.Character.SpawnEnemy(Vector3.zero, enemyType, transform);
                        enemy.SetSpeed(viewModel.Speed);
                        enemy.Initialize(enemyType);
                        enemy.OnDestroy += Enemy_OnDestroy;
                        Enemies.Add(enemy);
                    }
                }
            }
            if (scene.Items != null)
            {
                foreach (ViewModel.ItemViewModel item in scene.Items)
                {
                    Item.Spawn(Grid.GetWorldPosition(item.Coordinates[0], item.Coordinates[1]), item.Item, transform);
                }
            }
            if (scene.Characters != null)
            {
                foreach (ViewModel.Character character in scene.Characters)
                {
                    Manager.Character.SpawnCharacter(Grid.GetWorldPosition(character.Coordinates[0], character.Coordinates[1]), character.Type, transform);
                }
            }
        }

        // Randomly spawn enemies when the scene is shown
        public void SpawnEnemies()
        {
            List<Vector3> positions = GetRandomPositions(Enemies.Count);
            for (int i = 0; i < positions.Count; i++)
            {
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
        private void Enemy_OnDestroy(object sender, EventArgs e)
        {
            Enemies.Remove((Enemy) sender);
        }

        public void ToggleActive(bool active = false)
        {
            gameObject.SetActive(active);
            foreach (Enemy enemy in Enemies)
            {
                enemy.Disable();
            }
        }

        public void ToggleDoor(bool active)
        {
            Door door = GetDoor();
            if (door != null)
            {
                door.ToggleHiddenDoor(active);
            }
        }

        public Door GetDoor(int index = 0)
        {
            return WorldDoors[index];
        }

        public void AddDoor(Vector3 position, ViewModel.Grid transition)
        {
            // Because our world has each position as being centered, we have to apply the offset... same
            // as what we do in the AddToMesh method
            Transform door = Instantiate(Manager.Game.Graphics.WorldDoor, GetWorldPositionOffset(position, GetQuadSize()), Quaternion.identity, transform);
            if (door != null)
            {
                Door worldDoor = door.GetComponent<Door>();
                worldDoor.Initialize(GroundColor, transition);
                WorldDoors.Add(worldDoor);
            }
        }

        public void AddTransition(Vector3 position, ViewModel.Grid transition)
        {
            Transform item = Instantiate(Manager.Game.Graphics.WorldTransition, position, Quaternion.identity, transform);
            if (item != null)
            {
                Transition worldItem = item.GetComponent<Transition>();
                worldItem.Initialize(transition);
            }
        }

        public Vector3 GetQuadSize()
        {
            return Vector2.one * Grid.CellSize;
        }

        /// <summary>
        /// This method is a little confusing but because our meshes and sprites are pivoted in the center, but our grid positions
        /// them in the lower left of the cell, we need to adjust and center position the world position
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="quadSize"></param>
        /// <returns></returns>
        public Vector3 GetWorldPositionOffset(Vector3 worldPosition, Vector3 quadSize)
        {
            return worldPosition + quadSize * 0.5f;
        }

        public void RefreshGrid()
        {
            int width = Grid.Width;
            int height = Grid.Height;
            PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
            polygonCollider.pathCount = 0;
            Utilities.CreateEmptyMesh(width * height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles, out Color[] colors2, out Vector3[] normals);
            Grid.EachCell((viewModel, x, y) =>
            {
                // Quads start on the center of each position, so we shift it by the quadSize multiplied by 0.5
                Utilities.AddToMesh(x * height + y, viewModel, vertices, uvs, triangles, colors2, normals);
                Vector2[] colliderShape = viewModel.GetColliderShape();
                if (colliderShape != null)
                {
                    polygonCollider.pathCount++;
                    polygonCollider.SetPath(polygonCollider.pathCount - 1, colliderShape);
                }
            });
            Mesh.Clear();
            Mesh.vertices = vertices;
            Mesh.uv = uvs;
            Mesh.triangles = triangles;
            Mesh.normals = normals;
        }

        private void Grid_OnValueChanged(object sender, Grid<GridNode>.OnGridValueChangedEventArgs e)
        {
            GridNeedsRefresh = true;
        }

        private void LateUpdate()
        {
            if (GridNeedsRefresh)
            {
                GridNeedsRefresh = false;
                RefreshGrid();
            }
        }
    }
}