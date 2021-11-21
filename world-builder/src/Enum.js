class Enum {
  constructor(items) {
    if (Array.isArray(items)) {
      items = this.createKeys(items);
    }
    Object.assign(this, items);
  }

  createKeys(items) {
    const self = {};
    items.forEach((item, idx) => {
      self[item] = idx;
    });
    return self;
  }
}

export {
  Enum
};