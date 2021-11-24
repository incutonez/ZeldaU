import { Tiles } from "@/classes/enums/Tiles.js";
import { Model } from "@/classes/models/Model.js";
import { WorldColors } from "@/classes/enums/WorldColors.js";
import {
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

    return colors.map((color) => {
      return {
        Target: color,
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
    const targets = [];
    const replacers = [];
    targetColors.forEach((color) => {
      let target = color.Target;
      let replace = color.Replace;
      if (target === WorldColors.None) {
        target = "#00000000";
      }
      if (replace === WorldColors.None) {
        replace = "#00000000";
      }
      targets.push(encodeURIComponent(`#${target}`));
      replacers.push(encodeURIComponent(`#${replace}`));
    });
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