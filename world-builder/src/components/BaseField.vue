<template>
  <div
    class="flex"
    :class="labelAlign"
  >
    <BaseFieldLabel
      :value="label"
      :class="labelCls"
    />
    <input
      v-model="value"
      class="border text-sm py-0.5 px-1 outline-none focus:border-blue-500"
      @input="onInputField"
    >
  </div>
</template>

<script>
import BaseFieldLabel from "@/components/BaseFieldLabel.vue";
import { computed } from "vue";

export default {
  name: "BaseField",
  components: { BaseFieldLabel },
  props: {
    label: {
      type: String,
      default: "",
    },
    labelAlign: {
      type: String,
      default: "flex-col"
    },
    modelValue: {
      type: [ String, Number ],
      default: "",
    },
  },
  emits: [
    "update:modelValue",
    "input",
  ],
  setup(props, { emit }) {
    const value = computed({
      get() {
        return props.modelValue;
      },
      set(value) {
        emit("update:modelValue", value);
      }
    });

    const labelCls = computed(() => props.labelAlign === "flex-row" ? "mr-2" : "my-1");

    return {
      value,
      labelCls,
      onInputField(event) {
        emit("input", event);
      }
    };
  },
};
</script>
