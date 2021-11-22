﻿import { computed } from "vue";

export const baseFieldProps = {
  label: {
    type: String,
    default: "",
  },
  labelAlign: {
    type: String,
    default: "flex-col"
  },
  inputType: {
    type: String,
    default: "text",
  },
  modelValue: {
    type: [String, Number],
    default: "",
  },
};

export function useLabelCls(props) {
  return computed(() => {
    switch (props.labelAlign) {
      case "flex-row-reverse":
        return "ml-2";
      case "flex-col":
      case "flex-col-reverse":
        return "my-1";
      case "flex-row":
      default:
        return "mr-2";
    }
  });
}