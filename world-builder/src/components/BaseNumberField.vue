<template>
  <BaseField
    v-bind="$attrs"
    input-type="number"
    @keydown.up.prevent
    @keydown.down.prevent
    @wheel.prevent
  />
</template>

<script>
import BaseField from "@/components/BaseField.vue";

export default {
  name: "BaseNumberField",
  components: { BaseField }
};
</script>

<style>
/* Chrome, Safari, Edge, Opera */
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

/* Firefox */
input[type=number] {
  -moz-appearance: textfield;
}
</style>