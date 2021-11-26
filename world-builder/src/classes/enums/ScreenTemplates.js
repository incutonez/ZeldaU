import { EnumStore } from "@/classes/EnumStore.js";

/**
 * @property Base
 * @property Black
 * @property Blocks1
 * @property Blocks2
 * @property Blocks2Vertical
 * @property Blocks2X3Column
 * @property Blocks2X3Columns
 * @property Blocks3Horizontal
 * @property Blocks3Rows
 * @property Blocks4
 * @property Blocks4Alt
 * @property Blocks4Statues
 * @property Blocks4Way
 * @property Blocks6Alt
 * @property Blocks6Vertical
 * @property BlocksAquamentus
 * @property BlocksArrow
 * @property BlocksC
 * @property BlocksDiagonal
 * @property BlocksGleeok
 * @property BlocksHorizontal
 * @property BlocksMaze
 * @property BlocksRectangle
 * @property BlocksSpiral
 * @property BlocksStairsCenter
 * @property BlocksStairsEnd
 * @property BlocksX
 * @property Ending
 * @property Entrance
 * @property Exit
 * @property Ganon
 * @property KeepItem
 * @property KeepPath
 * @property Plain
 * @property PlainStatues
 * @property PlainStatues2
 * @property Sand
 * @property Shop
 * @property WaterBrackets
 * @property WaterH
 * @property WaterHAlt
 * @property WaterHorizontal
 * @property WaterHorizontal2
 * @property WaterLadder
 * @property WaterMaze
 * @property WaterRectangle
 * @property WaterT
 * @property WaterVertical
 */
export const ScreenTemplates = new EnumStore({
  Base: -1,
  Plain: 0,
  Entrance: 1,
  Blocks2X3Columns: 2,
  Blocks2X3Column: 3,
  Blocks4: 4,
  Blocks4Alt: 5,
  Blocks1: 6,
  BlocksX: 7,
  Blocks3Rows: 8,
  WaterMaze: 9,
  BlocksAquamentus: 10,
  Exit: 11,
  BlocksStairsCenter: 12,
  WaterBrackets: 13,
  PlainStatues: 14,
  Sand: 15,
  BlocksStairsEnd: 16,
  BlocksDiagonal: 17,
  BlocksHorizontal: 18,
  BlocksC: 19,
  Blocks2: 20,
  BlocksMaze: 21,
  WaterHorizontal: 22,
  WaterT: 23,
  Blocks4Statues: 24,
  BlocksGleeok: 25,
  WaterLadder: 26,
  Blocks6Alt: 27,
  WaterVertical: 28,
  Blocks3Horizontal: 29,
  WaterRectangle: 30,
  PlainStatues2: 31,
  BlocksRectangle: 32,
  WaterHorizontal2: 33,
  BlocksSpiral: 34,
  Ganon: 35,
  Ending: 36,
  Blocks2Vertical: 37,
  Blocks6Vertical: 38,
  Blocks4Way: 39,
  WaterH: 40,
  Black: 41,
  BlocksArrow: 42,
  WaterHAlt: 43,
  KeepItem: 44,
  KeepPath: 45,
  Shop: 46
});