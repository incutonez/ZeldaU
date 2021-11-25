import {
  clone,
  isObject
} from "@/utilities.js";

class Model {
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

  /**
   * If this is set, then we exclude any properties within the array when cloning or getting data
   * @returns {String[]}
   */
  get excluded() {
    return [];
  }

  getData({ excluded, options }) {
    const output = {};
    for (const key in this) {
      if (excluded?.indexOf(key) !== -1) {
        continue;
      }
      output[key] = options[key] || this[key];
    }
    return clone(output);
  }

  clone(options = {}) {
    return new this.constructor(this.getData({ options, excluded: this.excluded }));
  }
}

export {
  Model
};