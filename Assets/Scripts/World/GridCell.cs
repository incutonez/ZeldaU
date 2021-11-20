using System;
using System.Collections.Generic;
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
    public Vector3 CenterPosition { get; set; }
    public Vector3 QuadSize { get; set; }
    public Vector2 UV00 { get; set; }
    public Vector2 UV11 { get; set; }

    private Grid<GridCell> Grid { get; set; }

    public GridCell(Grid<GridCell> grid, int x, int y) {
      Grid = grid;
      X = x;
      Y = y;
    }

    public void Initialize(Tiles tileType, WorldColors? color, Vector3 position, int rotation, bool flipX, bool flipY) {
      // TODO: We can have multiple colors for castles
      // TODOJEF: Pick up here... need to figure out how to change castle colors... working on Q1C3
      Color = color;
      SetTileType(tileType);
      SetQuadSize();
      SetUVCoordinates();
      Rotation = rotation;
      FlipX = flipX;
      FlipY = flipY;
      WorldPosition = position;
      CenterPosition = WorldPosition + QuadSize * 0.5f;
    }

    public void SetTileType(Tiles tileType) {
      TileType = tileType;
      Grid.TriggerChange(X, Y);
    }

    public bool IsWalkable() {
      return TileType == Tiles.None;
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

    public void CalculateTotalCost() {
      TotalCost = WalkCost + DistanceCost;
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

      float xPos = WorldPosition.x;
      float yPos = WorldPosition.y;
      List<Vector2> points = new List<Vector2>();
      Vector2 topLeft = new Vector2 {
        x = xPos,
        y = yPos + QuadSize.y
      };
      Vector2 topRight = new Vector2 {
        x = xPos + QuadSize.x,
        y = yPos + QuadSize.y
      };
      Vector2 bottomRight = new Vector2 {
        x = xPos + QuadSize.x,
        y = yPos
      };
      Vector2 bottomLeft = new Vector2 {
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
          x = xPos + QuadSize.x,
          y = yPos + 0.5f
        };
        bottomRight = new Vector2 {
          x = xPos + QuadSize.x,
          y = yPos
        };
        bottomLeft = new Vector2 {
          x = xPos,
          y = yPos
        };
        // Top collision for gap
        points.AddRange(new List<Vector2> {
          new Vector2 {x = xPos, y = yPos + QuadSize.y - 0.5f},
          new Vector2 {x = xPos + QuadSize.x, y = yPos + QuadSize.y - 0.5f},
          new Vector2 {x = xPos + QuadSize.x, y = yPos + QuadSize.y},
          new Vector2 {x = xPos, y = yPos + QuadSize.y}
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
          y = yPos + QuadSize.y
        };
        bottomRight = new Vector2 {
          x = xPos,
          y = yPos + QuadSize.y
        };
        bottomLeft = new Vector2 {
          x = xPos,
          y = yPos
        };
        // Top collision for gap
        points.AddRange(new List<Vector2> {
          new Vector2 {x = xPos + QuadSize.x - 0.5f, y = yPos},
          new Vector2 {x = xPos + QuadSize.x - 0.5f, y = yPos + QuadSize.y},
          new Vector2 {x = xPos + QuadSize.x, y = yPos + QuadSize.y},
          new Vector2 {x = xPos + QuadSize.x, y = yPos}
        });
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

      return IsTile() ? null : (Color?) Constants.ColorInvisible;
    }

    public void SetUVCoordinates() {
      // TODOJEF: Can potentially allow doors here, and then in the box collider just make it a 0?  Would have to fix the quadSize getter
      if (Manager.Game.Graphics.TileCoordinates.ContainsKey(TileType) && TileType != Tiles.Door) {
        TileUVs coordinates = Manager.Game.Graphics.TileCoordinates[TileType];
        UV00 = coordinates.uv00;
        UV11 = coordinates.uv11;
      }
      else {
        UV00 = Vector2.zero;
        UV11 = Vector2.zero;
      }
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

    public override string ToString() {
      return string.Empty;
    }
  }
}