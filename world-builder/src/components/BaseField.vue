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
  inject,
  ref,
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
