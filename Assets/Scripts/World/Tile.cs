using System.Linq;
using UnityEngine;
using ViewModel;

namespace World {
  public class Tile : MonoBehaviour {
    public Tiles TileType { get; set; }
    public SpriteRenderer Renderer { get; set; }
    public PolygonCollider2D Collider { get; set; }
    public RectTransform Transform { get; set; }

    public static Tile Spawn(Vector3 position, Tiles tile, Transform parent, TileChild viewModel) {
      RectTransform transform = Instantiate(Manager.Game.Graphics.WorldTile);
      transform.SetParent(parent);
      // TODOJEF: Pick up here... the positioning is wrong
      transform.localPosition = position;
      transform.rotation = Quaternion.identity;

      Tile worldTile = transform.GetComponent<Tile>();
      worldTile.SetTile(tile, viewModel);

      return worldTile;
    }

    private void Awake() {
      Renderer = GetComponent<SpriteRenderer>();
      Collider = GetComponent<PolygonCollider2D>();
      Transform = GetComponent<RectTransform>();
    }

    public void SetTile(Tiles tile, TileChild viewModel) {
      TileType = tile;
      Sprite sprite = Manager.Game.Graphics.GetTile(tile.ToString());
      if (sprite != null) {
        Renderer.sprite = Utilities.CloneSprite(sprite, viewModel.ReplaceColors.Select(color => Utilities.HexToColor(color)).ToArray());
        if (sprite != null) {
          Transform.name = sprite.name;
        }
      }
    }
  }
}