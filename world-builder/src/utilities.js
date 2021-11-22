export function isObject(value, strict = true) {
  if (strict) {
    return value && value.constructor === Object;
  }
  return typeof value === "object";
}

export function isEmpty(value) {
  return value === undefined || value === null || value === "";
}