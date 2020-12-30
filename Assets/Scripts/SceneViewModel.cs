using NPCs;
using System.Collections.Generic;

public class SceneViewModel
{
    /// <summary>
    /// This is only used for Transition matters... it's the actual name of the transition that you'd like to load.  This is
    /// useful for things that are not in the overworld/underworld, like shops.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// This is only used for Transition matters... it's the x value for the next screen... if this is a positive value, then
    /// that means this transition is on the right side of the screen, and if it's negative, then it's on the left side
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// This is only used for Transition matters... it's the y value for the next screen... if this is a positive value, then
    /// that means this transition is on the top of the screen, and if it's negative, then it's on the bottom
    /// </summary>
    public int Y { get; set; }
    public WorldColors? AccentColor { get; set; }
    public WorldColors? GroundColor { get; set; }
    public List<ScreenTileViewModel> Tiles { get; set; }
    public List<SceneEnemyViewModel> Enemies { get; set; }
    public List<SceneCharacterViewModel> Characters { get; set; }
    public List<SceneItemViewModel> Items { get; set; }
}

public class SceneItemViewModel
{
    public List<float> Coordinates { get; set; }
    public Item Item { get; set; }
}

public class SceneCharacterViewModel
{
    public List<float> Coordinates { get; set; }
    public Characters Type { get; set; }
}

public class SceneEnemyViewModel
{
    List<SceneEnemyChildViewModel> Children { get; set; }
    public int Count { get; set; }
    public Enemies Type { get; set; }
}

public class SceneEnemyChildViewModel
{
    public List<float> Coordinates { get; set; }
    public Enemy Enemy { get; set; }
}

public class ScreenTileViewModel
{
    public WorldColors? AccentColor { get; set; }
    public List<ScreenTileChildViewModel> Children { get; set; }
    public Matters Type { get; set; }
}

public class ScreenTileChildViewModel
{
    /// <summary>
    /// This is a list of x, y coordinates, and if 4 values are specified, it becomes the max x, y range to keep adding this matter type
    /// </summary>
    public List<float> Coordinates { get; set; }
    public Matters TileType { get; set; } = Matters.None;
    public SceneViewModel Transition { get; set; }
}