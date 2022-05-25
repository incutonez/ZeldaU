using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace World {
  public struct TileUVs {
    public Vector2 uv00;
    public Vector2 uv11;
  }

  public class GridCell {
    public Tiles TileType { get; set; } = Tiles.None;
    public int X { get; set; }
    public int Y { get; set; }
    public GridCell PreviousCell { get; set; }

    /// <summary>
    /// Also known as g cost
    /// </summary>
    public int WalkCost { get; set; }

    /// <summary>
    /// Also known as h cost (or heurstic cost)
    /// </summary>
    public int DistanceCost { get; set; }

    /// <summary>
    /// Also known as f cost
    /// </summary>
    public int TotalCost { get; set; }

    public WorldColors? Color { get; set; }
    public int Rotation { get; set; }
    public bool FlipY { get; set; }
    public bool FlipX { get; set; }

    public Vector3 WorldPosition { get; set; }

    // TODO: Remove??  I was using it to set the tile's location, but I've since added a change to
    // GetWorldPosition to account for all cells
    public Vector3 CenterPosition { get; set; }
    public Vector3 QuadSize { get; set; }
    public Vector2 UV00 { get; set; }
    public Vector2 UV11 { get; set; }

    private Grid<GridCell> Grid { get; set; }

    public GridCell(Grid<GridCell> grid, int x, int y) {
      Grid = grid;
      X = x;
      Y = y;
      WorldPosition = grid.GetWorldPosition(X, Y);
      Initialize();
    }

    private void Initialize() {
      SetQuadSize();
      /* We have to set this because the QuadSize helps with scaling the positioning of the tile... for the
       * most part, this value is 1.  We then multiply by 0.5f because we want our tile position to be centered,
       * and if we don't do this, it messes up the pivot and any collisions. */
      CenterPosition = WorldPosition + QuadSize * Constants.PivotOffset;
    }

    public void SetTileType(Tiles tileType) {
      TileType = tileType;
      Initialize();
      Grid.TriggerChange(X, Y);
    }

    public bool IsWalkable() {
      return TileType == Tiles.None;
    }

    private bool IsHorizontalCastleWall() {
      switch (TileType) {
        case Tiles.WallLeftX:
        case Tiles.WallRightX:
          return true;
      }

      return false;
    }

    private bool IsSolidWall() {
      switch (TileType) {
        case Tiles.WallX:
        case Tiles.WallY:
          return true;
      }

      return false;
    }

    private bool IsVerticalDoor() {
      switch (TileType) {
        case Tiles.DoorClosedY:
        case Tiles.DoorLockedY:
        case Tiles.DoorUnlockedY:
        case Tiles.WallHoleY:
          return true;
      }

      return false;
    }

    private bool IsHorizontalDoor() {
      switch (TileType) {
        case Tiles.DoorClosedX:
        case Tiles.DoorLockedX:
        case Tiles.DoorUnlockedX:
        case Tiles.WallHoleX:
          return true;
      }

      return false;
    }

    private bool IsVerticalCastleWall() {
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

    private bool IsBackgroundTile() {
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

    private void CalculateTotalCost() {
      TotalCost = WalkCost + DistanceCost;
    }

    /// <summary>
    /// This method is used when we're prepping the cells for calculating a distance to the requested path.
    /// We initialize all WalkCosts to the highest int value, so we can determine what's been visited and what hasn't
    /// </summary>
    public void ResetCost() {
      WalkCost = int.MaxValue;
      CalculateTotalCost();
      PreviousCell = null;
    }

    public void SetCosts(int walkCost, int distanceCost) {
      WalkCost = walkCost;
      DistanceCost = distanceCost;
      CalculateTotalCost();
    }

    /// <summary>
    /// A little inefficient with the collider shapes that we add, but it's okay because when we add them, the
    /// composite collider on the Screen prefab takes care of actually creating the outlines based on all of our
    /// polygons in the screen
    /// </summary>
    /// <returns></returns>
    public Vector2[] GetColliderShape() {
      if (!IsTile() || IsBackgroundTile()) {
        return null;
      }

      var xPos = WorldPosition.x;
      var yPos = WorldPosition.y;
      var xPosTop = xPos + QuadSize.x;
      var yPosTop = yPos + QuadSize.y;
      List<Vector2> points = new();
      var topLeft = new Vector2 {
        x = xPos,
        y = yPosTop
      };
      var topRight = new Vector2 {
        x = xPosTop,
        y = yPosTop
      };
      var bottomRight = new Vector2 {
        x = xPosTop,
        y = yPos
      };
      var bottomLeft = new Vector2 {
        x = xPos,
        y = yPos
      };

      // For walls that have slants, let's just zero out that part of the triangle
      if (TileType == Tiles.WallTopRight) {
        topRight = Vector2.zero;
      }
      else if (TileType == Tiles.WallTopLeft) {
        topLeft = Vector2.zero;
      }
      else if (TileType == Tiles.WallBottomRight) {
        bottomRight = Vector2.zero;
      }
      else if (TileType == Tiles.WallBottomLeft) {
        bottomLeft = Vector2.zero;
      }
      // For doors, we have to create a gap in between
      else if (IsVerticalDoor()) {
        // Bottom collision for gap
        topLeft = new Vector2 {
          x = xPos,
          y = yPos + 0.5f
        };
        topRight = new Vector2 {
          x = xPosTop,
          y = yPos + 0.5f
        };
        bottomRight = new Vector2 {
          x = xPosTop,
          y = yPos
        };
        bottomLeft = new Vector2 {
          x = xPos,
          y = yPos
        };
        // Top collision for gap
        points.AddRange(new List<Vector2> {
          new() {x = xPos, y = yPosTop - 0.5f},
          new() {x = xPosTop, y = yPosTop - 0.5f},
          new() {x = xPosTop, y = yPosTop},
          new() {x = xPos, y = yPosTop}
        });
      }
      // For doors, we have to create a gap in between
      else if (IsHorizontalDoor()) {
        // Bottom collision for gap
        topLeft = new Vector2 {
          x = xPos + 0.5f,
          y = yPos
        };
        topRight = new Vector2 {
          x = xPos + 0.5f,
          y = yPosTop
        };
        bottomRight = new Vector2 {
          x = xPos,
          y = yPosTop
        };
        bottomLeft = new Vector2 {
          x = xPos,
          y = yPos
        };
        // Top collision for gap
        points.Add(new Vector2 {x = xPosTop - 0.5f, y = yPos});
        points.Add(new Vector2 {x = xPosTop - 0.5f, y = yPosTop});
        points.Add(new Vector2 {x = xPosTop, y = yPosTop});
        points.Add(new Vector2 {x = xPosTop, y = yPos});
      }

      if (topLeft != Vector2.zero) {
        points.Add(topLeft);
      }

      if (topRight != Vector2.zero) {
        points.Add(topRight);
      }

      if (bottomRight != Vector2.zero) {
        points.Add(bottomRight);
      }

      if (bottomLeft != Vector2.zero) {
        points.Add(bottomLeft);
      }

      return points.ToArray();
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

    public Color? GetColor() {
      // TODO: Why is this necessary?
      if (TileType == Tiles.Fire || IsWater()) {
        return UnityEngine.Color.white;
      }

      if (Color.HasValue) {
        return Color.GetColor();
      }

      return IsTile() ? null : Constants.ColorInvisible;
    }

    private void SetQuadSize() {
      if (IsVerticalCastleWall()) {
        // TODO: Create static value for this Vector2
        QuadSize = new Vector2(2f, 4.5f) * Grid.CellSize;
      }
      else if (IsHorizontalCastleWall()) {
        // TODO: Create static value for this Vector2
        QuadSize = new Vector2(5f, 2f) * Grid.CellSize;
      }
      else if (IsVerticalDoor() || IsHorizontalDoor() || IsSolidWall()) {
        // TODO: Create static value for this Vector2
        QuadSize = new Vector2(2f, 2f) * Grid.CellSize;
      }
      else {
        QuadSize = Vector2.one * Grid.CellSize;
      }
    }

    public override string ToString() {
      return string.Empty;
    }
  }
}
