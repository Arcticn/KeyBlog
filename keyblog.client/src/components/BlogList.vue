<template>
  <el-container style="display: flex; justify-content: center">
    <el-main style="margin-bottom: 5rem; max-width: 60rem">
      <PostList
        :posts="posts"
        :total="total"
        :currentPage="currentPage"
        :pageSize="pageSize"
        @update:currentPage="handlePageChange"
        @view-post="viewPost"
        @sort-change="handleSortChange"
        @search-change="handleSearchChange"
      />
    </el-main>
    <el-aside>
      <el-affix :offset="100">
        <UserCard />
        <CategoryTree
          style="margin-top: 1rem"
          :categories="categoryNodes"
          :currentCategoryId="currentCategoryId"
          @node-click="handleNodeClick"
        />
      </el-affix>
      <!-- <iframe height="800" style="border:none;" src="https://ac.yunyoujun.cn"></iframe> -->
    </el-aside>
  </el-container>

  <!-- <el-footer>Footer</el-footer> -->
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import api from "@/services/api";
import PostList from "./PostList.vue";
import UserCard from "./UserCard.vue";
import CategoryTree from "./CategoryTree.vue";
import "element-plus/theme-chalk/display.css";
import "@/components/styles/style.scss";
import { ErrorMessage } from "@/composables/PopupMessage";

const posts = ref([]);
const total = ref(0);
const currentPage = ref(1);
const pageSize = ref(10);
const categoryNodes = ref([]);
const currentCategoryId = ref(0);
const router = useRouter();
const sortBy = ref("LastUpdateTime");
const sortType = ref("desc");
const searchQuery = ref("");

const fetchData = async (categoryId = 0, page = 1) => {
  try {
    const response = await api.get("Blog/lists", {
      params: {
        categoryId,
        page,
        search: searchQuery.value,
        sortType: sortType.value,
        sortBy: sortBy.value,
        pageSize: pageSize.value,
      },
    });
    const data = response.data;
    const rootCategory = { id: 0, name: "所有博客" };
    categoryNodes.value = [rootCategory, ...data.categoryNodes];
    posts.value = data.posts.items;
    total.value = data.posts.totalCount;
    currentCategoryId.value = categoryId;
    sortBy.value = data.sortBy;
    sortType.value = data.sortType;
    searchQuery.value = "";
    //currentPage.value = page;
  } catch (error) {
    ErrorMessage(error);
  }
};

const handleNodeClick = async (node) => {
  await fetchData(node.id, 1);
};

const handlePageChange = (page) => {
  currentPage.value = page;
  fetchData(currentCategoryId.value, page);
};

const handleSortChange = (requestSortBy, requestSortType) => {
  sortBy.value = requestSortBy;
  sortType.value = requestSortType;
  fetchData(currentCategoryId.value, currentPage.value);
};

const handleSearchChange = (query) => {
  searchQuery.value = query;
  fetchData(currentCategoryId.value, currentPage.value);
};

onMounted(() => {
  fetchData();
});

const viewPost = (postId) => {
  router.push({ name: "Post", params: { id: postId } });
};
</script>
