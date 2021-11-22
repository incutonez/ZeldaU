<template>
  <div>
    <BaseField
      ref="field"
      v-bind="$attrs"
      @click:input="onClickInput"
    >
      <template #postInput>
        <div
          ref="triggerIcon"
          class="absolute h-full flex items-center top-0 right-0.5"
        >
          <ChevronDownIcon
            class="h-5 w-5 hover:text-blue-500 cursor-pointer"
          />
        </div>
      </template>
    </BaseField>
  </div>
  <ul
    ref="list"
    class="base-list hidden"
  >
    <li
      v-for="(record, idx) in store"
      :key="idx"
      class="base-list-item"
      @click="onClickListItem"
    >
      {{ record }}
    </li>
  </ul>
</template>

<script>
import BaseField from "@/components/BaseField.vue";
import {
  onMounted,
  onUnmounted,
  ref,
  watch
} from "vue";
import { ChevronDownIcon } from "@heroicons/vue/solid";

export default {
  name: "BaseFieldSelect",
  components: {
    BaseField,
    ChevronDownIcon,
  },
  inheritAttrs: false,
  props: {
    store: {
      type: Array,
      required: true,
    }
  },
  setup(props) {
    const list = ref(null);
    const field = ref(null);
    const triggerIcon = ref(null);
    const isExpanded = ref(false);

    watch(isExpanded, (value) => {
      if (value) {
        const position = field.value.$el.getBoundingClientRect();
        list.value.style.width = `${position.width}px`;
        list.value.style.left = `${position.x}px`;
        list.value.style.top = `${position.bottom}px`;
        list.value.classList.remove("hidden");
        field.value.getInputEl().focus();
      }
      else {
        list.value.classList.add("hidden");
      }
    });

    function toggleTrigger() {
      isExpanded.value = !isExpanded.value;
    }

    function showTrigger() {
      isExpanded.value = true;
    }

    function hideTrigger() {
      isExpanded.value = false;
    }

    function onClickInput(event) {
      if (triggerIcon.value.contains(event.target)) {
        toggleTrigger();
      }
      else {
        showTrigger();
      }
    }

    function onClickListItem() {
      hideTrigger();
    }

    function onClickDocument(event) {
      if (isExpanded.value && !(field.value.getInputEl().contains(event.target) || triggerIcon.value.contains(event.target))) {
        hideTrigger();
      }
    }

    onMounted(() => {
      /* Let's listen for any document clicks, as we'll need to collapse if user clicks on something
     * outside of this class */
      document.addEventListener("click", onClickDocument);
    });

    onUnmounted(() => {
      document.removeEventListener("click", onClickDocument);
    });

    return {
      list,
      field,
      triggerIcon,
      onClickInput,
      onClickListItem,
    };
  }
};
</script>

<style>
.base-list {
  @apply absolute text-sm w-full border border-t-0 shadow-sm;
}

.base-field {
  @apply pr-5;
}
</style>