export function isArray(value) {
  return Array.isArray(value);
}

export function isObject(value, strict = true) {
  if (strict) {
    return value && value.constructor === Object;
  }
  return typeof value === "object";
}

export function isEmpty(value) {
  return value === undefined || value === null || value === "" || isArray(value) && value.length === 0;
}

export function toQueryString(items) {
  const query = [];
  for (const key in items) {
    let item = items[key];
    if (!isArray(item)) {
      item = [item];
    }
    item.forEach((value) => {
      query.push(`${key}=${value}`);
    });
  }
  return query.join("&");
}

export function collect(items, keys) {
  const collection = [];
  if (isArray(keys)) {
    items.forEach((item) => {
      const collect = {};
      keys.forEach((key) => collect[key] = item[key]);
      collection.push(collect);
    });
  }
  else {
    items.forEach((item) => collection.push(item[keys]));
  }
  return collection;
}