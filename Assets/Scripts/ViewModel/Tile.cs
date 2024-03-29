﻿using System.Collections.Generic;
using Enums;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ViewModel {
  public class TileMeta {
    public List<Tile> Children { get; set; }
    public Tiles Type { get; set; }
  }

  public class Tile {
    public float X { get; set; }
    public float Y { get; set; }

    /// <summary>
    /// This comes in as pairs... the first color is the target, and the second is the replacement.
    /// If a 3rd color is specified, then that's the 2nd target color, and so on.
    /// </summary>
    public WorldColors[] Colors { get; set; }

    public Grid Transition { get; set; }
  }
}
