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
  console.log(req.query.tile, req.query.color);
  const data = await replaceColor({
    image: `../public/Tiles/${req.query.tile}.png`,
    colors: {
      type: "hex",
      targetColor: "#FFFFFF",
      replaceColor: `#${req.query.color}`
    }
  });
  try {
    data.getBuffer(Jimp.MIME_PNG, (err, buffer) => {
      res.send(buffer);
    });
  }
  catch (ex) {
    console.log(ex);
  }
});

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
});
