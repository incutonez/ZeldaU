using UnityEngine;

public class ScreenTile
{
    public ScreenGrid<ScreenTileViewModel> Grid { get; set; }

    public ScreenTile(int width, int height, float cellSize, Vector3 position)
    {
        Grid = new ScreenGrid<ScreenTileViewModel>(width, height, cellSize, position, (ScreenGrid<ScreenTileViewModel> grid, int x, int y) => new ScreenTileViewModel(grid, x, y));
    }

    public void SetTileMatterType(Vector3 position, Matters matterType)
    {
        ScreenTileViewModel value = Grid.GetValue(position);
        if (value != null)
        {
            value.SetMatterType(matterType);
        }
    }
}

public class ScreenTileViewModel
{
    public Matters MatterType { get; set; }

    private ScreenGrid<ScreenTileViewModel> Grid { get; set; }
    private int X { get; set; }
    private int Y { get; set; }

    public ScreenTileViewModel(ScreenGrid<ScreenTileViewModel> grid, int x, int y)
    {
        Grid = grid;
        X = x;
        Y = y;
    }

    public void SetMatterType(Matters matterType)
    {
        MatterType = matterType;
        Grid.TriggerChange(X, Y);
    }

    public override string ToString()
    {
        return MatterType.ToString();
    }
}
