import { Model } from "@/classes/models/Model.js";

class Tile extends Model {
  get path() {
    return "../../../public/Tiles/";
  }
}

export {
  Tile
};