using System;
using System.Linq;
using Enums;
using UnityEngine;

// TODO: Potentially use Tilemap instead of the Grid class
namespace World {
  public class Tile : MonoBehaviour {
    private Tiles TileType { get; set; }
    private SpriteRenderer Renderer { get; set; }
    private RectTransform Transform { get; set; }

    public static Tile Spawn(Tiles tileType, Transform parent, ViewModel.Tile tile, GridCell gridCell, WorldColors? worldAccentColor) {
      gridCell.SetTileType(tileType);
      var transform = Instantiate(Manager.Game.Graphics.WorldTile, gridCell.CenterPosition, Quaternion.identity);
      // We call SetParent after because we want the Awake method to be called in here
      transform.SetParent(parent);
      var worldTile = transform.GetComponent<Tile>();
      worldTile.SetTile(tileType, tile, worldAccentColor);
      return worldTile;
    }

    private void Awake() {
      Renderer = GetComponent<SpriteRenderer>();
      Transform = GetComponent<RectTransform>();
    }

    private void SetTile(Tiles tileType, ViewModel.Tile tile, WorldColors? worldAccentColor) {
      TileType = tileType;
      var sprite = Manager.Game.Graphics.GetTile(tileType);
      if (sprite != null) {
        var replaceColors = Array.Empty<Color>();
        if (tile.Colors == null) {
          if (worldAccentColor.HasValue) {
            replaceColors = new[] {WorldColors.PureWhite.GetColor(), worldAccentColor.GetColor()};
          }
        }
        else if (tile.Colors.Any()) {
          replaceColors = tile.Colors.Select(color => color.GetColor()).ToArray();
        }

        Renderer.sprite = Utilities.CloneSprite(sprite, replaceColors, true);
        Transform.name = sprite.name;
      }
    }
  }
}
