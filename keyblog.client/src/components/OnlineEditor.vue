<template>
  <el-main style="padding: 3rem">
    <el-card
      class="glass-effect"
      v-loading="incomingLoadingState"
      style="margin-bottom: 2rem"
    >
      <el-row justify="space-between" style="margin-bottom: 0" v-if="isAdmin">
        <el-col :span="24">
          <div class="input-container">
            <span>发布状态：</span>
            <el-radio-group v-model="publishState">
              <el-radio-button :value="true" border>已发布</el-radio-button>
              <el-radio-button :value="false" border>未发布</el-radio-button>
            </el-radio-group>
            <el-radio-group
              v-model="saveMethod"
              v-if="isAdmin"
              style="margin-left: auto"
            >
              <el-radio-button value="true" border>远程</el-radio-button>
              <el-radio-button value="false" border>本地</el-radio-button>
            </el-radio-group>
            <span style="margin-left: 2rem">
              <el-button type="primary" plain @click="onSave">保存</el-button>
            </span>
          </div>
        </el-col>
      </el-row>
      <el-row style="margin-bottom: 0">
        <el-col :span="24">
          <div class="input-container" v-if="isAdmin">
            <span>发布时间：</span>
            <el-date-picker
              v-model="time"
              type="datetime"
              placeholder="Select date and time"
              @change="console.log(time)"
            />
          </div>
          <div class="input-container">
            <span>博客标题：</span>
            <el-input
              v-model="inputTitle"
              placeholder="在此处输入博客标题"
              style="max-width: 50rem; margin-right: 2rem"
            />
            <span style="margin-left: auto" v-if="!isAdmin">
              <el-button type="primary" plain @click="onSave">保存</el-button>
            </span>
          </div>
          <div class="input-container" v-if="isAdmin">
            <span>文章分类：</span>
            <CategorySelector
              :categories="categories"
              :incomingCategoryId="incomingCategoryId"
              @add-category="updateCategories"
              @id-change="updateCategoryId"
            />
          </div>
        </el-col>
      </el-row>
    </el-card>
    <el-card
      v-loading="incomingLoadingState"
      class="glass-effect markdown-content"
    >
      <MdEditor
        v-model="text"
        :theme="theme"
        style="height:45rem;"
        :previewTheme="previewTheme"
        :codeTheme="codeTheme"
        :toolbars="toolbars"
        class="editor"
        @onSave="onSave"
      >
        <template #defToolbars>
          <ExportPDF :modelValue="text" :file-name="inputTitle"/>
          <Emoji />
        </template>
      </MdEditor>
    </el-card>
  </el-main>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "@/services/api";
import { MdEditor } from "md-editor-v3";
import { useRoute } from "vue-router";
import {
  WarningMessage,
  SuccessMessage,
  ErrorMessage,
} from "@/composables/PopupMessage.js";
import "md-editor-v3/lib/style.css";
// import BaseHeader from "./layouts/BaseHeader.vue";
import { codeTheme,previewTheme } from "@/composables/theme";
import { useDarkMode } from "../composables/useDarkMode";
import CategorySelector from "./CategorySelector.vue";
import "md-editor-v3/lib/style.css";
import '@vavt/cm-extension/dist/previewTheme/arknights.css';
import { ExportPDF } from "@vavt/v3-extension";
import { Emoji } from '@vavt/v3-extension';
// All CSS for this extension library
// import '@vavt/v3-extension/lib/asset/style.css';
// Or individual style for Emoji
import '@vavt/v3-extension/lib/asset/Emoji.css';
import "@vavt/v3-extension/lib/asset/ExportPDF.css";

const toolbars = [
  'bold',
  'underline',
  'italic',
  '-',
  'title',
  'strikeThrough',
  'sub',
  'sup',
  'quote',
  'unorderedList',
  'orderedList',
  'task',
  1,
  '-',
  'codeRow',
  'code',
  'link',
  'image',
  'table',
  'mermaid',
  'katex',
  '-',
  'revoke',
  'next',
  'save',
  0,
  '=',
  'pageFullscreen',
  'fullscreen',
  'preview',
  'previewOnly',
  'htmlPreview',
  'catalog',
  'github'
];

