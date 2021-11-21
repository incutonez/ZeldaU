module.exports = {
  purge: [ "./index.html", "./src/**/*.{vue,js,ts,jsx,tsx}" ],
  darkMode: false, // or 'media' or 'class'
  theme: {
    colors: {
      "zTan": "#FFEFA6",
      "zGreen": "#00a800",
    },
    extend: {
      gridTemplateColumns: {
        "16": "repeat(16, minmax(0, 1fr))",
      },
      gridTemplateRows: {
        "10": "repeat(10, minmax(0, 1fr))",
        "11": "repeat(11, minmax(0, 1fr))"
      },
      gridRowStart: {
        "11": "11",
        "10": "10",
        "9": "9",
        "8": "8"
      }
    },
  },
  variants: {
    extend: {},
  },
  plugins: [],
};
