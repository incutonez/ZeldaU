<template>
  <div class="flex h-full w-full">
    <BaseGrid
      ref="grid"
      v-model:selected-cell="selectedCell"
      :cells="store"
      :total-columns="record.totalColumns"
      :total-rows="record.totalRows"
      :show-grid-lines="showGridLines"
      :get-cell-color="getCellColor"
      @replace-cell="onReplaceCell"
    />
    <div class="p-4">
      <div class="flex">
        <BaseCheckbox
          v-model="showGridLines"
          label="Grid Lines"
        />
        <BaseButton
          text="Save"
          class="self-end"
          @click="onClickSaveBtn"
        />
      </div>
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
        :key="selectedCell.id"
        class="mt-8"
      >
        <div class="flex space-x-2">
          <BaseComboBox
            v-model="selectedCell.Tile"
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
        </div>
        <BaseComboBox
          v-for="targetColor in selectedCell.TargetColors"
          :key="targetColor.id"
          v-model="targetColor.Value"
          :label="`Replace ${WorldColors.getKey(targetColor.Target)}`"
          :store="accentColorsStore"
        />
        <div v-if="selectedCell.isTransition()">
          <div class="flex space-x-2">
            <BaseField
              v-model="selectedCell.Transition.X"
              label="X Change"
              width="w-24"
            />
            <BaseField
              v-model="selectedCell.Transition.Y"
              label="Y Change"
              width="w-24"
            />
          </div>
          <BaseComboBox
            v-model="selectedCell.Transition.TileType"
            label="Tile"
            :store="tilesStore"
          />
          <BaseComboBox
            v-model="selectedCell.Transition.Template"
            label="Template"
            :store="screenTemplatesStore"
          />
          <BaseCheckbox
            v-model="selectedCell.Transition.IsFloating"
            label="Floating"
          />
        </div>
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
  provide,
  reactive,
  ref,
  toRefs,
} from "vue";
import { Grid } from "@/classes/models/Grid.js";
import BaseCheckbox from "@/components/BaseCheckbox.vue";
import BaseGrid from "@/components/BaseGrid.vue";
import BaseButton from "@/components/BaseButton.vue";
import { isArray } from "@/utilities.js";
import { useKeyboardMouseProvider } from "@/composables/useKeyboardMouseProvider.js";
import BaseField from "@/components/BaseField.vue";
import { ScreenTemplates } from "@/classes/enums/ScreenTemplates.js";

console.log(WorldColors);

/**
 * TODOJEF:
 * - Add special properties for Transitions
 * - Should rename Door to ShopDoor, as it's a little special
 * - Optimize the export... right now, it does all individual cells... should be able to group by type
 * - Actually save to file system
 * - Load into Unity game to see it working
 * - Finish other TODOJEFs
 * - Add special properties to Tiles... will need to wire this up in Unity code
 * -- Like CanBreak, CanBurn, CanBomb
 */
export default {
  name: "App",
  components: {
    BaseField,
    BaseButton,
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
      groundColorsStore: WorldColors,
      accentColorsStore: WorldColors,
      screenTemplatesStore: ScreenTemplates,
      tilesStore: Tiles,
      showGridLines: true,
      record: Grid.initialize(11, 16),
    });
    const selectedGround = computed(() => state.groundColorsStore.findRecord(state.record.GroundColor)?.backgroundStyle);
    provide("pressedKeys", useKeyboardMouseProvider());

    function getCellColor() {
      return state.accentColorsStore.findRecord(state.record.GroundColor)?.backgroundStyle;
    }

    // We have to have this because we do cell replacements, which requires us doing some deep copying here
    // TODOJEF: Is there a better way of doing this?
    // TODOJEF: Make this an actual store?
    const store = computed(() => {
      return [...state.record.cells];
    }, {
      immediate: true
    });

    function onReplaceCell({ indices, replacement }) {
      if (!isArray(indices)) {
        indices = [indices];
        // Make sure we update the selection with the replacement
        selectedCell.value = replacement;
      }
      indices.forEach((idx) => {
        let record = state.record.cells[idx];
        record = state.record.cells[idx] = replacement.clone({
          Coordinates: record.Coordinates,
        });
        record.grid = replacement.grid;
      });
    }

    function onClickSaveBtn() {
      console.log(state.record.getConfig());
    }

    return {
      ...toRefs(state),
      selectedGround,
      contextMenu,
      theDialog,
      grid,
      store,
      selectedCell,
      getCellColor,
      WorldColors,
      onReplaceCell,
      onClickSaveBtn
    };
  },
};
</script>

<style>
html, body, #app {
  @apply h-full w-full;
}
</style>
