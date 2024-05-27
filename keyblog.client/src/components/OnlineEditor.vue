<template>
  <BaseHeader />
  <el-main style="padding: 3rem">
    <el-row>
      <el-card class="glass-effect" style="width: 100rem">
        <el-col :span="24">
          <div class="input-container">
            <span>发布状态：</span>
            <el-radio-group v-model="isPublish">
              <el-radio-button value="true" border>已发布</el-radio-button>
              <el-radio-button value="false" border>未发布</el-radio-button>
            </el-radio-group>
          </div>
          <div class="input-container">
            <span>发布时间：</span>
            <el-date-picker
              v-model="time"
              type="datetime"
              placeholder="Select date and time"
            />
          </div>
          <div class="input-container">
            <span>博客标题：</span>
            <el-input
              v-model="input_title"
              placeholder="在此处输入博客标题"
              style="max-width: 50rem"
            />
          </div>
          <div class="input-container">
            <span>文章分类：</span>
            <CategorySelector
              :categories="categories"
              v-model:selectedCategoryId="selectedCategoryId"
              @add-category="updateCategories"
            />
          </div>
        </el-col>
      </el-card>
    </el-row>
    <el-card class="glass-effect">
      <MdEditor v-model="text" :theme="theme" class="editor" @onSave="onSave" />
    </el-card>
  </el-main>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import BaseHeader from "./layouts/BaseHeader.vue";
import { useDarkMode } from "../composables/useDarkMode";
import CategorySelector from "./CategorySelector.vue";

const input_title = ref("");
const isPublish = ref("false");
const selectedCategoryId = ref(null);

const { theme } = useDarkMode();

const time = ref(null);
const categories = ref(null);

const updateCategories = async (newCategories) => {
  try {
    console.log("newCategory:", newCategories);
    const response = await axios.post("/api/blog/addCategory", newCategories);
    console.log("Category added successfully:", response.data);
    fetchData();
  } catch (error) {
    console.log("Error adding category:", error);
  }
};

const onSave = async (v) => {
  try {
    const response = await axios.post("/api/blog/savePost", {
      title: input_title.value,
      content: v,
      categoryId: selectedCategoryId.value,
      isPublish: isPublish.value,
    });
    console.log("Content saved successfully:", response.data);
  } catch (error) {
    console.error("Error saving content:", error);
  }
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

onMounted(() => {
  fetchData();
});

const text = ref("Hello Editor!");
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
