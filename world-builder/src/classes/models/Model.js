import { isObject } from "@/utilities.js";

class Model {
  id = "";
  value = "";

  set(field, value) {
    // Turn single instances into object syntax, so we can normalize the processing
    if (!isObject(field)) {
      field = {
        [field]: value
      };
    }
    for (const key in field) {
      Reflect.set(this, key, field[key]);
    }
  }
}

export {
  Model
};