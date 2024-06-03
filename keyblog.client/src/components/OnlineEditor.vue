<template>
  <BaseHeader />
  <el-main style="padding: 3rem">
    <el-card class="glass-effect" style="margin-bottom: 2rem">
      <el-row justify="space-between" style="margin-bottom: 0" v-if="isAdmin">
        <el-col :span="24">
          <div class="input-container">
            <span>发布状态：</span>
            <el-radio-group v-model="publishState">
              <el-radio-button :value="true" border>已发布</el-radio-button>
              <el-radio-button :value="false" border>未发布</el-radio-button>
            </el-radio-group>
            <el-radio-group v-model="saveMethod" v-if="isAdmin" style="margin-left: auto">
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
    <el-card class="glass-effect">
      <MdEditor v-model="text" :theme="theme" class="editor" @onSave="onSave" />
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
import BaseHeader from "./layouts/BaseHeader.vue";
import { useDarkMode } from "../composables/useDarkMode";
import CategorySelector from "./CategorySelector.vue";

const defaultDateTime = new Date();

const blogId = ref(null);
const text = ref("Hello Editor!");
const inputTitle = ref("");
const publishState = ref(false);
const selectedCategoryId = ref(null);
const time = ref(defaultDateTime);
const saveMethod = ref("true");
const route = useRoute();

const incomingId = ref(null);
const incomingCategoryId = ref(null);

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
    ErrorMessage("Error adding category:", error);
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
    SuccessMessage("Content saved successfully:", response.data);
  } catch (error) {
    ErrorMessage("Error saving content:", error);
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
  }
};

const isAdmin = ref(true);
onMounted(() => {
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
