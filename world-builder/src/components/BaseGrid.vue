<template>
  <div
    ref="self"
    class="base-grid"
    v-bind="$attrs"
  >
    <div
      v-for="cell in cells"
      :key="cell.id"
      :class="getCellCls(cell)"
      :style="getCellColor()"
      @click="onClickCell($event, cell)"
      @contextmenu="onContextMenuCell"
      @mouseover="onMouseOverCell(cell)"
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
  inject,
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
    totalColumns: {
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
  emits: ["update:selectedCell", "replaceCell"],
  setup(props, { emit }) {
    const self = ref(null);
    const contextMenu = ref(null);
    const testDialog = ref(null);
    const testValue = ref("Hello");
    const pressedKeys = inject("pressedKeys");
    const activeCursor = ref("cursor-pointer");
    const lastCopiedCell = ref(null);
    const hoverCell = ref(null);
    const hoverRow = ref(null);
    const hoverColumn = ref(null);

    watch(() => pressedKeys.shift, (value) => {
      if (value) {
        activeCursor.value = "cursor-cell";
      }
      else {
        activeCursor.value = "cursor-pointer";
      }
    });

    watch(() => pressedKeys.copy, (value) => {
      if (value) {
        lastCopiedCell.value = props.selectedCell;
      }
    });

    watch(() => pressedKeys.paste, (value) => {
      const selectedCell = props.selectedCell;
      const replacement = lastCopiedCell.value;
      if (value && selectedCell && replacement && selectedCell !== replacement) {
        const indices = props.cells.indexOf(selectedCell);
        emit("replaceCell", {
          indices,
          replacement: replacement
        });
      }
    });

    function getCellCls(cell) {
      let hoverCls = false;
      if (props.selectedCell && pressedKeys.shift) {
        let y = cell.y;
        let x = cell.x;
        let fromX = props.selectedCell.x;
        let fromY = props.selectedCell.y;
        let toX = hoverCell.value.x;
        let toY = hoverCell.value.y;
        if (fromX > toX) {
          fromX = toX;
          toX = props.selectedCell.x;
        }
        if (fromY > toY) {
          fromY = toY;
          toY = props.selectedCell.y;
        }
        if (x <= toX && y <= toY && x >= fromX && y >= fromY) {
          hoverCls = true;
        }
      }
      return {
        [`grid-cell row-start-${props.totalRows - cell.y}`]: true,
        "grid-cell-selected": cell === props.selectedCell,
        "grid-cell-hover": hoverCls,
        [activeCursor.value]: true,
      };
    }

    function getSelectedCells() {
      const cells = [];
      const totalColumns = props.totalColumns;
      const selectedCell = props.selectedCell;
      let fromX = selectedCell.x;
      let fromY = selectedCell.y;
      let toX = hoverCell.value.x;
      let toY = hoverCell.value.y;
      if (fromX > toX) {
        fromX = toX;
        toX = selectedCell.x;
      }
      if (fromY > toY) {
        fromY = toY;
        toY = selectedCell.y;
      }
      for (let x = fromX; x <= toX; x++) {
        for (let y = fromY; y <= toY; y++) {
          const index = x + y * totalColumns;
          if (index === selectedCell.getIndex()) {
            continue;
          }
          cells.push(index);
        }
      }
      return cells;
    }

    function onClickCell(event, cell) {
      const selectedCell = props.selectedCell;
      if (pressedKeys.shift) {
        const indices = getSelectedCells();
        emit("replaceCell", {
          indices,
          // TODOJEF: More performant to call set instead of cloning all the time?
          replacement: selectedCell
        });
        document.getSelection().removeAllRanges();
        return;
      }
      if (cell === selectedCell) {
        cell = null;
      }
      emit("update:selectedCell", cell);
    }

    function onContextMenuCell(event) {
      contextMenu.value.show(event);
    }

    function hideContextMenu() {
      contextMenu.value.hide();
    }

    function onMouseOverCell(cell) {
      hoverCell.value = cell;
      hoverRow.value = cell.y;
      hoverColumn.value = cell.x;
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
      activeCursor,
      getCellCls,
      pressedKeys,
      onClickCell,
      onContextMenuCell,
      onMouseOverCell,
      onClickTilesMenu() {
        testDialog.value.show();
        hideContextMenu();
      },
    };
  }
};
</script>
