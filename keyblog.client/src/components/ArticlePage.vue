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
import { ref, onMounted } from "vue";
import axios from "axios";
import { MdPreview, MdCatalog } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import BaseHeader from "./layouts/BaseHeader.vue";
import { useDarkMode } from "../composables/useDarkMode";
import anchor from "markdown-it-anchor";
import { config } from "md-editor-v3";

config({
  markdownItConfig(mdit) {
    mdit.use(anchor, {
      permalink: anchor.permalink.linkAfterHeader({
        style: "visually-hidden",
        assistiveText: (title) => `Permalink to “${title}”`,
        visuallyHiddenClass: "visually-hidden",
        wrapper: ['<div class="wrapper">', "</div>"],
      })
    });
  },
});

const id = "MD_EDITOR_ID";
const markdownContent = ref("");
const scrollElement = document.documentElement;

const { theme } = useDarkMode();

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
.markdown-content .md-editor-preview {
  text-align: left;
}

.catalog-container {
  overflow-y: auto;
  max-height: calc(
    100vh - 80px
  ); /* 80px is the combined height of header and footer */
  padding: 20px;
  position: sticky;
  top: 10px; /* Adjust as needed to offset from the top */
}

/* Custom styles to fix code block line spacing */
.markdown-content pre {
  margin: 0 !important;
}

.md-editor {
  --md-bk-color: transparent; /* 使用网页的背景色 */
  /* --md-color: inherit; 文字颜色继承网页的颜色 */
}
.md-editor-catalog-active > span {
  color: #409eff;
}
.md-editor-catalog-link span:hover {
  color: #a0cfff;
}
</style>
