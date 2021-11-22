import { isObject } from "@/utilities.js";

class Store extends Array {
  idKey = "id";
  valueKey = "value";
  model = null;

  constructor(args, model = Object) {
    super(args);
    this.model = model;
    this.initialize(args);
  }

  initialize(items) {
    const first = this.first;
    if (isObject(items, false) || !isObject(first, false)) {
      const result = [];
      for (const item in items) {
        result.push(new this.model({
          [this.idKey]: items[item],
          [this.valueKey]: item
        }));
      }
      Object.assign(this, result);
    }
  }

  findRecord(value) {
    let found;
    for (let i = 0; i < this.length; i++) {
      const record = this[i];
      if (record[this.valueKey] === value || record[this.idKey] === value) {
        found = record;
        break;
      }
    }
    return found;
  }

  get first() {
    return this[0];
  }

  get last() {
    return [this.length - 1];
  }
}

export {
  Store
};