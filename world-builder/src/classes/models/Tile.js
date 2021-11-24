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

  constructor(args) {
    super(args);
    this.set(args);
  }

  get x() {
    return this.Coordinates[0];
  }

  get y() {
    return this.Coordinates[1];
  }

  get tileSrc() {
    let key;
    const type = this.Type;
    if (type === Tiles.None) {
      return "";
    }
    if (type === Tiles.Transition) {
      key = "Transparent";
    }
    else {
      key = Tiles.getKey(type);
    }
    return `/Tiles/${key}.png`;
  }

  get TargetColors() {
    switch (this.Type) {
      case Tiles.Bush:
      default:
        return [WorldColors.White, WorldColors.Black];
    }
  }

  get tileImage() {
    const type = this.Type;
    const accentColors = this.AccentColors;
    if (type === Tiles.None || isEmpty(accentColors) || isEmpty(type)) {
      return "";
    }
    let key;
    if (type === Tiles.Transition) {
      key = "Transparent";
    }
    else {
      key = Tiles.getKey(type);
    }
    const params = {
      tile: key,
      targetColors: this.TargetColors.map((color) => {
        return encodeURIComponent(`#${color}`);
      }).slice(0, accentColors.length),
      replaceColors: accentColors.map((color) => {
        return encodeURIComponent(`#${color}`);
      }),
    };
    return `http://localhost:3001/image?${toQueryString(params)}`;
  }
}

export {
  Tile
};