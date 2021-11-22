import { isObject } from "@/utilities.js";

class Enum {
  constructor(items) {
    this.initialize(items);
  }

  initialize(items) {
    if (Array.isArray(items)) {
      const self = {};
      items.forEach((item, idx) => {
        if (isObject(item)) {
          Object.assign(self, item);
        }
        else {
          self[this.createKey(item)] = idx;
        }
      });
      items = self;
    }
    Object.assign(this, items);
  }

  createKey(item) {
    // We split on uppercase 
    return item.split(/([A-Z][a-z]+)/g).filter(Boolean).join("_").toUpperCase();
  }

  get keys() {
    return Object.keys(this);
  }

  get values() {
    return Object.values(this);
  }
}

export {
  Enum
};