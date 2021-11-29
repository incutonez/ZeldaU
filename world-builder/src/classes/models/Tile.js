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
  /**
   * @type {Grid}
   */
  grid = null;

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

  isTransition() {
    return this.Type === Tiles.Transition;
  }

  getTileKey() {
    let key;
    const value = this.Type;
    if (value !== Tiles.None) {
      if (this.isTransition()) {
        key = "Transparent";
      }
      else {
        key = Tiles.getKey(value);
      }
    }
    return key;
  }

  updateType(targetColors) {
    const key = this.getTileKey();
    if (this.isTransition()) {
      this.Transition = this.Transition || {
        X: 0,
        Y: 0,
        TileType: Tiles.Transition,
        Name: null,
        Template: null,
        IsFloating: false,
      };
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

  getTargetColors(useKey) {
    const targetColors = this.TargetColors.filter((color) => !!color.Value);
    if (useKey) {
      return targetColors.reduce((value, item) => {
        value.push(WorldColors.getKey(item.Target), WorldColors.getKey(item.Value));
        return value;
      }, []);
    }
    else {
      return targetColors.map((color) => {
        let value = color.Value;
        let target = color.Target;
        if (target === WorldColors.None) {
          target = "00000000";
        }
        if (value === WorldColors.None) {
          value = "00000000";
        }
        return {
          Target: target,
          Value: value
        };
      });
    }
  }

  getIndex() {
    return this.x + this.y * this.grid.totalColumns;
  }

  setTargetColors(targetColors) {
    if (!targetColors) {
      let colors;
      /**
       * TODOJEF: I really should standardize these colors
       * - 1 color => White
       * - 2 colors => White, Black
       * - 3 colors => White, Black, Blue
       * - 4 colors => White, Black, Blue, Red
       */
      switch (this.Type) {
        // TODOJEF: Add color change for these?
        case Tiles.StairsKeep:
        case Tiles.WallKeep:
        case Tiles.Transition:
        case Tiles.None:
          colors = [];
          break;
        case Tiles.Block:
        case Tiles.CastleSand:
        case Tiles.DoorClosedY:
        case Tiles.DoorClosedX:
        case Tiles.DoorUnlockedX:
        case Tiles.DoorUnlockedY:
        case Tiles.DoorLockedX:
        case Tiles.DoorLockedY:
        case Tiles.Statue1:
        case Tiles.Statue2:
        case Tiles.WallLeftX:
        case Tiles.WallLeftY:
        case Tiles.WallLeftYFlip:
        case Tiles.WallRightX:
        case Tiles.WallRightY:
        case Tiles.WallRightYFlip:
        case Tiles.WallX:
        case Tiles.WallY:
          colors = [WorldColors.PureWhite, WorldColors.PureBlue, WorldColors.PureRed];
          break;
        case Tiles.WallHoleX:
        case Tiles.WallHoleY:
          colors = [WorldColors.PureWhite, WorldColors.Black, WorldColors.PureBlue, WorldColors.PureRed];
          break;
        case Tiles.Fire:
        case Tiles.FireAlt:
          colors = [WorldColors.FireOuter, WorldColors.FireInner, WorldColors.PureWhite];
          break;
        case Tiles.CastleBottomLeft:
        case Tiles.CastleBottomRight:
        case Tiles.CastleTop:
        case Tiles.CastleTopAlt:
        case Tiles.CastleTopLeftAlt:
        case Tiles.CastleTopRightAlt:
        case Tiles.CastleTopLeft:
        case Tiles.CastleTopRight:
        case Tiles.Dock:
        case Tiles.Grave:
        case Tiles.StairsUp:
        case Tiles.Statue:
        case Tiles.TreeBottomRight:
        case Tiles.TreeTopLeft:
        case Tiles.Water:
        case Tiles.WaterTopLeft:
        case Tiles.WaterTopRight:
        case Tiles.WaterBottomLeft:
        case Tiles.WaterBottomRight:
          colors = [WorldColors.PureWhite, WorldColors.PureBlue, WorldColors.Black];
          break;
        case Tiles.GroundTile:
          colors = [WorldColors.PureWhite, WorldColors.PureRed, WorldColors.Black];
          break;
        case Tiles.CastleWater:
          colors = [WorldColors.PureBlue];
          break;
        case Tiles.Door:
          colors = [WorldColors.Black];
          break;
        case Tiles.SandBottom:
        case Tiles.SandCenter:
          colors = [WorldColors.PureWhite];
          break;
        case Tiles.PondBottom:
        case Tiles.PondBottomLeft:
        case Tiles.PondBottomRight:
        case Tiles.PondTop:
        case Tiles.PondTopLeft:
        case Tiles.PondTopRight:
        case Tiles.PondCenter:
        case Tiles.PondCenterLeft:
        case Tiles.PondCenterRight:
          colors = [WorldColors.PureWhite, WorldColors.PureBlue];
          break;
        case Tiles.Bush:
        default:
          colors = [WorldColors.PureWhite, WorldColors.Black];
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
    const key = this.getTileKey();
    if (isEmpty(key)) {
      return "";
    }
    const targetColors = this.getTargetColors();
    const params = {
      tile: key,
      targetColors: collect(targetColors, "Target"),
      replaceColors: collect(targetColors, "Value")
    };
    return `http://localhost:3001/image?${toQueryString(params)}`;
  }

  getConfig() {
    return {
      Type: Tiles.getKey(this.Type),
      Children: [
        {
          Transition: this.Transition,
          Coordinates: this.Coordinates,
          ReplaceColors: this.getTargetColors(true),
        }
      ]
    };
  }
}

export {
  Tile
};