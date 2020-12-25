using NPCs;
using System.Collections.Generic;

public class SceneViewModel
{
    /// <summary>
    /// This is only used for Transition matters... it's the actual name of the transition that you'd like to load.  This is
    /// useful for things that are not in the overworld/underworld, like shops.
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// This is only used for Transition matters... it's the x value for the next screen... if this is a positive value, then
    /// that means this transition is on the right side of the screen, and if it's negative, then it's on the left side
    /// </summary>
    public int x { get; set; }
    /// <summary>
    /// This is only used for Transition matters... it's the y value for the next screen... if this is a positive value, then
    /// that means this transition is on the top of the screen, and if it's negative, then it's on the bottom
    /// </summary>
    public int y { get; set; }
    public WorldColors? accentColor { get; set; }
    public WorldColors? groundColor { get; set; }
    public List<SceneMatterViewModel> matters { get; set; }
    public List<SceneEnemyViewModel> enemies { get; set; }
}

public class SceneEnemyViewModel
{
    List<SceneEnemyChildViewModel> children { get; set; }
    public int count { get; set; }
    public Enemies type { get; set; }
}

public class SceneEnemyChildViewModel
{
    public List<int> coordinates { get; set; }
    public Enemy enemy { get; set; }
}

public class SceneMatterViewModel
{
    public WorldColors? accentColor { get; set; }
    public List<SceneMatterChildViewModel> children { get; set; }
    public Matters type { get; set; }
}

public class SceneMatterChildViewModel
{
    /// <summary>
    /// This is a list of x, y coordinates, and if 4 values are specified, it becomes the max x, y range to keep adding this matter type
    /// </summary>
    public List<float> coordinates { get; set; }
    public Matter matter { get; set; }
    public SceneViewModel transition { get; set; }
}