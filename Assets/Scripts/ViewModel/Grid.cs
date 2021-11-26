using NPCs;
using System.Collections.Generic;

namespace ViewModel {
  public class Grid {
    /// <summary>
    /// This is only used for Transition matters... it's the actual name of the transition that you'd like to load.  This is
    /// useful for things that are not in the overworld/underworld, like shops.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// This is only used for Transition matters... it's the x value for the next screen... if this is a positive value, then
    /// that means this transition is on the right side of the screen, and if it's negative, then it's on the left side.
    /// 
    /// When this is set in a castle file, it means it's the starting x coordinate.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// This is only used for Transition matters... it's the y value for the next screen... if this is a positive value, then
    /// that means this transition is on the top of the screen, and if it's negative, then it's on the bottom
    /// 
    /// When this is set in a castle file, it means it's the starting y coordinate... almost always 0.
    /// </summary>
    public int Y { get; set; }

    public WorldColors? AccentColor { get; set; }
    public WorldColors? GroundColor { get; set; }
    public List<Tile> Tiles { get; set; }
    public List<Enemy> Enemies { get; set; }
    public List<Character> Characters { get; set; }
    public List<ItemViewModel> Items { get; set; }
    public bool IsCastle { get; set; }

    /// <summary>
    /// This property is used to tell the screen builder to not attempt loading a file, as this is a template that
    /// we'll be pulling from to generate our world
    /// </summary>
    public bool IsFloating { get; set; }

    public ScreenTemplates? Template { get; set; }
  }

  public class ItemViewModel {
    public List<float> Coordinates { get; set; }
    public Base.Item Item { get; set; }
  }

  public class Character {
    public List<float> Coordinates { get; set; }
    public Characters Type { get; set; }
  }

  public class Enemy {
    public List<EnemyChild> Children { get; set; }
    public int Count { get; set; }
    public Enemies Type { get; set; }
    public float Speed { get; set; } = 3f;
  }

  public class EnemyChild {
    public List<float> Coordinates { get; set; }
    public World.Enemy Enemy { get; set; }
  }

  public class Tile {
    public WorldColors? AccentColor { get; set; }
    public List<TileChild> Children { get; set; }
    public Tiles Type { get; set; }
  }

  public class ReplaceColors {
    public WorldColors Target { get; set; }
    public WorldColors Value { get; set; }
  }

  public class TileChild {
    /// <summary>
    /// This is a list of x, y coordinates, and if 4 values are specified, it becomes the max x, y range to keep adding this matter type
    /// </summary>
    public List<float> Coordinates { get; set; }

    public Tiles TileType { get; set; } = Tiles.None;

    public string[] ReplaceColors { get; set; }
    public Grid Transition { get; set; }
    public int Rotation { get; set; }
    public bool FlipY { get; set; }
    public bool FlipX { get; set; }
  }
}