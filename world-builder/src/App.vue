<template>
  <div
    ref="contextMenu"
    class="context-menu hidden"
  >
    <ul>
      <li
        class="context-menu-item"
        @click="onClickContextMenuItem"
      >
        Item 1
      </li>
    </ul>
  </div>
  <div
    class="flex h-full w-full"
    @click="hideContextMenu"
  >
    <div
      class="flex-1 p-4 grid grid-cols-16 grid-rows-10 auto-rows-fr"
    >
      <template
        v-for="(row, rowIdx) in record.rows"
        :key="rowIdx"
      >
        <div
          v-for="(cell, cellIdx) in row"
          :key="`${rowIdx}_${cellIdx}`"
          :class="`grid-cell item-highlight grid-row-${rowIdx + 1} bg-${cell?.groundColor || record.groundColor}`"
          :data-cell-idx="cellIdx"
          :data-row-idx="rowIdx"
          @contextmenu="onContextMenuCell"
        >
          {{ cellIdx }}, {{ rowIdx }}
        </div>
      </template>
    </div>
    <div class="p-4">
      <BaseField
        v-model="record.groundColor"
        label="Ground Color"
        @input="onInputGroundColor"
      />
    </div>
  </div>
</template>

<script>
import BaseField from "@/components/BaseField.vue";

var b = `public enum WorldColors
{
    None = 0,
    [Color("FFEFA6")]
    Tan = 1,
    [Color("00a800")]
    Green = 2,
    [Color("c84c0c")]
    Brown = 3,
    [Color("2038ec")]
    Blue = 4,
    [Color("747474")]
    Gray = 5,
    [Color("fcfcfc")]
    White = 6,
    [Color("7c0800")]
    Red = 7,
    [Color("000000")]
    Black = 8,
    // Begin Quest 1 Castle colors
    [Color("008088")]
    Q1C1Accent = 9,
    [Color("183c5c")]
    Q1C1Door = 10,
    [Color("00e8d8")]
    Q1C1Body = 11
}`;
b = b.replace(/^public enum [^\n]+\n/, "").replace(/\[Color\(\"[^"]+\"\)\]\n/g, "").replace(/=/g, ":").replace(/\/\/[^\n]+\n/g, "")
.replace(/([\d\w]+) :/g, "\"$1\":").replace(/\n/g, "");
console.log(JSON.parse(b));
export default {
  name: "App",
  components: { BaseField },
  data() {
    return {
      record: {
        groundColor: "zTan",
        rows: Array.from(Array(11), () => new Array(16).fill(null)),
      }
    };
  },
  methods: {
    onInputGroundColor() {
      console.log(this.record);
    },
    getContextMenu() {
      return this.$refs.contextMenu;
    },
    hideContextMenu() {
      this.getContextMenu().classList.add("hidden");
    },
    showContextMenu(event) {
      const contextMenu = this.getContextMenu();
      contextMenu.style.left = `${event.pageX}px`;
      contextMenu.style.top = `${event.pageY}px`;
      contextMenu.classList.remove("hidden");
      event.preventDefault();
    },
    onContextMenuCell(event) {
      this.showContextMenu(event);
    },
    onClickContextMenuItem() {
      this.hideContextMenu();
    }
  }
};
</script>

<style lang="scss">
$totalRows: 11;
$totalColumns: 16;

html, body, #app {
  @apply h-full w-full;
}

.context-menu {
  @apply absolute bg-gray-100;

  .context-menu-item {
    @extend .item-highlight;
    @apply border-b px-4 py-2 cursor-pointer;
  }
}

/**
 * We want our grid to start at the bottom right, so we have to essentially reverse the row-start
 * for each row... row 1 has 10, row 2 has 9, etc.
 */
@for $i from 1 through $totalRows {
  .grid-row-#{$i} {
    @apply row-start-#{$totalRows - $i + 1};
  }
}

.grid-cell {
  @apply border-t border-l text-center;
}

// Target last column
.grid-cell:nth-child(#{$totalColumns}n+#{$totalColumns}) {
  @apply border-r;
}

// Taken from https://keithclark.co.uk/articles/targeting-first-and-last-rows-in-css-grid-layouts/
// Target last row, which is considered the first row due to the grid's starting position... 0,0 is bottom left
.grid-cell:nth-child(-n+#{$totalColumns}) {
  @apply border-b;
}
</style>
