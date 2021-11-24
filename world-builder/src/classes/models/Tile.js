import { Tiles } from "@/classes/enums/Tiles.js";
import { Model } from "@/classes/models/Model.js";
import { WorldColors } from "@/classes/enums/WorldColors.js";
import {
  collect,
  isEmpty,
  toQueryString
} from "@/utilities.js";

class Tile extends Model {
  /**
   * @type {Tiles}
   */
  Type = Tiles.None;
  /**
   * @type {WorldColors[]}
   */
  AccentColors = [WorldColors.White];
  /**
   * @type {WorldColors[]}
   */
  TargetColors = [WorldColors.White];
  /**
   * @type {Number[]}
   */
  Coordinates = [];
  /**
   * @type {Grid}
   */
  Transition = null;
  Rotation = 0;
  FlipX = false;
  FlipY = false;
  /**
   * @type {Tile[]}
   */
  Children = [];
  /**
   * @type {Grid}
   */
  Grid = null;
  tileSrc = null;

  constructor(args) {
    super(args);
    this.set(args);
    this.Tile = this.Type;
  }

  get Tile() {
    return this.Type;
  }

  set Tile(value) {
    this.Type = value;
    let key;
    if (value !== Tiles.None) {
      if (value === Tiles.Transition) {
        key = "Transparent";
      }
      else {
        key = Tiles.getKey(value);
      }
    }
    this.tileSrc = key ? `/Tiles/${key}.png` : "";
    this.TargetColors = this.getTargetColors();
  }

  get x() {
    return this.Coordinates[0];
  }

  get y() {
    return this.Coordinates[1];
  }

  getTargetColors() {
    let colors;
    switch (this.Type) {
      case Tiles.None:
        colors = [];
        break;
      case Tiles.Block:
        colors = [WorldColors.White, WorldColors.PureBlue, WorldColors.PureRed];
        break;
      case Tiles.CastleBottomLeft:
      case Tiles.CastleBottomRight:
      case Tiles.CastleTop:
      case Tiles.CastleTopAlt:
      case Tiles.CastleTopLeftAlt:
      case Tiles.CastleTopRightAlt:
      case Tiles.CastleTopLeft:
      case Tiles.CastleTopRight:
        colors = [WorldColors.White, WorldColors.PureBlue, WorldColors.Black];
        break;
      case Tiles.Bush:
      default:
        colors = [WorldColors.White, WorldColors.Black];
    }

    return colors.map((color, index) => {
      return {
        Target: color,
        Position: index,
      };
    });
  }

  get tileImage() {
    const type = this.Type;
    let targetColors = this.TargetColors.filter((color) => !!color.Replace);
    if (type === Tiles.None || isEmpty(type)) {
      return "";
    }
    let key;
    if (type === Tiles.Transition) {
      key = "Transparent";
    }
    else {
      key = Tiles.getKey(type);
    }
    targetColors = targetColors.sort((lhs, rhs) => {
      const lhsPos = lhs.Position;
      const rhsPos = rhs.Position;
      if (lhsPos === rhsPos) {
        return 0;
      }
      return lhsPos < rhsPos ? -1 : 1;
    });
    const targets = [];
    const replacers = [];
    targetColors.forEach((color) => {
      targets.push(encodeURIComponent(`#${color.Target}`));
      replacers.push(encodeURIComponent(`#${color.Replace}`));
    });
    /* We slice at the end of the maps because it's possible we're in a state where we have more targetColors
     * or replaceColors than the other, so we normalize on the lowest length */
    const params = {
      tile: key,
      targetColors: targets,
      replaceColors: replacers
    };
    return `http://localhost:3001/image?${toQueryString(params)}`;
  }
}

export {
  Tile
};