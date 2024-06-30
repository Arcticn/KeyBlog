<template>
  <el-container>
    <el-main class="markdown-content">
      <el-card
        v-loading="markdownContent === ''"
        class="glass-effect"
        style="margin-left: 5rem; margin-right: 1rem;min-height: 30rem"
      >
        <MdPreview
          :editorId="id"
          :theme="theme"
          :previewTheme="previewTheme"
          :codeTheme="codeTheme"
          v-model="markdownContent"
          @onGetCatalog="onGetCatalog"
        />
      </el-card>
    </el-main>
    <el-aside class="catalog-container" width="250px">
      <el-affix :offset="100">
        <el-card
          v-loading="markdownContent === ''"
          class="glass-effect"
          style="width: 11rem; padding-right: 10px;min-height: 10rem;max-height: 40rem;overflow-y: auto;"
          body-style="padding:10px"
          
        >
          <MdCatalog
            :editorId="id"
            style="font-size: 15px"
            :scrollElement="scrollElement"
          />
        </el-card>
      </el-affix>
    </el-aside>
  </el-container>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "@/services/api";
import { MdPreview, MdCatalog } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { previewTheme,codeTheme } from "@/composables/theme";
// import BaseHeader from "./layouts/BaseHeader.vue";
import { useDarkMode } from "../composables/useDarkMode";

import { ErrorMessage } from "@/composables/PopupMessage";
import '@vavt/cm-extension/dist/previewTheme/arknights.css';

// config({
//   markdownItConfig(mdit) {
//     mdit.use(anchor, {
//       permalink: anchor.permalink.linkAfterHeader({
//         style: "visually-hidden",
//         assistiveText: (title) => `Permalink to “${title}”`,
//         visuallyHiddenClass: "visually-hidden",
//         wrapper: ['<div class="wrapper">', "</div>"],
//       }),
//     });
//   },
// });

const id = "MD_EDITOR_ID"; //Preview和目录的editorId必须相同
const markdownContent = ref("");
const scrollElement = document.documentElement;

const { theme } = useDarkMode();

const props = defineProps({
  id: String,
});

const fetchAndRenderContent = async () => {
  try {
    const response = await api.get(`Post/posts/${props.id}`);
    const postData = response.data;
    markdownContent.value = postData.content;
  } catch (error) {
    ErrorMessage(error.response.data);
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
  padding: 20px;
  position: sticky;
  top: 10px; /* Adjust as needed to offset from the top */
}

/* Custom styles to fix code block line spacing */
/* .markdown-content pre {
  margin: 0 !important;
} */

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
