import fs from "fs";
import path from "path";
import glob from "glob";

/**
 * This takes a C# enum and turns it into a JSON string
 * @param {String} data
 * @returns {String}
 */
function toEnum(data) {
  let output = "";
  const matches = data.trim().match(/public enum [^}]+}/g);
  const isDefault = matches.length === 1;
  matches.forEach((match) => {
    const matchName = match.match(/public enum ([^\r]+)/)[1];
    // Need the trim because there's some sort of zwnbsp character that'll show up
    match = match.trim().replace(/public enum [^{]+/, "")
    // Remove any special descriptions
    .replace(/\[[^\]]+\]\r\n/g, "")
    // Change = to :
    .replace(/=/g, ":")
    // Remove any comments
    .replace(/\/\/\/?[^\n]+\n/g, "");
    // If there are no default values, let's create them
    if (match.indexOf(":") === -1) {
      match = "['" + match.replace(/\s+/g, "").replace(/\{|\}/g, "").split(/([^,]+),/).filter((item) => item).join("','") + "']";
      console.log(match);
    }
    output += `export ${isDefault ? "default" : `const ${matchName} =`} new Enum(${match})`;
  });
  return `import {Enum} from "../Enum.js"\n${output}`;
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
