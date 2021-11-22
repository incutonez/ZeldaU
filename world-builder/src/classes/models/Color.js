import WorldColors from "@/classes/enums/WorldColors.js";

class Color {
  id = "";
  value = "";

  constructor(args) {
    Object.assign(this, args);
  }

  get backgroundStyle() {
    if (this.id === WorldColors.TRANSPARENT) {
      return "";
    }
    return `background-color: #${this.id};`;
  }
}

export {
  Color
};