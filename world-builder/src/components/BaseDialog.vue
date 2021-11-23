<template>
  <div
    ref="self"
    class="base-dialog hidden"
  >
    <div class="base-dialog-content">
      <div class="base-dialog-title">
        <slot name="title">
          <span class="text-white">{{ title }}</span>
          <XIcon
            class="h-5 w-5 ml-auto cursor-pointer hover:text-blue-100"
            @click="onClickCloseButton"
          />
        </slot>
      </div>
      <slot />
    </div>
  </div>
</template>

<script>
import { ref } from "vue";
import { XIcon } from "@heroicons/vue/solid";

export default {
  name: "BaseDialog",
  components: {
    XIcon
  },
  props: {
    title: {
      type: String,
      default: ""
    }
  },
  setup() {
    const self = ref(null);

    function show() {
      self.value.classList.remove("hidden");
    }

    function hide() {
      self.value.classList.add("hidden");
    }

    function onClickCloseButton() {
      hide();
    }

    return {
      self,
      show,
      onClickCloseButton
    };
  }
};
</script>

<style scoped>

</style>