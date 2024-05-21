<template>
  <el-container>
    <BaseHeader />
    <el-container>
      <el-main class="markdown-content">
        <MdPreview
          :editorId="id"
          :theme="theme"
          v-model="markdownContent"
          @onGetCatalog="onGetCatalog"
        />
      </el-main>
      <el-aside class="catalog-container" width="250px">
        <el-affix :offset="10">
          <MdCatalog :editorId="id" :scrollElement="scrollElement" />
        </el-affix>
      </el-aside>
    </el-container>
    <el-footer class="footer">Footer Content</el-footer>
  </el-container>
</template>

<script setup>
import { ref, onMounted,watch } from "vue";
import axios from "axios";
import { MdPreview, MdCatalog } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import BaseHeader from "./layouts/BaseHeader.vue";
import { isDark } from "./composables/useDarkMode";

const id = "MD_EDITOR_ID";
const markdownContent = ref("") ;
const scrollElement = document.documentElement;
const theme = ref(isDark.value ? "dark" : "light");

watch(isDark, (newVal) => {
  theme.value = newVal ? "dark" : "light";
});

const props = defineProps({
  id: String,
});

const fetchAndRenderContent = async () => {
  try {
    const response = await axios.get(`/api/Blog/articles/${props.id}`); 
    const postData = response.data;
    markdownContent.value = postData.content;
  } catch (error) {
    console.error("Error fetching post data:", error);
  }
};

onMounted(() => {
  fetchAndRenderContent();
  
});


</script>

<style>

.header {
  padding: 10px;
  background-color: #f5f5f5;
  text-align: center;
}

.footer {
  padding: 10px;
  background-color: #f5f5f5;
  text-align: center;
}



.markdown-content {
  flex: 1;
  overflow-y: auto;
  padding: 20px;
}

.markdown-content .md-editor-preview {
  text-align: left;
}

.catalog-container {
  overflow-y: auto;
  max-height: calc(100vh - 80px); /* 80px is the combined height of header and footer */
  padding: 20px;
  position: sticky;
  top: 10px; /* Adjust as needed to offset from the top */
}

/* Custom styles to fix code block line spacing */
.markdown-content pre {
  margin: 0 !important;
}

.markdown-content code {
  white-space: pre-wrap !important;
  display: block;
}
</style>