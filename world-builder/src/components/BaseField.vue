<template>
  <div
    class="flex"
    :class="[labelAlign, pack, width]"
  >
    <BaseFieldLabel
      :value="label"
      :class="labelCls"
    />
    <div
      class="relative flex"
      @click="onClickField"
    >
      <input
        v-model="value"
        class="base-field"
        :class="inputCls"
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
import { computed } from "vue";
import {
  baseFieldProps,
  useInputCls,
  useLabelCls
} from "@/components/useBaseField.js";

export default {
  name: "BaseField",
  components: { BaseFieldLabel },
  props: {
    ...baseFieldProps,
  },
  emits: [
    "update:modelValue",
    "input",
    "click:input",
    "blur:input",
    "change"
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

    return {
      value,
      labelCls: useLabelCls(props),
      inputCls: useInputCls(props),
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
