<template>
  <div
    ref="self"
    class="base-grid"
    v-bind="$attrs"
  >
    <div
      v-for="(cell, cellIdx) in cells"
      :key="cellIdx"
      :class="getCellCls(cell, totalRows, selectedCell)"
      :style="getCellColor()"
      @click="onClickCell(cell)"
      @contextmenu="onContextMenuCell"
    >
      <img
        v-if="cell.tileImage"
        :src="cell.tileImage"
        class="w-full h-full"
      >
    </div>
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
    cells: {
      type: Array,
      default: () => []
    },
    totalRows: {
      type: Number,
      required: true,
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

    function getCellCls(cell, totalRows, selectedCell) {
      return {
        [`grid-cell row-start-${totalRows - cell.y}`]: true,
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
