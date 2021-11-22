import { isObject } from "@/utilities.js";
import { Store } from "@/classes/Store.js";

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

  createKey(item, alter = false) {
    // We split on uppercase 
    return alter ? item.split(/([A-Z][a-z]+)/g).filter(Boolean).join("_").toUpperCase(); : item;
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

  // TODOJEF: NEED PRINT OUT METHOD
}

export {
  Enum
};