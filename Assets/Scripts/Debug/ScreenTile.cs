using UnityEngine;

public class ScreenTile
{
    public ScreenGrid<ScreenTileViewModel> Grid { get; set; }

    public ScreenTile(int width, int height, float cellSize, Vector3 position)
    {
        Grid = new ScreenGrid<ScreenTileViewModel>(width, height, cellSize, position, (ScreenGrid<ScreenTileViewModel> grid, int x, int y) => new ScreenTileViewModel(grid, x, y));
    }

    public void SetTileType(Vector3 position, Matters matterType, WorldColors color)
    {
        ScreenTileViewModel viewModel = Grid.GetViewModel(position);
        if (viewModel != null)
        {
            viewModel.Initialize(matterType, color);
        }
    }
}
