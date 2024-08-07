<template>
  <el-main
    style="
      padding: 3rem;
      display: flex;
      justify-content: flex-start;
      flex-flow: row wrap;
    "
  >
    <el-table
      v-loading="categoryNodesWithRoot === null"
      class="glass-effect"
      ref="categoryTable"
      :data="categoryNodesWithRoot"
      style="
        margin-bottom: 2rem;
        margin-right: 4rem;
        height: fit-content;
        width: 502px;
      "
      row-key="id"
      @row-click="updateRow"
    >
      <el-table-column prop="name" label="分类名称" width="200" sortable />
      <el-table-column label="操作" width="300">
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
            plain
            @click="openCategoryMoveDialog(scope.row)"
          >
            移动
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
    <!-- v-if="(posts && posts.length > 0) || loadPostStatus === false" -->
    <el-table
      v-loading="loadPostStatus"
      class="glass-effect"
      :data="posts"
      style="margin-bottom: 2rem; height: fit-content; width: 692px"
      row-key="id"
    >
      <el-table-column prop="title" label="博客名称" width="260" sortable />
      <el-table-column
        prop="isPublish"
        :formatter="formatPublish"
        label="状态"
        width="100"
        sortable
      />
      <el-table-column
        prop="lastUpdateTime"
        :formatter="formatDate1"
        label="最后修改时间"
        width="180"
        sortable
      />
      <el-table-column label="操作" width="150">
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
    <!-- </el-card> -->
  </el-main>

  <el-dialog
    v-model="dialogTableVisible"
    title="移动分类"
    style="width: fit-content"
  >
    <el-table
      :data="categoryNodesWithRoot"
      @row-click="handleCategoryMove"
      row-key="id"
    >
      <el-table-column prop="name" label="分类名称" width="560" sortable />
    </el-table>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="dialogTableVisible = false">关闭</el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup>
import api from "@/services/api";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { ElMessage, ElMessageBox } from "element-plus";
// import BaseHeader from "./layouts/BaseHeader.vue";
import {
  WarningMessage,
  SuccessMessage,
  ErrorMessage,
} from "@/composables/PopupMessage.js";

const categoryRow = ref(null);
const categoryNodes = ref(null);
const categoryNodesWithRoot = ref(null);
const editName = ref(null);
const newName = ref(null);
const posts = ref(null);
const router = useRouter();
const categoryTable = ref(null);
const loadPostStatus = ref(false);

const formatDate1 = (row, column, cellValue) => {
  if (!cellValue) return "";
  // 创建 UTC 时间的 Date 对象
  const date = new Date(cellValue);

  // 将 UTC 时间转换为本地时间
  const localDate = new Date(date.getTime() - date.getTimezoneOffset() * 60000);

  return localDate.toLocaleString();
};

const formatPublish = (row, column, cellValue) => {
  return cellValue ? "已发布" : "未发布";
};

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
    ErrorMessage(error.response.data);
  }
};

const handleCategoryEdit = async (row) => {
  if (row.id === 0) {
    WarningMessage("根目录不可编辑", "根目录不可编辑");
    return;
  }
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
    ErrorMessage(error.response.data);
  }
};

const dialogTableVisible = ref(false);
const categoryToMove = ref(null);
const openCategoryMoveDialog = (row) => {
  if (row.id === 0) {
    WarningMessage("根目录不可移动", "根目录不可移动");
    return;
  }
  dialogTableVisible.value = true;
  categoryToMove.value = row;
};

const handleCategoryMove = async (row) => {
  await ElMessageBox.confirm("移动到此目录？", "移动", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
  });
  console.log(categoryToMove.value.id);
  console.log("Selected ID:", row.id);
  try {
    const response = await api.put("Category/moveCategory", null, {
      params: {
        id: categoryToMove.value.id,
        parentId: row.id,
      },
    });
    SuccessMessage("移动成功", response.data.message);
    // 在这里处理用户确认后的逻辑
  } catch (error) {
    // 处理用户取消或关闭对话框的情况
    ErrorMessage(error.response.data);
  }
};

const handleCategoryDelete = async (row) => {
  if (row.id === 0) {
    WarningMessage("根目录不可删除", "根目录不可删除");
    return;
  }
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
        ErrorMessage(error.response.data);
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
        ErrorMessage(error.response.data);
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
  // console.log(row);
  categoryTable.value.toggleRowExpansion(row);
  categoryRow.value = row;
  fetchPosts();
};

const fetchPosts = async () => {
  loadPostStatus.value = true;
  // console.log(categoryRow.value);
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
  } finally {
    loadPostStatus.value = false;
  }
};

const fetchData = async () => {
  try {
    const response = await api.get("Category/getCategories");
    const data = response.data;
    categoryNodes.value = data.categoryNodes;
    addRootNode();
    // console.log(categoryNodes);

    if (categoryNodesWithRoot.value.length > 0) {
      // 延迟以确保数据绑定到表格后再展开第一行
      setTimeout(() => {
        categoryTable.value.toggleRowExpansion(
          categoryNodesWithRoot.value[0],
          true
        );
      }, 0);
    }
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

const addRootNode = () => {
  const rootNode = {
    id: 0,
    name: "根目录",
    parentId: 0,
    children: categoryNodes.value,
  };
  categoryNodesWithRoot.value = [rootNode];
};

onMounted(() => {
  fetchData();
});
</script>
