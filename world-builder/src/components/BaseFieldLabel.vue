<template>
  <label class="flex items-center text-blue-900 font-semibold uppercase text-xs">
    {{ value }}{{ separator }}
  </label>
</template>

<script>
export default {
  name: "BaseFieldLabel",
  props: {
    value: {
      type: String,
      default: "",
    },
    separator: {
      type: String,
      default: ":",
    }
  }
};
</script>
