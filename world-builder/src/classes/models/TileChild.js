import { Tiles } from "@/classes/enums/Tiles.js";
import { Model } from "@/classes/models/Model.js";
import { WorldColors } from "@/classes/enums/WorldColors.js";
import { isEmpty } from "@/utilities.js";

class TileChild extends Model {
  /**
   * @type {Tiles}
   */
  Type = Tiles.None;
  /**
   * @type {WorldColors}
   */
  AccentColor = WorldColors.None;
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
   * @type {TileChild[]}
   */
  Children = [];

  constructor(args) {
    super(args);
    this.set(args);
  }

  get tileImage() {
    const type = this.Type;
    const color = this.AccentColor;
    if (type === Tiles.None || color === WorldColors.None || isEmpty(color) || isEmpty(type)) {
      return "";
    }
    let key;
    if (type === Tiles.Transition) {
      key = "Transparent";
    }
    else {
      key = Tiles.getKey(type);
    }
    return `http://localhost:3001/image?tile=${key}&color=${color}`;
  }
}

export {
  TileChild
};