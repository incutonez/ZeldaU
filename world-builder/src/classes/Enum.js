// We use the relative pathing here, so our copyEnums script can make use of this class
import { isObject } from "../utilities.js";
import { Store } from "../classes/Store.js";

class Enum {
  #model;

  constructor(items, model) {
    this.#model = model;
    this.initialize(items);
  }

  initialize(items) {
    const self = {};
    if (Array.isArray(items)) {
      items.forEach((item, idx) => {
        if (isObject(item)) {
          Object.assign(self, item);
        }
        else {
          self[this.createKey(item)] = idx;
        }
      });
    }
    else {
      for (const item in items) {
        self[this.createKey(item)] = items[item];
      }
    }
    Object.assign(this, self);
  }

  getKey(value) {
    const index = this.values.indexOf(value);
    return this.keys[index];
  }

  createKey(item, alter = false) {
    // We split on uppercase 
    return alter ? item.split(/([A-Z][a-z]+)/g).filter(Boolean).join("_").toUpperCase() : item;
  }

  get store() {
    return new Store(this, this.#model);
  }

  get keys() {
    return Object.keys(this);
  }

  get values() {
    return Object.values(this);
  }

  toClassDescription() {
    return "/**\n" + this.keys.map((key) => {
      return ` * @property ${key}`;
    }).join("\n") + "\n */";
  }
}

export {
  Enum
};