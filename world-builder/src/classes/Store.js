// We use the relative pathing here, so our copyEnums script can make use of this class
import {
  isArray,
  isObject
} from "../utilities.js";
import { Model } from "../classes/models/Model.js";

class Store extends Array {
  idKey = "id";
  valueKey = "value";
  sorters;
  model;

  constructor(data, model = Model, sorters = []) {
    super(data);
    this.model = model;
    this.sorters = sorters;
    this.initialize(data);
  }

  initialize(items) {
    if (isArray(items)) {
      if (!isObject(items[0], false)) {
        const result = [];
        for (let i = 0; i < items.length; i++) {
          result.push(new this.model({
            [this.idKey]: i,
            [this.valueKey]: items[i]
          }));
        }
        Object.assign(this, result);
      }
    }
    else if (isObject(items, false)) {
      const result = [];
      for (const item in items) {
        result.push(new this.model({
          [this.idKey]: items[item],
          [this.valueKey]: item
        }));
      }
      Object.assign(this, result);
    }
    this.sort();
  }

  sort(sorters) {
    sorters = sorters || this.sorters;
    sorters?.forEach((sorter) => {
      if (isObject(sorter)) {
        const { property, direction = 1 } = sorter;
        super.sort((lhs, rhs) => {
          const lhsProp = lhs[property];
          const rhsProp = rhs[property];
          if (lhsProp === rhsProp) {
            return 0;
          }
          return lhsProp < rhsProp ? direction * -1 : direction * 1;
        });
      }
      // We have a function
      else {
        super.sort(sorter);
      }
    });
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