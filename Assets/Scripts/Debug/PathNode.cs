public class PathNode
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsWalkable { get; set; } = true;

    private ScreenGrid<PathNode> Grid { get; set; }

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
    public PathNode PreviousNode { get; set; }

    public PathNode(ScreenGrid<PathNode> grid, int x, int y)
    {
        Grid = grid;
        X = x;
        Y = y;
    }

    public void CalculateTotalCost()
    {
        TotalCost = WalkCost + DistanceCost;
    }

    public void SetWalkable(bool walkable)
    {
        IsWalkable = walkable;
        Grid.TriggerChange(X, Y);
    }

    public override string ToString()
    {
        return $"{X} {Y}";
    }
}