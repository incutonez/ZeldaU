using System;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public struct TileUVs
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    public class GridNode
    {
        public Tiles TileType { get; set; } = Tiles.None;
        public int X { get; set; }
        public int Y { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public GridNode PreviousNode { get; set; }
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

        private Grid<GridNode> Grid { get; set; }

        public GridNode(Grid<GridNode> grid, int x, int y)
        {
            Grid = grid;
            X = x;
            Y = y;
        }

        public void Initialize(Tiles tileType, WorldColors? color, float x, float y, int rotation, bool flipX, bool flipY)
        {
            // TODOJEF: Potentially cache getter values in here?
            // TODO: We can have multiple colors for castles
            Color = color;
            SetTileType(tileType);
            PosX = x;
            PosY = y;
            Rotation = rotation;
            FlipX = flipX;
            FlipY = flipY;
        }

        public void SetTileType(Tiles tileType)
        {
            TileType = tileType;
            Grid.TriggerChange(X, Y);
        }

        public bool IsWalkable()
        {
            return TileType == Tiles.None;
        }

        public bool IsHorizontalCastleWall()
        {
            switch (TileType)
            {
                case Tiles.WallLeftX:
                case Tiles.WallRightX:
                    return true;
            }
            return false;
        }

        public bool IsVerticalDoor()
        {
            switch (TileType)
            {
                case Tiles.DoorClosedY:
                case Tiles.DoorLockedY:
                case Tiles.DoorUnlockedY:
                case Tiles.WallHoleY:
                case Tiles.WallY:
                    return true;
            }
            return false;
        }

        public bool IsHorizontalDoor()
        {
            switch (TileType)
            {
                case Tiles.DoorClosedX:
                case Tiles.DoorLockedX:
                case Tiles.DoorUnlockedX:
                case Tiles.WallHoleX:
                case Tiles.WallX:
                    return true;
            }
            return false;
        }

        public bool IsVerticalCastleWall()
        {
            switch (TileType)
            {
                case Tiles.WallLeftY:
                case Tiles.WallRightY:
                case Tiles.WallRightYFlip:
                case Tiles.WallLeftYFlip:
                    return true;
            }
            return false;
        }

        public bool IsTile()
        {
            switch (TileType)
            {
                case Tiles.Transition:
                case Tiles.None:
                case Tiles.Door:
                    return false;
            }
            return true;
        }

        public bool IsBackgroundTile()
        {
            switch (TileType)
            {
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
                    return true;
            }
            return false;
        }

        public void CalculateTotalCost()
        {
            TotalCost = WalkCost + DistanceCost;
        }

        /// <summary>
        /// A little inefficient with the collider shapes that we add, but it's okay because when we add them, the
        /// composite collider on the Screen prefab takes care of actually creating the outlines based on all of our
        /// polygons in the screen
        /// </summary>
        /// <returns></returns>
        public Vector2[] GetColliderShape()
        {
            if (!IsTile() || IsBackgroundTile())
            {
                return null;
            }

            float cellSize = Grid.CellSize;
            Vector3 position = Grid.GetWorldPosition(X, Y);
            List<Vector2> points = new List<Vector2>();
            Vector2 topLeft = new Vector2
            {
                x = position.x,
                y = position.y + cellSize
            };
            Vector2 topRight = new Vector2
            {
                x = position.x + cellSize,
                y = position.y + cellSize
            };
            Vector2 bottomRight = new Vector2
            {
                x = position.x + cellSize,
                y = position.y
            };
            Vector2 bottomLeft = new Vector2
            {
                x = position.x,
                y = position.y
            };

            if (TileType == Tiles.WallTopRight)
            {
                topRight = Vector2.zero;
            }
            else if (TileType == Tiles.WallTopLeft)
            {
                topLeft = Vector2.zero;
            }
            else if (TileType == Tiles.WallBottomRight)
            {
                bottomRight = Vector2.zero;
            }
            else if (TileType == Tiles.WallBottomLeft)
            {
                bottomLeft = Vector2.zero;
            }

            if (topLeft != Vector2.zero)
            {
                points.Add(topLeft);
            }
            if (topRight != Vector2.zero)
            {
                points.Add(topRight);
            }
            if (bottomRight != Vector2.zero)
            {
                points.Add(bottomRight);
            }
            if (bottomLeft != Vector2.zero)
            {
                points.Add(bottomLeft);
            }
            return points.ToArray();
        }

        public Color? GetColor()
        {
            if (Color.HasValue)
            {
                return Color.GetColor();
            }
            return IsTile() ? null : (Color?) Constants.COLOR_INVISIBLE;
        }

        // TODO: Can cache this?
        public Vector3 GetWorldPosition()
        {
            return Grid.GetWorldPosition(PosX, PosY) + GetQuadSize() * 0.5f;
        }

        public void GetUVCoordinates(out Vector2 uv00, out Vector2 uv11)
        {
            // TODOJEF: Can potentially allow doors here, and then in the box collider just make it a 0?  Would have to fix the quadSize getter
            if (Manager.Game.Graphics.TileCoordinates.ContainsKey(TileType) && TileType != Tiles.Door)
            {
                TileUVs coordinates = Manager.Game.Graphics.TileCoordinates[TileType];
                uv00 = coordinates.uv00;
                uv11 = coordinates.uv11;
            }
            else
            {
                uv00 = Vector2.zero;
                uv11 = Vector2.zero;
            }
        }

        public Vector3 GetQuadSize()
        {
            if (IsVerticalCastleWall())
            {
                return new Vector2(2f, 4.5f) * Grid.CellSize;
            }
            else if (IsHorizontalCastleWall())
            {
                return new Vector2(5f, 2f) * Grid.CellSize;
            }
            else if (IsVerticalDoor())
            {
                return new Vector2(2f, 2f) * Grid.CellSize;
            }
            else if (IsHorizontalDoor())
            {
                return new Vector2(2f, 2f) * Grid.CellSize;
            }
            if (IsTile())
            {
                return Vector2.one * Grid.CellSize;
            }
            return Vector3.zero;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}