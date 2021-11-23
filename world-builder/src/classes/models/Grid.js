import { Model } from "@/classes/models/Model.js";
import { WorldColors } from "@/classes/enums/WorldColors.js";
import { TileChild } from "@/classes/models/TileChild.js";

class Grid extends Model {
  #rows;
  #columns;

  Name = "";
  X = 0;
  Y = 0;
  /**
   * @type {WorldColors}
   */
  AccentColor = WorldColors.Brown;
  /**
   * @type {WorldColors}
   */
  GroundColor = WorldColors.Tan;
  /**
   * @type {TileChild[]}
   */
  Tiles = [];
  Enemies = [];
  Characters = [];
  Items = [];
  IsCastle = false;
  IsFloating = false;
  /**
   * @type {ScreenTemplates}
   */
  Template = null;

  constructor(args) {
    super(args);
    this.set(args);
  }

  static initialize(rows, columns) {
    const config = [];
    for (let y = 0; y < rows; y++) {
      const row = [];
      for (let x = 0; x < columns; x++) {
        row.push(new TileChild({
          Coordinates: [x, y],
        }));
      }
      config.push(row);
    }
    return new this({
      rows: config
    });
  }
}

export {
  Grid
};