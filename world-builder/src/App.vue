<template>
  <div class="flex h-full w-full">
    <BaseGrid
      ref="grid"
      v-model:selected-cell="selectedCell"
      :rows="record.rows"
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
          v-model="selectedCell.AccentColor"
          label="Cell Accent Color"
          :store="accentColorsStore"
        />
        <BaseComboBox
          v-model="selectedCell.Type"
          label="Cell Tile"
          :store="tilesStore"
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
  reactive,
  ref,
  toRefs,
} from "vue";
import { Grid } from "@/classes/models/Grid.js";
import BaseCheckbox from "@/components/BaseCheckbox.vue";
import BaseGrid from "@/components/BaseGrid.vue";

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
    const state = reactive({
      groundColorsStore: WorldColors.store,
      accentColorsStore: WorldColors.store,
      tilesStore: Tiles.store,
      showGridLines: true,
      record: Grid.initialize(11, 16),
    });
    const selectedGround = computed(() => state.groundColorsStore.findRecord(state.record.GroundColor)?.backgroundStyle);

    function getCellColor() {
      return state.accentColorsStore.findRecord(state.record.GroundColor)?.backgroundStyle;
    }

    return {
      ...toRefs(state),
      selectedGround,
      contextMenu,
      theDialog,
      grid,
      selectedCell,
      getCellColor
    };
  },
};
</script>

<style>
html, body, #app {
  @apply h-full w-full;
}
</style>
