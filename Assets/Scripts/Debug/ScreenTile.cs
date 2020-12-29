using UnityEngine;

public class ScreenTile
{
    public ScreenGrid<ScreenTileViewModel> Grid { get; set; }

    public ScreenTile(int width, int height, float cellSize, Vector3 position)
    {
        Grid = new ScreenGrid<ScreenTileViewModel>(width, height, cellSize, position, (ScreenGrid<ScreenTileViewModel> grid, int x, int y) => new ScreenTileViewModel(grid, x, y));
    }

    public void SetTileMatterType(Vector3 position, Matters matterType, WorldColors color)
    {
        ScreenTileViewModel viewModel = Grid.GetViewModel(position);
        if (viewModel != null)
        {
            viewModel.Initialize(matterType, color);
        }
    }
}

public class ScreenTileViewModel
{
    public Matters MatterType { get; set; } = Matters.None;

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

    public void Initialize(Matters matterType, WorldColors color)
    {
        MatterType = matterType;
        Color = color;
        Grid.TriggerChange(X, Y);
    }

    public Color GetColor()
    {
        if (MatterType == Matters.Transition || MatterType == Matters.door)
        {
            Color = WorldColors.Black;
        }
        else if (Color == WorldColors.None)
        {
            return Constants.COLOR_INVISIBLE;
        }
        return Utilities.HexToColor(Color.GetDescription());
    }

    public override string ToString()
    {
        return MatterType.ToString();
    }
}
