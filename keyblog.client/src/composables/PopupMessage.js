import { ElMessage } from "element-plus";

export const SuccessMessage = (newMessage) => {
  ElMessage({
    message: newMessage,
    duration: 1500,
    showClose: true,
    type: "success",
  });
};

export const WarningMessage = (newMessage) => {
  ElMessage({
    message: newMessage,
    duration: 1500,
    showClose: true,
    type: "warning",
  });
};

export const InfoMessage = (newMessage) => {
  ElMessage({
    message: newMessage,
    duration: 1500,
    showClose: true,
    type: "info",
  });
};

export const ErrorMessage = (newMessage) => {
  ElMessage({
    message: newMessage,
    duration: 1500,
    showClose: true,
    type: "error",
  });
};

