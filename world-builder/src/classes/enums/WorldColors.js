import { EnumStore } from "@/classes/EnumStore.js";
import { Color } from "@/classes/models/Color.js";

/**
 * @property Black
 * @property Blue
 * @property Brown
 * @property FireInner
 * @property FireOuter
 * @property Gray
 * @property Green
 * @property None
 * @property PureBlue
 * @property PureGreen
 * @property PureRed
 * @property PureWhite
 * @property Q1C1Accent
 * @property Q1C1Body
 * @property Q1C1Door
 * @property Red
 * @property Tan
 * @property White
 */
export const WorldColors = new EnumStore({
  None: "Transparent",
  Tan: "FFEFA6",
  Green: "00a800",
  Brown: "c84c0c",
  Blue: "2038ec",
  Gray: "747474",
  White: "fcfcfc",
  Red: "7c0800",
  Black: "000000",
  Q1C1Accent: "008088",
  Q1C1Door: "183c5c",
  Q1C1Body: "00e8d8",
  PureBlue: "0000FF",
  PureRed: "FF0000",
  PureGreen: "00FF00",
  FireOuter: "F83800",
  FireInner: "FFA044",
  PureWhite: "FFFFFF"
}, Color);