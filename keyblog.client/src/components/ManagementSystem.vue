<template>
  <BaseHeader />
  <el-table
    :data="categoryNodes"
    style="margin-bottom: 20px; margin: 2rem"
    row-key="id"
    @row-click="fetchPosts"
  >
    <el-table-column prop="name" label="分类名称" sortable />
    <el-table-column label="操作">
      <template #default="scope">
        <el-button
          size="small"
          type="info"
          @click="handleCategoryCreate(scope.row)"
        >
          新增子类
        </el-button>
        <el-button
          size="small"
          type="primary"
          @click="handleCategoryEdit(scope.row)"
        >
          编辑
        </el-button>
        <el-button
          size="small"
          type="danger"
          @click="handleCategoryDelete(scope.row)"
        >
          删除
        </el-button>
      </template>
    </el-table-column>
  </el-table>
  <el-table
    :data="posts"
    style="margin-bottom: 20px; margin: 2rem"
    row-key="id"
  >
    <el-table-column prop="title" label="博客名称" sortable />
    <el-table-column label="操作">
      <template #default="scope">
        <el-button
          size="small"
          type="info"
          @click="handleCategoryCreate(scope.row)"
        >
          新增子类
        </el-button>
        <el-button
          size="small"
          type="primary"
          @click="handleCategoryEdit(scope.row)"
        >
          编辑
        </el-button>
        <el-button
          size="small"
          type="danger"
          @click="handleCategoryDelete(scope.row)"
        >
          删除
        </el-button>
      </template>
    </el-table-column>
  </el-table>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref, watch } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import BaseHeader from "./layouts/BaseHeader.vue";
import {
  WarningMessage,
  SuccessMessage,
  ErrorMessage,
} from "@/composables/PopupMessage.js";

const categoryNodes = ref(null);
const editName = ref(null);
const newName = ref(null);
const posts = ref(null);

const handleCategoryCreate = async (row) => {
  newName.value = (
    await ElMessageBox.prompt("请输入类别名称", "新增", {
      confirmButtonText: "确认",
      cancelButtonText: "取消",
    })
  ).value;

  const newCategory = {
    name: newName.value,
    parentId: row.id,
    visible: true,
  };

  try {
    const response = await axios.post("/api/Category/addCategory", newCategory);
    fetchData();
    SuccessMessage("新增成功", response.data.message);
  } catch (error) {
    ErrorMessage(error);
  }
};

const handleCategoryEdit = async (row) => {
  editName.value = (
    await ElMessageBox.prompt("请输入新的类别名称", "修改", {
      confirmButtonText: "确认",
      cancelButtonText: "取消",
    })
  ).value;
  console.log(row.id);
  try {
    const response = await axios.put("/api/Category/editCategory", null, {
      params: {
        id: row.id,
        name: editName.value,
      },
    });
    fetchData();
    SuccessMessage("修改成功", response.data.message);
  } catch (error) {
    ErrorMessage(error);
  }
};

const handleCategoryDelete = async (row) => {
  ElMessageBox.confirm("该类别所有帖子都将被一起删除，是否继续？", "警告", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      try {
        console.log(row.id);
        const response = await axios.delete("/api/Category/deleteCategory", {
          params: {
            id: row.id,
          },
        });
        fetchData();
        SuccessMessage("删除成功", response.data.message);
      } catch (error) {
        ErrorMessage(error);
      }
    })
    .catch(() => {
      ElMessage({
        type: "info",
        message: "Delete canceled",
      });
    });
};

const fetchPosts = async (row) => {
  try {
    const response = await axios.get("/api/Blog/lists", {
      params: {
        categoryId: row.id,
        page: 1,
        pageSize: 999,
      },
    });
    posts.value = response.data.posts.items;
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

const fetchData = async () => {
  try {
    const response = await axios.get("/api/Category/getCategories");
    const data = response.data;
    categoryNodes.value = data.categoryNodes;
    // console.log(categoryNodes);
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

onMounted(() => {
  fetchData();
});
</script>
