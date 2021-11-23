<template>
  <BaseField
    input-type="checkbox"
    v-bind="$attrs"
  />
</template>

<script>
import BaseField from "@/components/BaseField.vue";

export default {
  name: "BaseCheckbox",
  components: { BaseField }
};
</script>
