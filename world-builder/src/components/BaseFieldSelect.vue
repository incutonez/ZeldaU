<template>
  <div
    class="relative"
    :class="[width]"
  >
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
          :class="inputCls"
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
    <ul
      ref="list"
      class="base-list hidden"
    >
      <li
        v-for="record in store"
        :key="record[store.idKey]"
        class="base-list-item"
        :class="selectedRecord === record ? 'bg-blue-300' : ''"
        @click="onClickListItem(record)"
      >
        <slot
          name="contentListItem"
          :record="record"
          :store="store"
          :displayValue="record[store.valueKey]"
        >
          {{ record[store.valueKey] }}
        </slot>
      </li>
    </ul>
  </div>
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
  useInputCls,
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
  emits: ["update:modelValue", "select"],
  setup(props, { emit }) {
    const list = ref(null);
    const field = ref(null);
    const inputEl = ref(null);
    const triggerIcon = ref(null);
    const isExpanded = ref(false);
    const selectedRecord = ref(null);
    const value = computed(() => selectedRecord.value?.[props.store.valueKey]);

    if (!isEmpty(props.modelValue)) {
      selectedRecord.value = props.store.findRecord(props.modelValue);
    }

    watch(isExpanded, (value) => {
      if (value) {
        const position = inputEl.value;
        const parent = inputEl.value.offsetParent;
        list.value.style.width = `${position.offsetWidth}px`;
        list.value.style.left = `${position.offsetLeft}px`;
        // Our input lives in a relatively positioned div, so let's include the div's offsetTop
        list.value.style.top = `${position.offsetHeight + parent.offsetTop}px`;
        list.value.classList.remove("hidden");
        inputEl.value.focus();
      }
      else {
        list.value.classList.add("hidden");
      }
    });

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
      emit("select", record);
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
      inputEl,
      selectedRecord,
      inputCls: useInputCls(props),
      labelCls: useLabelCls(props),
      onClickInput,
      onClickListItem,
    };
  },
};
</script>

<style>
.base-field {
  @apply pr-5;
}
</style>