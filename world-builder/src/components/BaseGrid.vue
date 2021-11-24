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
        const index = props.cells.indexOf(selectedCell);
        emit("replaceCell", {
          index,
          replacement: replacement.clone({
            Coordinates: selectedCell.Coordinates
          })
        });
      }
    });

    function getCellCls(cell) {
      return {
        [`grid-cell row-start-${props.totalRows - cell.y}`]: true,
        "grid-cell-selected": cell === props.selectedCell,
        [activeCursor.value]: true,
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
      activeCursor,
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
