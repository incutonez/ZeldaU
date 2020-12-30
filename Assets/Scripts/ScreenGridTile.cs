using System.Collections.Generic;
using UnityEngine;

public struct TileUVs
{
    public Vector2 uv00;
    public Vector2 uv11;
}

public class ScreenGridTile
{
    public Matters TileType { get; set; } = Matters.None;

    private ScreenGrid<ScreenGridTile> Grid { get; set; }
    private int X { get; set; }
    private int Y { get; set; }
    private WorldColors Color { get; set; }

    public ScreenGridTile(ScreenGrid<ScreenGridTile> grid, int x, int y)
    {
        Grid = grid;
        X = x;
        Y = y;
    }

    public void Initialize(Matters tileType, WorldColors color)
    {
        // TODOJEF: Potentially cache getter values in here?
        TileType = tileType;
        Color = color;
        Grid.TriggerChange(X, Y);
    }

    public bool IsTile()
    {
        switch (TileType)
        {
            case Matters.Transition:
            case Matters.None:
            case Matters.door:
                return false;
        }
        return true;
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

        if (TileType == Matters.wallTR)
        {
            topRight = Vector2.zero;
        }
        else if (TileType == Matters.wallTL)
        {
            topLeft = Vector2.zero;
        }
        else if (TileType == Matters.wallBR)
        {
            bottomRight = Vector2.zero;
        }
        else if (TileType == Matters.wallBL)
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
        if (GameHandler.TileCoordinates.ContainsKey(TileType) && TileType != Matters.door)
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
        return TileType.ToString();
    }
}