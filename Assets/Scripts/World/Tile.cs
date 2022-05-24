using System;
using System.Linq;
using UnityEngine;

// TODOJEF: Potentially use Tilemap instead of the Grid class
namespace World {
  public class Tile : MonoBehaviour {
    public Tiles TileType { get; set; }
    public SpriteRenderer Renderer { get; set; }
    public RectTransform Transform { get; set; }

    public static Tile Spawn(Vector3 position, Tiles tileType, Transform parent, ViewModel.Tile tile, GridCell gridCell, WorldColors? worldAccentColor) {
      RectTransform transform = Instantiate(Manager.Game.Graphics.WorldTile);
      // We call SetParent after because we want the Awake method to be called in here
      transform.SetParent(parent);

      gridCell.Initialize(tileType, position);
      Tile worldTile = transform.GetComponent<Tile>();
      worldTile.SetTile(tileType, tile, worldAccentColor);
      // TODOJEF: Pick up here... I think I have to somehow use the GridCell.Initialize logic
      // started copying it over into this class, but I'm not sure if that's right?  I at least fixed
      // the positioning... I don't think the AI can walk these cells because we're not initializing that
      // logic in here
      transform.localPosition = gridCell.CenterPosition;
      transform.rotation = Quaternion.identity;

      return worldTile;
    }

    private void Awake() {
      Renderer = GetComponent<SpriteRenderer>();
      Transform = GetComponent<RectTransform>();
    }

    public void SetTile(Tiles tileType, ViewModel.Tile tile, WorldColors? worldAccentColor) {
      TileType = tileType;
      Sprite sprite = Manager.Game.Graphics.GetTile(tileType);
      if (sprite != null) {
        Color[] replaceColors = Array.Empty<Color>();
        if (tile.Colors == null) {
          if (worldAccentColor.HasValue) {
            replaceColors = new[] {WorldColors.PureWhite.GetColor(), worldAccentColor.GetColor()};
          }
        }
        else if (tile.Colors.Any()) {
          replaceColors = tile.Colors.Select(color => color.GetColor()).ToArray();
        }

        Renderer.sprite = Utilities.CloneSprite(sprite, replaceColors, true);

        if (sprite != null) {
          Transform.name = sprite.name;
        }
      }
    }
  }
}
