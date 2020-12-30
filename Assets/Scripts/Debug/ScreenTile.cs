using UnityEngine;

public class ScreenTile
{
    public ScreenGrid<ScreenGridTile> Grid { get; set; }

    public ScreenTile(int width, int height, float cellSize, Vector3 position)
    {
        Grid = new ScreenGrid<ScreenGridTile>(width, height, cellSize, position, (ScreenGrid<ScreenGridTile> grid, int x, int y) => new ScreenGridTile(grid, x, y));
    }

    public void SetTileType(Vector3 position, Matters matterType, WorldColors color)
    {
        ScreenGridTile viewModel = Grid.GetViewModel(position);
        if (viewModel != null)
        {
            viewModel.Initialize(matterType, color);
        }
    }
}
