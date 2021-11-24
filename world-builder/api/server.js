import express from "express";
import cors from "cors";
import replaceColor from "replace-color";
import Jimp from "jimp";

const app = express();
app.use(cors());
app.use(express.urlencoded({ extended: true }));
app.use(express.json()); // To parse the incoming requests with JSON payloads
const port = 3001;

app.get("/image", async (req, res) => {
  try {
    let {
      targetColors,
      replaceColors
    } = req.query;
    let data = await Jimp.read(`../public/Tiles/${req.query.tile}.png`);
    if (replaceColors && targetColors) {
      if (!Array.isArray(replaceColors)) {
        replaceColors = [replaceColors];
      }
      if (!Array.isArray(targetColors)) {
        targetColors = [targetColors];
      }
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
