<template>
  <div
    ref="self"
    class="base-grid"
    v-bind="$attrs"
  >
    <template
      v-for="(row, rowIdx) in rows"
      :key="rowIdx"
    >
      <div
        v-for="(cell, cellIdx) in row"
        :key="`${rowIdx}_${cellIdx}`"
        :class="getCellCls(cell, rows.length, rowIdx, selectedCell)"
        :style="getCellColor()"
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
    ref="testDialog"
    title="Hello World"
  >
    <template #default>
      <BaseField
        v-model="testValue"
        label="Hello"
      />
    </template>
  </BaseDialog>
</template>

<script>
import {
  onMounted,
  ref,
  watch
} from "vue";
import BaseContextMenu from "@/components/BaseContextMenu.vue";
import BaseDialog from "@/components/BaseDialog.vue";
import BaseField from "@/components/BaseField.vue";

export default {
  name: "BaseGrid",
  components: {
    BaseField,
    BaseContextMenu,
    BaseDialog
  },
  inheritAttrs: false,
  props: {
    rows: {
      type: Array,
      default: () => []
    },
    selectedCell: {
      type: Object,
      default: () => null
    },
    getCellColor: {
      type: Function,
      default: () => null
    },
    showGridLines: {
      type: Boolean,
      default: true,
    }
  },
  emits: ["update:selectedCell"],
  setup(props, { emit }) {
    const self = ref(null);
    const contextMenu = ref(null);
    const testDialog = ref(null);
    const testValue = ref("Hello");

    function getCellCls(cell, totalRows, rowIdx, selectedCell) {
      return {
        [`grid-cell row-start-${totalRows - rowIdx}`]: true,
        "grid-cell-selected": cell === selectedCell
      };
    }

    function onClickCell(cell) {
      emit("update:selectedCell", cell);
    }

    function onContextMenuCell(event) {
      contextMenu.value.show(event);
    }

    function hideContextMenu() {
      contextMenu.value.hide();
    }

    onMounted(() => {
      toggleGridLines(props.showGridLines);
    });

    watch(() => props.showGridLines, (value) => {
      toggleGridLines(value);
    });

    function toggleGridLines(value) {
      if (value) {
        self.value.classList.add("grid-show-lines");
      }
      else {
        self.value.classList.remove("grid-show-lines");
      }
    }

    return {
      contextMenu,
      self,
      testDialog,
      testValue,
      getCellCls,
      onClickCell,
      onContextMenuCell,
      onClickTilesMenu() {
        testDialog.value.show();
        hideContextMenu();
      },
    };
  }
};
</script>
