import { Store } from "../classes/Store.js";
import { Enum } from "../classes/Enum.js";

class EnumStore extends Store {
  constructor(data, model = Enum, sorters = [{ property: "value" }]) {
    super(data, model, sorters);
  }

  initialize(items) {
    super.initialize(items);
    const self = {};
    this.forEach((item) => {
      self[item[this.valueKey]] = item[this.idKey];
    });
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

  get keys() {
    return Object.keys(this);
  }

  get values() {
    return Object.values(this);
  }

  toClassDescription() {
    return "/**\n" + this.map((item) => {
      return ` * @property ${item[this.valueKey]}`;
    }).join("\n") + "\n */";
  }
}

export {
  EnumStore
};