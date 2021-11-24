<template>
  <button
    :type="type"
    class="bg-blue-500 hover:bg-blue-700 text-white font-bold px-2 py-1 text-sm rounded"
  >
    {{ text }}
  </button>
</template>

<script>
export default {
  name: "BaseButton",
  props: {
    text: {
      type: String,
      default: ""
    },
    type: {
      type: String,
      default: "button"
    },
  }
};
</script>
