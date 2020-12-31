using UnityEngine;

public class ScreenTile
{
    public ScreenGrid<ScreenGridNode> Grid { get; set; }

    public ScreenTile(int width, int height, float cellSize, Vector3 position)
    {
        Grid = new ScreenGrid<ScreenGridNode>(width, height, cellSize, position, (ScreenGrid<ScreenGridNode> grid, int x, int y) => new ScreenGridNode(grid, x, y));
    }

    public void SetTileType(Vector3 position, Tiles matterType, WorldColors color)
    {
        ScreenGridNode viewModel = Grid.GetViewModel(position);
        if (viewModel != null)
        {
            viewModel.Initialize(matterType, color);
        }
    }
}
