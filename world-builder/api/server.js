import express from "express";
import cors from "cors";
import replaceColor from "replace-color";
import Jimp from "jimp";

const app = express();
app.use(cors());
app.use(express.urlencoded({ extended: true }));
app.use(express.json()); // To parse the incoming requests with JSON payloads
const port = 3001;

function determinePriority(targetColors, replaceColors) {
  /**
   * This is a little tricky... we need to see if any of our targeted colors are in the replace colors,
   * and if so, make sure our priority for replacing those colors is in a proper order...
   * e.g. if we replace black with blue, but then a later color is blue with red, then we'll be replacing
   * the first black => blue with red
   */
  for (let i = targetColors.length; i >= 0; i--) {
    const index = replaceColors.indexOf(targetColors[i]);
    if (index === -1) {
      continue;
    }
    if (index < i) {
      const tempReplace = replaceColors[index];
      const tempTarget = targetColors[index];
      replaceColors[index] = replaceColors[i];
      replaceColors[i] = tempReplace;
      targetColors[index] = targetColors[i];
      targetColors[i] = tempTarget;
    }
  }
}

app.get("/image", async (req, res) => {
  try {
    let {
      targetColors,
      replaceColors
    } = req.query;
    let data = `../public/Tiles/${req.query.tile}.png`;
    if (!Array.isArray(replaceColors)) {
      replaceColors = [replaceColors];
    }
    if (!Array.isArray(targetColors)) {
      targetColors = [targetColors];
    }
    // TODOJEF: Is there a way to tell different shades of colors?
    determinePriority(targetColors, replaceColors);
    for (let i = 0; i < replaceColors.length; i++) {
      data = await replaceColor({
        image: data,
        deltaE: 7,
        colors: {
          type: "hex",
          targetColor: targetColors[i],
          replaceColor: replaceColors[i]
        }
      });
    }
    data.getBuffer(Jimp.MIME_PNG, (err, buffer) => {
      res.send(buffer);
    });
  }
  catch (ex) {
    console.log(ex);
    res.sendStatus(500);
  }
});

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
});
