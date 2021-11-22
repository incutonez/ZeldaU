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
          :class="`grid-cell item-highlight grid-start-${record.rows - rowIdx + 1} bg-${cell?.groundColor || record.groundColor}`"
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

console.log("App.vue");
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

<style>
html, body, #app {
  @apply h-full w-full;
}

.context-menu {
  @apply absolute bg-gray-100;
}

.context-menu-item {
  @apply border-b px-4 py-2 cursor-pointer item-highlight;
}

.grid-cell {
  @apply border-t border-l text-center;
}

/* Target last column */
.grid-cell:nth-child(#{$totalColumns}n+#{$totalColumns}) {
  @apply border-r;
}

/* Taken from https://keithclark.co.uk/articles/targeting-first-and-last-rows-in-css-grid-layouts/ */
/* Target last row, which is considered the first row due to the grid's starting position... 0,0 is bottom left */
.grid-cell:nth-child(-n+#{$totalColumns}) {
  @apply border-b;
}
</style>
