import fs from "fs";
import path from "path";
import glob from "glob";

/**
 * This takes a C# enum and turns it into a JSON string
 * @param {String} data
 * @returns {String}
 */
function toEnum(data) {
  // Need the trim because there's some sort of zwnbsp character that'll show up
  data = data.trim().replace(/public enum [^{]+/, "")
  // Remove any special descriptions
  .replace(/\[[^\]]+\]\r\n/g, "")
  // Change = to :
  .replace(/=/g, ":")
  // Remove any comments
  .replace(/\/\/\/?[^\n]+\n/g, "");
  // If there are no default values, let's create them
  if (data.indexOf(":") === -1) {
    let i = 0;
    const splits = data.replace(/\s+/g, "").replace(/\{|\}/g, "").split(/([^,]+),/);
    splits.forEach((item, idx) => {
      if (item) {
        // The last one doesn't have a comma, so we can't exact match it
        if (idx + 1 === splits.length) {
          data = data.replace(new RegExp(`${item}`), `${item}: ${i++},`);
        } else {
          data = data.replace(new RegExp(`${item},`), `${item}: ${i++},`);
        }
      }
    });
  }
  return `export default ${data}`;
}

if (fs.existsSync("src/enums")) {
  fs.rmSync("src/enums", { recursive: true });
}
fs.mkdirSync("src/enums");
glob("../Assets/Scripts/Enums/*.cs", {
  ignore: [ "../Assets/Scripts/Enums/EnumExtensions.cs" ]
}, (err, files) => {
  for (const file of files) {
    const data = toEnum(fs.readFileSync(file, "utf8"));
    const ext = path.extname(file);
    const baseName = path.basename(file, ext);
    fs.writeFileSync(`src/enums/${baseName}.js`, data, {
      flag: "w+"
    });
  }
});
