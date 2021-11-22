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
    const inputEl = ref(null);
    // TODOJEF: Doesn't work if fieldValue is not defined... get Vue warning
    const value = inject("fieldValue") || computed({
      get() {
        console.log("using");
        return props.modelValue;
      },
      set(value) {
        setValue(value);
      }
    });

    console.log(props.modelValue);

    function setValue(value) {
      console.log("updating");
      emit("update:modelValue", value);
    }

    return {
      value,
      inputEl,
      labelCls: useLabelCls(props),
      setValue,
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
