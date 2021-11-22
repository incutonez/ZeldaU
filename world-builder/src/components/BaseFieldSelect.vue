<template>
  <div>
    <div
      class="flex"
      :class="labelAlign"
    >
      <BaseFieldLabel
        :value="label"
        :class="labelCls"
      />
      <div
        ref="field"
        class="relative"
        @click="onClickInput"
      >
        <input
          ref="inputEl"
          :value="value"
          class="base-field"
          :type="inputType"
        >
        <div
          ref="triggerIcon"
          class="absolute h-full flex items-center top-0 right-0.5"
        >
          <ChevronDownIcon
            class="h-5 w-5 hover:text-blue-500 cursor-pointer"
          />
        </div>
      </div>
    </div>
  </div>
  <ul
    ref="list"
    class="base-list hidden"
  >
    <li
      v-for="record in store"
      :key="record[store.idKey]"
      class="base-list-item"
      @click="onClickListItem(record)"
    >
      {{ record[store.valueKey] }}
    </li>
  </ul>
</template>

<script>
import {
  computed,
  onMounted,
  onUnmounted,
  ref,
  watch
} from "vue";
import { ChevronDownIcon } from "@heroicons/vue/solid";
import { Store } from "@/classes/Store.js";
import { isEmpty } from "@/utilities.js";
import BaseFieldLabel from "@/components/BaseFieldLabel.vue";
import {
  baseFieldProps,
  useLabelCls
} from "@/components/useBaseField.js";

export default {
  name: "BaseFieldSelect",
  components: {
    ChevronDownIcon,
    BaseFieldLabel,
  },
  inheritAttrs: false,
  props: {
    ...baseFieldProps,
    store: {
      type: Store,
      required: true,
    }
  },
  // TODOJEF: Can't put this in useBaseField because IDE warns about not existing... fix?
  emits: ["update:modelValue"],
  setup(props, { emit }) {
    const list = ref(null);
    const field = ref(null);
    const triggerIcon = ref(null);
    const isExpanded = ref(false);
    const selectedRecord = ref(null);
    const value = computed(() => selectedRecord.value?.[props.store.valueKey]);

    if (!isEmpty(props.modelValue)) {
      selectedRecord.value = props.store.findRecord(props.modelValue);
    }

    watch(isExpanded, (value) => {
      if (value) {
        const position = field.value.getBoundingClientRect();
        list.value.style.width = `${position.width}px`;
        list.value.style.left = `${position.x}px`;
        list.value.style.top = `${position.bottom}px`;
        list.value.classList.remove("hidden");
        field.value.focus();
      }
      else {
        list.value.classList.add("hidden");
      }
    });

    function onChangeInput(value) {
      console.log("here", value, props.store.findRecord(value));
    }

    function toggleTrigger() {
      isExpanded.value = !isExpanded.value;
    }

    function showTrigger() {
      isExpanded.value = true;
    }

    function hideTrigger() {
      isExpanded.value = false;
    }

    function onClickInput(event) {
      if (triggerIcon.value.contains(event.target)) {
        toggleTrigger();
      }
      else {
        showTrigger();
      }
    }

    function select(record) {
      selectedRecord.value = record;
    }

    function onClickListItem(record) {
      select(record);
      emit("update:modelValue", record[props.store.idKey]);
      hideTrigger();
    }

    function onClickDocument(event) {
      if (isExpanded.value && !(field.value.contains(event.target) || triggerIcon.value.contains(event.target))) {
        hideTrigger();
      }
    }

    onMounted(() => {
      /* Let's listen for any document clicks, as we'll need to collapse if user clicks on something
     * outside of this class */
      document.addEventListener("click", onClickDocument);
    });

    onUnmounted(() => {
      document.removeEventListener("click", onClickDocument);
    });

    return {
      list,
      field,
      triggerIcon,
      value,
      labelCls: useLabelCls(props),
      onClickInput,
      onClickListItem,
      onChangeInput,
    };
  },
};
</script>

<style>
.base-list {
  @apply absolute text-sm w-full border border-t-0 shadow-sm;
}

.base-field {
  @apply pr-5;
}
</style>