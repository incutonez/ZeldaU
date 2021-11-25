import { Model } from "@/classes/models/Model.js";
import { WorldColors } from "@/classes/enums/WorldColors.js";
import { Tile } from "@/classes/models/Tile.js";
import { Tiles } from "@/classes/enums/Tiles.js";

class Grid extends Model {
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
  cells = null;
  totalRows = 0;
  totalColumns = 0;

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
          grid: self,
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

  getConfig() {
    const data = [];
    const cells = this.cells;
    for (let i = 0; i < cells.length; i++) {
      const cell = cells[i];
      if (cell.Type === Tiles.None) {
        continue;
      }
      data.push(cell.getConfig());
    }
    return data;
  }
}

export {
  Grid
};