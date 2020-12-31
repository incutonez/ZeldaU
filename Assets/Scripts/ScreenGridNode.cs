using System.Collections.Generic;
using UnityEngine;

public struct TileUVs
{
    public Vector2 uv00;
    public Vector2 uv11;
}

public class ScreenGridNode
{
    public Tiles TileType { get; set; } = Tiles.None;
    public int X { get; set; }
    public int Y { get; set; }
    public ScreenGridNode PreviousNode { get; set; }
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
    public WorldColors Color { get; set; }

    private ScreenGrid<ScreenGridNode> Grid { get; set; }

    public ScreenGridNode(ScreenGrid<ScreenGridNode> grid, int x, int y)
    {
        Grid = grid;
        X = x;
        Y = y;
    }

    public void Initialize(Tiles tileType, WorldColors color)
    {
        // TODOJEF: Potentially cache getter values in here?
        Color = color;
        SetTileType(tileType);
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
        if (!IsTile())
        {
            return null;
        }

        float cellSize = Grid.CellSize;
        Vector3 position = Grid.GetWorldPosition(X, Y);
        List<Vector2> points = new List<Vector2>();
        Vector2 topLeft = new Vector2 {
            x = position.x,
            y = position.y + cellSize
        };
        Vector2 topRight = new Vector2 {
            x = position.x + cellSize,
            y = position.y + cellSize
        };
        Vector2 bottomRight = new Vector2 {
            x = position.x + cellSize,
            y = position.y
        };
        Vector2 bottomLeft = new Vector2 {
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

    public Color GetColor()
    {
        if (IsTile())
        {
            return Utilities.HexToColor(Color.GetDescription());
        }
        return Constants.COLOR_INVISIBLE;
    }

    public float GetRotation()
    {
        return 0f;
    }

    public Vector3 GetWorldPosition()
    {
        return Grid.GetWorldPosition(X, Y) + GetQuadSize() * 0.5f;
    }

    public void GetUVCoordinates(out Vector2 uv00, out Vector2 uv11)
    {
        // TODOJEF: Can potentially allow doors here, and then in the box collider just make it a 0?  Would have to fix the quadSize getter
        if (GameHandler.TileCoordinates.ContainsKey(TileType) && TileType != Tiles.Door)
        {
            TileUVs coordinates = GameHandler.TileCoordinates[TileType];
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