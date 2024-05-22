<template>
  <BaseHeader />
  <el-container style="padding:1rem">
    <MdEditor v-model="text" 
    :theme="theme"
    class="editor"
    @onSave="onSave"
    />
  </el-container>
  
</template>

<script setup>
import { ref,watch } from "vue";
import  axios  from "axios";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import BaseHeader from "./layouts/BaseHeader.vue";
import { isDark } from "./composables/useDarkMode";

const theme = ref(isDark.value ? "dark" : "light");

watch(isDark, (newVal) => {
  theme.value = newVal ? "dark" : "light";
});

const onSave = async (v) => {
  try {
    const response = await axios.post('/api/blog/saveArticle', { content: v });
    console.log('Content saved successfully:', response.data);
    // 处理成功响应
  } catch (error) {
    console.error('Error saving content:', error);
    // 处理错误响应
  }
};


const text = ref("Hello Editor!");
</script>

<style scoped>
.editor {
    text-align: left;
}

</style>
