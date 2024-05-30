<template>
  <BaseHeader />
  <el-table
    :data="categoryNodes"
    style="width: 100%; margin-bottom: 20px"
    row-key="id"
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
</template>

<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";
import {
  SuccessMessage,
  ErrorMessage,
  WarningMessage,
} from "@/composables/PopupMessage.vue";
import { ElMessage, ElMessageBox } from 'element-plus'
import BaseHeader from "./layouts/BaseHeader.vue";

const categoryNodes = ref(null);
const editName=ref(null);

const handleCategoryCreate = (row) => {};

const handleCategoryEdit = (row) => {
  ElMessageBox.prompt('请输入新的类别名称', '修改', {
    confirmButtonText: 'OK',
    cancelButtonText: 'Cancel',
    message: 
  })
  try {
    const response = axios.put("/api/blog/deleteCategory",{params:{row.id,row.name}});
    SuccessMessage("删除成功",response.data.message);
  } catch (error) {
    ErrorMessage(error);
  }
};

const handleCategoryDelete = (row) => {
  try {
    const response = axios.delete("/api/blog/deleteCategory",row.id);
    SuccessMessage("删除成功",response.data.message);
  } catch (error) {
    ErrorMessage(error);
  }
};

const fetchData = async () => {
  try {
    const response = await axios.get("/api/blog/getCategories");
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
