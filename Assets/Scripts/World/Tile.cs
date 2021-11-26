using System.Linq;
using UnityEngine;
using ViewModel;
using Grid = ViewModel.Grid;

namespace World {
  public class Tile : MonoBehaviour {
    public Tiles TileType { get; set; }
    public SpriteRenderer Renderer { get; set; }
    public PolygonCollider2D Collider { get; set; }
    public RectTransform Transform { get; set; }
    public Vector3 QuadSize { get; set; }

    private Grid<GridCell> Grid { get; set; }

    public static Tile Spawn(Vector3 position, Tiles tile, Transform parent, TileChild viewModel, GridCell gridCell, Grid<GridCell> grid) {
      RectTransform transform = Instantiate(Manager.Game.Graphics.WorldTile);
      transform.SetParent(parent);

      Tile worldTile = transform.GetComponent<Tile>();
      worldTile.SetTile(tile, viewModel, grid);
      // TODOJEF: Pick up here... I think I have to somehow use the GridCell.Initialize logic
      // started copying it over into this class, but I'm not sure if that's right?  I at least fixed
      // the positioning... I don't think the AI can walk these cells because we're not initializing that
      // logic in here
      transform.localPosition = position + worldTile.QuadSize * 0.5f;
      transform.rotation = Quaternion.identity;

      return worldTile;
    }

    public bool IsHorizontalCastleWall() {
      switch (TileType) {
        case Tiles.WallLeftX:
        case Tiles.WallRightX:
          return true;
      }

      return false;
    }

    public bool IsSolidWall() {
      switch (TileType) {
        case Tiles.WallX:
        case Tiles.WallY:
          return true;
      }

      return false;
    }

    public bool IsVerticalDoor() {
      switch (TileType) {
        case Tiles.DoorClosedY:
        case Tiles.DoorLockedY:
        case Tiles.DoorUnlockedY:
        case Tiles.WallHoleY:
          return true;
      }

      return false;
    }

    public bool IsHorizontalDoor() {
      switch (TileType) {
        case Tiles.DoorClosedX:
        case Tiles.DoorLockedX:
        case Tiles.DoorUnlockedX:
        case Tiles.WallHoleX:
          return true;
      }

      return false;
    }

    public bool IsVerticalCastleWall() {
      switch (TileType) {
        case Tiles.WallLeftY:
        case Tiles.WallRightY:
        case Tiles.WallRightYFlip:
        case Tiles.WallLeftYFlip:
          return true;
      }

      return false;
    }

    public bool IsTile() {
      switch (TileType) {
        case Tiles.Transition:
        case Tiles.None:
        case Tiles.Door:
          return false;
      }

      return true;
    }

    public bool IsBackgroundTile() {
      switch (TileType) {
        case Tiles.CastleSand:
        case Tiles.SandBottom:
        case Tiles.SandBottomLeft:
        case Tiles.SandBottomRight:
        case Tiles.SandCenter:
        case Tiles.SandCenterLeft:
        case Tiles.SandCenterRight:
        case Tiles.SandTop:
        case Tiles.SandTopLeft:
        case Tiles.SandTopRight:
        case Tiles.GroundTile:
        case Tiles.Dock:
          return true;
      }

      return false;
    }

    public bool IsWater() {
      switch (TileType) {
        case Tiles.PondBottom:
        case Tiles.PondBottomLeft:
        case Tiles.PondBottomRight:
        case Tiles.PondCenter:
        case Tiles.PondCenterLeft:
        case Tiles.PondCenterRight:
        case Tiles.PondTop:
        case Tiles.PondTopLeft:
        case Tiles.PondTopRight:
          return true;
      }

      return false;
    }

    public void SetQuadSize() {
      if (IsVerticalCastleWall()) {
        QuadSize = new Vector2(2f, 4.5f) * Grid.CellSize;
      }
      else if (IsHorizontalCastleWall()) {
        QuadSize = new Vector2(5f, 2f) * Grid.CellSize;
      }
      else if (IsVerticalDoor() || IsHorizontalDoor() || IsSolidWall()) {
        QuadSize = new Vector2(2f, 2f) * Grid.CellSize;
      }
      else if (IsTile()) {
        QuadSize = Vector2.one * Grid.CellSize;
      }
      else {
        QuadSize = Vector2.zero;
      }
    }

    private void Awake() {
      Renderer = GetComponent<SpriteRenderer>();
      Collider = GetComponent<PolygonCollider2D>();
      Transform = GetComponent<RectTransform>();
    }

    public void SetTile(Tiles tile, TileChild viewModel, Grid<GridCell> grid) {
      TileType = tile;
      Grid = grid;
      SetQuadSize();
      Sprite sprite = Manager.Game.Graphics.GetTile(tile.ToString());
      if (sprite != null) {
        Renderer.sprite = Utilities.CloneSprite(sprite, viewModel.ReplaceColors.Select(color => Utilities.HexToColor(color)).ToArray());
        if (sprite != null) {
          Transform.name = sprite.name;
        }
      }
    }
  }
}