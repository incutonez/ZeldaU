import { Tiles } from "@/classes/enums/Tiles.js";
import { Model } from "@/classes/models/Model.js";
import { WorldColors } from "@/classes/enums/WorldColors.js";

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
}

export {
  TileChild
};