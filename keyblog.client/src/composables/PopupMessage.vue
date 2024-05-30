<script setup>
import { ElMessage } from "element-plus";
import { defineExpose } from "vue";

defineExpose({SuccessMessage,WarningMessage,ErrorMessage})

const WarningMessage = (newMeassage) => {
  ElMessage({
    message: newMeassage,
    duration: 1500,
    showClose: true,
    type: "warning",
  });
};

const SuccessMessage = (newMeassage) => {
  ElMessage({
    message: newMeassage,
    duration: 1500,
    showClose: true,
    type: "success",
  });
};

const ErrorMessage = (newMeassage) => {
  ElMessage({
    message: newMeassage,
    duration: 1500,
    showClose: true,
    type: "error",
  });
};

</script>