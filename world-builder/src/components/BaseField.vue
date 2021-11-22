<template>
  <div
    class="flex"
    :class="labelAlign"
  >
    <BaseFieldLabel
      :value="label"
      :class="labelCls"
    />
    <div
      class="relative"
      @click="onClickField"
    >
      <input
        ref="inputEl"
        v-model="value"
        class="base-field"
        :type="inputType"
        @input="onInputField"
        @blur="onBlurField"
      >
      <slot name="postInput" />
    </div>
  </div>
</template>

<script>
import BaseFieldLabel from "@/components/BaseFieldLabel.vue";
import {
  computed,
  ref
} from "vue";
import {
  baseFieldProps,
  useLabelCls
} from "@/components/useBaseField.js";

export default {
  name: "BaseField",
  components: { BaseFieldLabel },
  props: {
    ...baseFieldProps,
    modelValue: {
      type: [String, Number],
      default: "",
    },
    inputType: {
      type: String,
      default: "text",
    },
  },
  emits: [
    "update:modelValue",
    "input",
    "click:input",
    "blur:input"
  ],
  setup(props, { emit }) {
    const inputEl = ref(null);
    const value = computed({
      get() {
        return props.modelValue;
      },
      set(value) {
        emit("update:modelValue", value);
      }
    });

    return {
      value,
      inputEl,
      labelCls: useLabelCls(props),
      getInputEl() {
        return inputEl.value;
      },
      onInputField(event) {
        emit("input", event);
      },
      onClickField(event) {
        emit("click:input", event);
      },
      onBlurField(event) {
        emit("blur:input", event);
      }
    };
  },
};
</script>

<style>
.base-field {
  @apply border text-sm py-0.5 px-1 outline-none focus:border-blue-500;
}
</style>