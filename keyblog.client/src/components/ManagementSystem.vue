<template>
  <BaseHeader />
  <el-main style="padding: 3rem">
    <el-card class="glass-effect" style="margin-bottom: 2rem;max-width: fit-content">
      <el-table
        :data="categoryNodes"
        style="width: 30rem"
        row-key="id"
        @row-click="updateRow"
      >
        <el-table-column prop="name" label="分类名称" width="180" sortable />
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
    </el-card>
    <el-card class="glass-effect" style="margin-bottom: 2rem;max-width: fit-content">
      <el-table
        :data="posts"
        style="max-width: 30rem"
        row-key="id"
      >
        <el-table-column prop="title" label="博客名称" sortable />
        <el-table-column label="操作">
          <template #default="scope1">
            <el-button
              size="small"
              type="primary"
              @click="handlePostEdit(scope1.row)"
            >
              编辑
            </el-button>
            <el-button
              size="small"
              type="danger"
              @click="handlePostDelete(scope1.row)"
            >
              删除
            </el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </el-main>
</template>

<script setup>
import api from "@/services/api";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { ElMessage, ElMessageBox } from "element-plus";
import BaseHeader from "./layouts/BaseHeader.vue";
import {
  WarningMessage,
  SuccessMessage,
  ErrorMessage,
} from "@/composables/PopupMessage.js";

const categoryRow = ref(null);
const categoryNodes = ref(null);
const editName = ref(null);
const newName = ref(null);
const posts = ref(null);
const router = useRouter();

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
    const response = await api.post("Category/addCategory", newCategory);
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
    const response = await api.put("Category/editCategory", null, {
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
        const response = await api.delete("Category/deleteCategory", {
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

const handlePostEdit = async (row) => {
  router.push({
    path: "/editor",
    query: {
      id: row.id,
    },
  });
};

const handlePostDelete = async (row) => {
  ElMessageBox.confirm("是否删除该帖子？", "警告", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      try {
        const response = await api.delete("Post/deletePost", {
          params: {
            id: row.id,
          },
        });
        fetchPosts();
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

const updateRow = (row) => {
  categoryRow.value = row;
  fetchPosts();
};

const fetchPosts = async () => {
  console.log(categoryRow.value);
  try {
    const response = await api.get("Blog/lists", {
      params: {
        categoryId: categoryRow.value.id,
        page: 1,
        pageSize: 999,
        adminMode: true,
      },
    });
    posts.value = response.data.posts.items;
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

const fetchData = async () => {
  try {
    const response = await api.get("Category/getCategories");
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
