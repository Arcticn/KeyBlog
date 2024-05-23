<template>
  <BaseHeader />
  <el-main style="padding: 3rem">
    <el-row>
      <el-col :span="24">
        <div class="input-container">
          <span>发布状态：</span>
          <el-radio-group v-model="isPublish">
            <el-radio-button value="true" border>已发布</el-radio-button>
            <el-radio-button value="false" border>未发布</el-radio-button>
          </el-radio-group>
        </div>
        <div class="input-container">
          <span>博客标题：</span>
          <el-input v-model="input_title" placeholder="在此处输入博客标题" />
        </div>
        <div class="input-container">
          <span>分类：</span>
          <el-select
            v-model="selectedCategory"
            style="max-width: 10rem;"
            placeholder="选择分类"
            @change="handleCategoryChange"
          >
            <el-option
              v-for="category in categories"
              :key="category.id"
              :label="category.name"
              :value="category.id"
            >
            </el-option>
          </el-select>
        </div>
        <div v-if="subcategories.length > 0" class="input-container">
          <span>子分类：</span>
          <el-select v-model="selectedSubcategory" placeholder="选择子分类">
            <el-option
              v-for="subcategory in subcategories"
              :key="subcategory.id"
              :label="subcategory.name"
              :value="subcategory.id"
            >
            </el-option>
          </el-select>
          <el-button @click="addSubcategory">添加子分类</el-button>
        </div>
      </el-col>
    </el-row>

    <MdEditor v-model="text" :theme="theme" class="editor" @onSave="onSave" />
  </el-main>
</template>

<script setup>
import { ref } from "vue";
import axios from "axios";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import BaseHeader from "./layouts/BaseHeader.vue";
import { useDarkMode } from "../composables/useDarkMode";

const input_title = ref("");
const isPublish = ref("false");

const selectedCategory = ref(null);
const selectedSubcategory = ref(null);
const categories = ref([
  {
    id: 1,
    name: "分类1",
    subcategories: [
      { id: 101, name: "子分类1-1" },
      { id: 102, name: "子分类1-2" },
    ],
  },
  { id: 2, name: "分类2", subcategories: [] },
  { id: 3, name: "分类3", subcategories: [{ id: 301, name: "子分类3-1" }] },
]);
const subcategories = ref([]);

const handleCategoryChange = (categoryId) => {
  const category = categories.value.find((c) => c.id === categoryId);
  if (category && category.subcategories.length > 0) {
    subcategories.value = category.subcategories;
    selectedSubcategory.value = null; // 重置子分类选择
  } else {
    subcategories.value = [];
  }
};

const addSubcategory = () => {
  const category = categories.value.find(
    (c) => c.id === selectedCategory.value
  );
  if (category) {
    const newSubcategoryId = Date.now(); // 简单生成一个唯一ID
    const newSubcategoryName = prompt("输入新的子分类名称：");
    if (newSubcategoryName) {
      const newSubcategory = { id: newSubcategoryId, name: newSubcategoryName };
      category.subcategories.push(newSubcategory);
      subcategories.value.push(newSubcategory); // 更新当前子分类列表
    }
  }
};

const { isDark, theme } = useDarkMode();

const onSave = async (v) => {
  try {
    console.log(isDark.value);
    const response = await axios.post("/api/blog/saveArticle", {
      title: input_title,
      content: v,
    });
    console.log("Content saved successfully:", response.data);
    // 处理成功响应
  } catch (error) {
    console.error("Error saving content:", error);
    // 处理错误响应
  }
};

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
