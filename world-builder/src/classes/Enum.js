// We use the relative pathing here, so our copyEnums script can make use of this class
import { Model } from "../classes/models/Model.js";

class Enum extends Model {
  id = "";
  value = "";

  constructor(data) {
    super(data);
    this.set(data);
  }
}

export {
  Enum
};