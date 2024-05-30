<template>
  <BaseHeader />
  <el-main style="padding: 3rem">
    <el-card class="glass-effect" style="margin-bottom: 2rem">
      <el-row justify="space-between" style="margin-bottom: 0">
        <el-col :span="13">
          <div class="input-container">
            <span>发布状态：</span>
            <el-radio-group v-model="publishState">
              <el-radio-button :value="true" border>已发布</el-radio-button>
              <el-radio-button :value="false" border>未发布</el-radio-button>
            </el-radio-group>
          </div>
        </el-col>
        <el-col :span="11">
          <div
            class="input-container"
            style="
              display: flex;
              justify-content: flex-end;
              align-items: center;
            "
          >
            <el-radio-group v-model="saveMethod">
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
          <div class="input-container">
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
              style="max-width: 50rem"
            />
          </div>
          <div class="input-container">
            <span>文章分类：</span>
            <CategorySelector
              :categories="categories"
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
import axios from "axios";
import { MdEditor } from "md-editor-v3";
import { ElMessage } from "element-plus";
import "md-editor-v3/lib/style.css";
import BaseHeader from "./layouts/BaseHeader.vue";
import { useDarkMode } from "../composables/useDarkMode";
import CategorySelector from "./CategorySelector.vue";

const defaultDateTime = new Date();

const text = ref("Hello Editor!");
const inputTitle = ref("");
const publishState = ref(false);
const selectedCategoryId = ref(null);
const time = ref(defaultDateTime);
const saveMethod = ref("true");

const { theme } = useDarkMode();

const categories = ref(null);

const updateCategoryId = async (newId) => {
  selectedCategoryId.value = newId;
  console.log(selectedCategoryId.value);
};

const updateCategories = async (newCategories) => {
  try {
    console.log("newCategory:", newCategories);
    const response = await axios.post("/api/blog/addCategory", newCategories);
    console.log("Category added successfully:", response.data);
    fetchData();
    SuccessMessage("成功添加分类");
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
    Title: inputTitle.value,
    Content: text.value,
    CategoryId: selectedCategoryId.value,
    Summary: null,
    CreationTime: time.value,
    IsPublish: publishState.value,
  };
  if (saveMethod.value === "true") {
    saveRemote(newPost);
  } else {
    saveLocal(newPost);
  }
};

const saveRemote = async (newPost) => {
  try {
    const response = await axios.post("/api/blog/savePost", newPost);
    SuccessMessage("Content saved successfully:", response.data);
  } catch (error) {
    ErrorMessage("Error saving content:", error);
  }
};

const saveLocal = (newPost) => {
  // Add your saveLocal implementation here
};

const fetchData = async () => {
  try {
    const response = await axios.get("/api/blog/getCategories");
    const data = response.data;
    categories.value = data.categoryNodes;
    console.log(categories);
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

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

onMounted(() => {
  fetchData();
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
