import { Tiles } from "@/classes/enums/Tiles.js";
import { Model } from "@/classes/models/Model.js";
import { WorldColors } from "@/classes/enums/WorldColors.js";
import {
  collect,
  isEmpty,
  toQueryString
} from "@/utilities.js";
import { v4 as uuidv4 } from "uuid";

class Tile extends Model {
  /**
   * @type {Tiles}
   */
  Type = Tiles.None;
  /**
   * @type {WorldColors[]}
   */
  TargetColors = null;
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

  constructor(args) {
    super(args);
    this.set(args);
    this.id = uuidv4();
    this.updateType(this.TargetColors);
  }

  get excluded() {
    return ["grid", "tileSrc"];
  }

  get Tile() {
    return this.Type;
  }

  set Tile(value) {
    this.Type = value;
    this.updateType();
  }

  updateType(targetColors) {
    let key;
    const value = this.Type;
    if (value !== Tiles.None) {
      if (value === Tiles.Transition) {
        key = "Transparent";
      }
      else {
        key = Tiles.getKey(value);
      }
    }
    this.tileSrc = key ? `/Tiles/${key}.png` : "";
    this.setTargetColors(targetColors);
  }

  get x() {
    return this.Coordinates[0];
  }

  get y() {
    return this.Coordinates[1];
  }

  getTargetColors(includeHex) {
    return this.TargetColors.filter((color) => !!color.Value).map((color) => {
      let value = color.Value;
      let target = color.Target;
      if (target === WorldColors.None) {
        target = "00000000";
      }
      if (value === WorldColors.None) {
        value = "00000000";
      }
      if (includeHex) {
        target = `#${target}`;
        value = `#${value}`;
      }
      return {
        Target: target,
        Value: value
      };
    });
  }

  setTargetColors(targetColors) {
    if (!targetColors) {
      let colors;
      switch (this.Type) {
        case Tiles.None:
          colors = [];
          break;
        case Tiles.Block:
        case Tiles.CastleSand:
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

      targetColors = colors.map((color) => {
        return {
          Target: color,
          id: uuidv4()
        };
      });
    }
    this.TargetColors = targetColors;
  }

  get tileImage() {
    const type = this.Type;
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
    let targetColors = this.getTargetColors();
    const params = {
      tile: key,
      targetColors: collect(targetColors, "Target"),
      replaceColors: collect(targetColors, "Value")
    };
    return `http://localhost:3001/image?${toQueryString(params)}`;
  }

  getConfig() {
    return {
      Type: this.Type,
      Children: [
        {
          Coordinates: this.Coordinates,
          ReplaceColors: collect(this.getTargetColors(true), ["Target", "Value"]),
        }
      ]
    };
  }
}

export {
  Tile
};