const defaultDateTime = new Date();
const blogId = ref(null);
const text = ref("# Hello Editor!");
const inputTitle = ref("");
const publishState = ref(false);
const selectedCategoryId = ref(null);
const time = ref(defaultDateTime);
const saveMethod = ref("true");
const route = useRoute();

const incomingId = ref(null);
const incomingCategoryId = ref(null);
const incomingLoadingState = ref(false);

const { theme } = useDarkMode();

const categories = ref(null);

const updateCategoryId = async (newId) => {
  selectedCategoryId.value = newId;
  console.log(selectedCategoryId.value);
};

const updateCategories = async (newCategories) => {
  try {
    console.log("newCategory:", newCategories);
    const response = await api.post("Category/addCategory", newCategories);
    fetchData();
    SuccessMessage("成功添加分类", response.data);
  } catch (error) {
    ErrorMessage(error.response.data);
  }
};

const onSave = async () => {
  console.log(selectedCategoryId.value);
  console.log(publishState.value);
  if (inputTitle.value === "") {
    WarningMessage("请输入标题");
    return;
  }

  if (saveMethod.value === "true") {
    if (selectedCategoryId.value === null) {
      WarningMessage("请选择分类");
      return;
    }
    if (text.value === "") {
      WarningMessage("请输入内容");
      return;
    }
    if (time.value === null) {
      WarningMessage("请选择发布时间");
      return;
    }

    const newPost = {
      Id: null,
      Title: inputTitle.value,
      Content: text.value,
      CategoryId: selectedCategoryId.value,
      Summary: null,
      CreationTime: time.value,
      IsPublish: publishState.value,
    };
    if (incomingId.value) {
      newPost.Id = incomingId.value;
    }
    saveRemote(newPost);
  } else {
    const newPost = {
      Title: inputTitle.value,
      Content: text.value,
    };
    saveLocal(newPost);
  }
};

const saveRemote = async (newPost) => {
  try {
    const response = await api.post("Post/savePost", newPost);
    incomingId.value = response.data.id;
    SuccessMessage("Content saved successfully:", response.data);
  } catch (error) {
    ErrorMessage(error.response.data);
  }
};

const saveLocal = (newPost) => {
  const blob = new Blob([newPost.Content], {
    type: "text/markdown;charset=utf-8",
  });
  const url = URL.createObjectURL(blob);
  const a = document.createElement("a");
  a.href = url;
  const filename = newPost.Title + ".md";
  a.download = filename;
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
  URL.revokeObjectURL(url);
};

const fetchData = async () => {
  try {
    const response = await api.get("Category/getCategories");
    const data = response.data;
    categories.value = data.categoryNodes;
    console.log(categories);
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

const fetchPost = async () => {
  incomingLoadingState.value = true;
  try {
    const response = await api.get(`Post/posts/${blogId.value}`);
    const postData = response.data;
    incomingId.value = postData.id;
    inputTitle.value = postData.title;
    text.value = postData.content;
    incomingCategoryId.value = postData.categoryId;
    time.value = postData.creationTime;
    publishState.value = postData.isPublish;
  } catch (error) {
    console.error("Error fetching post data:", error);
  } finally {
    incomingLoadingState.value = false;
  }
};

const isAdmin = ref(true);
onMounted(() => {
  console.log(codeTheme);
  const currentUser = JSON.parse(localStorage.getItem("currentUser"));
  if (currentUser) {
    if (!currentUser.isAdmin) {
      isAdmin.value = false;
    }
  } else isAdmin.value = false;
  saveMethod.value = isAdmin.value ? "true" : "false";

  fetchData();
  blogId.value = route.query.id;
  if (blogId.value) {
    fetchPost();
  }
});
</script>

<style scoped>
/* .markdown-content pre {
  margin: 0 !important;
} */

.input-container {
  display: flex;
  align-items: center;
  white-space: nowrap;
  margin-bottom: 20px;
}

.editor {
  text-align: left;
}

.el-row {
  margin-bottom: 20px;
}
</style>
