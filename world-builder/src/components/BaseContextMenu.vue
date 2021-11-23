<template>
  <div
    ref="self"
    class="context-menu hidden"
  >
    <slot />
  </div>
</template>

<script>
import { ref } from "vue";

export default {
  name: "BaseContextMenu",
  setup() {
    const self = ref(null);

    function show(event) {
      self.value.style.left = `${event.pageX}px`;
      self.value.style.top = `${event.pageY}px`;
      self.value.classList.remove("hidden");
      event.preventDefault();
    }

    function hide() {
      self.value.classList.add("hidden");
    }

    return {
      self,
      show,
      hide
    };
  }
};
</script>
