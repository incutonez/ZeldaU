<template>
  <div
    ref="contextMenu"
    class="context-menu hidden"
  >
    <ul>
      <li
        class="context-menu-item"
        @click="onClickTilesMenu"
      >
        Tiles
      </li>
    </ul>
  </div>
  <div
    ref="tilesDialog"
    class="base-dialog hidden"
  >
    <BaseFieldSelect
      label="Tiles"
      :store="tilesStore"
    />
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
          :class="`grid-cell row-start-${record.rows.length - rowIdx}`"
          :style="selectedGround"
          :data-cell-idx="cellIdx"
          :data-row-idx="rowIdx"
          @contextmenu="onContextMenuCell"
        >
          {{ cellIdx }}, {{ rowIdx }}
        </div>
      </template>
    </div>
    <div class="p-4 space-y-2">
      <BaseFieldSelect
        v-model="record.groundColor"
        label="Ground Color"
        :store="groundColorsStore"
      />
      <BaseFieldSelect
        v-model="record.accentColor"
        label="Accent Color"
        :store="accentColorsStore"
      />
    </div>
  </div>
</template>

<script>
import BaseFieldSelect from "@/components/BaseFieldSelect.vue";
import WorldColors from "@/classes/enums/WorldColors.js";
import Tiles from "@/classes/enums/Tiles.js";

export default {
  name: "App",
  components: { BaseFieldSelect },
  data() {
    return {
      groundColorsStore: WorldColors.store,
      accentColorsStore: WorldColors.store,
      tilesStore: Tiles.store,
      record: {
        groundColor: WorldColors.Tan,
        accentColor: WorldColors.Green,
        rows: Array.from(Array(11), () => new Array(16).fill(null)),
      }
    };
  },
  computed: {
    selectedGround() {
      return this.groundColorsStore.findRecord(this.record.groundColor)?.backgroundStyle;
    }
  },
  methods: {
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
    onClickTilesMenu() {
      this.$refs.tilesDialog.classList.remove("hidden");
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

/* Target last column */
.grid-cell:nth-child(16n+16) {
  @apply border-r;
}

/* Taken from https://keithclark.co.uk/articles/targeting-first-and-last-rows-in-css-grid-layouts/ */
/* Target last row, which is considered the first row due to the grid's starting position... 0,0 is bottom left */
.grid-cell:nth-child(-n+16) {
  @apply border-b;
}
</style>
