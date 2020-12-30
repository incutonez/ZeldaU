using System.Collections.Generic;
using UnityEngine;

public struct TileUVs
{
    public Vector2 uv00;
    public Vector2 uv11;
}

public class ScreenTileViewModel
{
    public Matters TileType { get; set; } = Matters.None;

    private ScreenGrid<ScreenTileViewModel> Grid { get; set; }
    private int X { get; set; }
    private int Y { get; set; }
    private WorldColors Color { get; set; }

    public ScreenTileViewModel(ScreenGrid<ScreenTileViewModel> grid, int x, int y)
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

    public Vector2[] GetCollider()
    {
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

        if (TileType == Matters.None)
        {
            return null;
        }

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
        if (TileType == Matters.Transition || TileType == Matters.door)
        {
            Color = WorldColors.Black;
        }
        else if (Color == WorldColors.None)
        {
            return Constants.COLOR_INVISIBLE;
        }
        return Utilities.HexToColor(Color.GetDescription());
    }

    public float GetRotation()
    {
        return 0f;
    }

    public Vector3 GetWorldPosition()
    {
        return Grid.GetWorldPosition(X, Y) + GetQuadSize() * 0.5f;
    }

    public void GetCoordinates(out Vector2 uv00, out Vector2 uv11)
    {
        if (GameHandler.TileCoordinates.ContainsKey(TileType) && TileType != Matters.door)
        {
            TileUVs coords = GameHandler.TileCoordinates[TileType];
            uv00 = coords.uv00;
            uv11 = coords.uv11;
        }
        else
        {
            uv00 = Vector2.zero;
            uv11 = Vector2.zero;
        }
    }

    public Vector3 GetQuadSize()
    {
        switch (TileType)
        {
            case Matters.door:
            case Matters.None:
                return Vector3.zero;
        }
        return Vector2.one * Grid.CellSize;
    }

    public bool IsDoor()
    {
        return TileType == Matters.door;
    }

    public override string ToString()
    {
        return TileType.ToString();
    }
}