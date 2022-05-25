using System.Collections.Generic;
using Enums;

// ReSharper disable UnusedAutoPropertyAccessor.Global

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
    public List<TileMeta> Tiles { get; set; }
    public List<Enemy> Enemies { get; set; }
    public List<Character> Characters { get; set; }
    public List<ItemMeta> Items { get; set; }
    public bool IsCastle { get; set; }

    /// <summary>
    /// This property is used to tell the screen builder to not attempt loading a file, as this is a template that
    /// we'll be pulling from to generate our world
    /// </summary>
    public bool IsFloating { get; set; }

    public ScreenTemplates? Template { get; set; }
  }
}
