import { Model } from "@/classes/models/Model.js";
import { WorldColors } from "@/classes/enums/WorldColors.js";
import { Tile } from "@/classes/models/Tile.js";

class Grid extends Model {
  #totalRows;
  #totalColumns;
  #cells;

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
   * @type {Tile[]}
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
    const self = new this();
    for (let y = 0; y < rows; y++) {
      for (let x = 0; x < columns; x++) {
        config.push(new Tile({
          Coordinates: [x, y],
          Grid: self,
        }));
      }
    }
    self.set({
      cells: config,
      totalRows: rows,
      totalColumns: columns,
    });
    return self;
  }
}

export {
  Grid
};