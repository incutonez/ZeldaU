<template>
  <div class="flex h-full w-full">
    <BaseGrid
      ref="grid"
      v-model:selected-cell="selectedCell"
      :cells="record.cells"
      :total-rows="record.totalRows"
      :show-grid-lines="showGridLines"
      :get-cell-color="getCellColor"
    />
    <div class="p-4">
      <BaseCheckbox
        v-model="showGridLines"
        label="Grid Lines"
      />
      <BaseComboBox
        v-model="record.GroundColor"
        label="Ground Color"
        :store="groundColorsStore"
      />
      <BaseComboBox
        v-model="record.AccentColor"
        label="World Accent Color"
        :store="accentColorsStore"
      />
      <div
        v-if="selectedCell"
        :key="selectedCell"
        class="mt-8"
      >
        <BaseComboBox
          v-model="selectedCell.Type"
          label="Cell Tile"
          :store="tilesStore"
        />
        <div class="w-16 h-16 bg-blue-100">
          <img
            v-if="selectedCell.tileSrc"
            :src="selectedCell.tileSrc"
            class="w-full h-full"
          >
        </div>
        <BaseComboBox
          v-for="(targetColor, idx) in selectedCell.TargetColors"
          :key="idx"
          v-model="selectedCell.AccentColors[idx]"
          :label="`Target ${WorldColors.getKey(targetColor)}`"
          :store="accentColorsStore"
        />
      </div>
    </div>
  </div>
</template>

<script>
import BaseComboBox from "@/components/BaseComboBox.vue";
import { WorldColors } from "@/classes/enums/WorldColors.js";
import { Tiles } from "@/classes/enums/Tiles.js";
import {
  computed,
  onUnmounted,
  provide,
  reactive,
  ref,
  toRefs,
} from "vue";
import { Grid } from "@/classes/models/Grid.js";
import BaseCheckbox from "@/components/BaseCheckbox.vue";
import BaseGrid from "@/components/BaseGrid.vue";

/**
 * TODOJEF:
 * - Get cell highlighting working with shift
 */
export default {
  name: "App",
  components: {
    BaseGrid,
    BaseCheckbox,
    BaseComboBox
  },
  setup() {
    const contextMenu = ref(null);
    const theDialog = ref(null);
    const selectedCell = ref(null);
    const grid = ref(null);
    const isShiftHeld = ref(false);
    const state = reactive({
      groundColorsStore: WorldColors.store,
      accentColorsStore: WorldColors.store,
      tilesStore: Tiles.store,
      showGridLines: true,
      record: Grid.initialize(11, 16),
    });
    const selectedGround = computed(() => state.groundColorsStore.findRecord(state.record.GroundColor)?.backgroundStyle);
    provide("isShiftHeld", isShiftHeld);

    function getCellColor() {
      return state.accentColorsStore.findRecord(state.record.GroundColor)?.backgroundStyle;
    }

    function onDocumentKeyDown(event) {
      isShiftHeld.value = event.shiftKey;
    }

    function onDocumentKeyUp(event) {
      isShiftHeld.value = event.shiftKey;
    }

    document.addEventListener("keydown", onDocumentKeyDown);
    document.addEventListener("keyup", onDocumentKeyUp);

    onUnmounted(() => {
      document.removeEventListener("keydown", onDocumentKeyDown);
      document.removeEventListener("keyup", onDocumentKeyUp);
    });

    return {
      ...toRefs(state),
      selectedGround,
      contextMenu,
      theDialog,
      grid,
      selectedCell,
      getCellColor,
      WorldColors
    };
  },
};
</script>

<style>
html, body, #app {
  @apply h-full w-full;
}
</style>
