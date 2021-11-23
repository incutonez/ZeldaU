<template>
  <BaseContextMenu ref="contextMenu">
    <template #default>
      <ul>
        <li
          class="context-menu-item"
          @click="onClickTilesMenu"
        >
          Tiles
        </li>
      </ul>
    </template>
  </BaseContextMenu>
  <BaseDialog
    ref="theDialog"
    title="Hello World"
  >
    <template #default>
      <BaseFieldSelect
        label="Tiles"
        :store="tilesStore"
      />
    </template>
  </BaseDialog>
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
          :class="getCellCls(cell, record.rows.length, rowIdx, selectedCell)"
          :style="getCellColor(cell.AccentColor)"
          :data-cell-idx="cellIdx"
          :data-row-idx="rowIdx"
          @click="onClickCell(cell)"
          @contextmenu="onContextMenuCell"
        >
          <img
            v-if="cell.tileImage"
            :src="cell.tileImage"
            class="w-full h-full"
          >
        </div>
      </template>
    </div>
    <div class="p-4">
      <BaseFieldSelect
        v-model="record.GroundColor"
        label="Ground Color"
        :store="groundColorsStore"
      />
      <BaseFieldSelect
        v-model="record.AccentColor"
        label="World Accent Color"
        :store="accentColorsStore"
      />
      <div
        v-if="selectedCell"
        :key="selectedCell"
        class="mt-8"
      >
        <BaseFieldSelect
          v-model="selectedCell.AccentColor"
          label="Cell Accent Color"
          :store="accentColorsStore"
        />
        <BaseFieldSelect
          v-model="selectedCell.Type"
          label="Cell Tile"
          :store="tilesStore"
        />
      </div>
    </div>
  </div>
</template>

<script>
import BaseFieldSelect from "@/components/BaseFieldSelect.vue";
import { WorldColors } from "@/classes/enums/WorldColors.js";
import { Tiles } from "@/classes/enums/Tiles.js";
import BaseDialog from "@/components/BaseDialog.vue";
import {
  computed,
  reactive,
  ref,
  toRefs
} from "vue";
import BaseContextMenu from "@/components/BaseContextMenu.vue";
import { Grid } from "@/classes/models/Grid.js";

export default {
  name: "App",
  components: {
    BaseContextMenu,
    BaseDialog,
    BaseFieldSelect
  },
  setup() {
    const contextMenu = ref(null);
    const theDialog = ref(null);
    const selectedCell = ref(null);
    const state = reactive({
      groundColorsStore: WorldColors.store,
      accentColorsStore: WorldColors.store,
      tilesStore: Tiles.store,
      record: Grid.initialize(11, 16),
    });
    const selectedGround = computed(() => state.groundColorsStore.findRecord(state.record.GroundColor)?.backgroundStyle);

    function getCellColor(accentColor) {
      if (accentColor === WorldColors.None) {
        accentColor = "";
      }
      return state.accentColorsStore.findRecord(state.record.GroundColor)?.backgroundStyle;
    }

    function getCellCls(cell, totalRows, rowIdx) {
      return {
        [`grid-cell row-start-${totalRows - rowIdx}`]: true,
        "grid-cell-selected": cell === selectedCell.value
      };
    }

    function hideContextMenu() {
      contextMenu.value.hide();
    }

    function onClickCell(cell) {
      selectedCell.value = cell;
    }

    return {
      ...toRefs(state),
      selectedGround,
      contextMenu,
      theDialog,
      hideContextMenu,
      onClickCell,
      selectedCell,
      getCellCls,
      getCellColor,
      onContextMenuCell(event) {
        contextMenu.value.show(event);
      },
      onClickTilesMenu() {
        theDialog.value.show();
        hideContextMenu();
      }
    };
  },
};
</script>

<style>
html, body, #app {
  @apply h-full w-full;
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
