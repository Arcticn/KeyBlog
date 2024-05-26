<template>
  <BaseHeader />
  <el-container>
    <el-main>
      <PostList
        :posts="posts"
        :total="total"
        :currentPage="currentPage"
        :pageSize="pageSize"
        @update:currentPage="handlePageChange"

        @view-post="viewPost"
      />
    </el-main>
    <el-aside id="categoryNodes" style="margin-right: 8rem">
      <el-affix :offset="100">
        <CategoryTree
          :categories="categoryNodes"
          :currentCategoryId="currentCategoryId"
          @node-click="handleNodeClick"
        />
      </el-affix>
      <!-- <iframe height="800" style="border:none;" src="https://ac.yunyoujun.cn"></iframe> -->
    </el-aside>
  </el-container>

  <el-footer>Footer</el-footer>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import PostList from "./PostList.vue";
import CategoryTree from "./CategoryTree.vue";
import "element-plus/theme-chalk/display.css";
import "@/components/styles/style.scss";

const posts = ref([]);
const total = ref(0);
const currentPage = ref(1);
const pageSize = ref(6);
const categoryNodes = ref([]);
const currentCategoryId = ref(0);
const router = useRouter();

const fetchData = async (categoryId = 0, page = 1) => {
  try {
    const response = await axios.get("/api/Blog/lists", {
      params: {
        categoryId,
        page,
        pageSize: pageSize.value,
      },
    });
    const data = response.data;
    categoryNodes.value = data.categoryNodes;
    posts.value = data.posts.items;
    total.value = data.posts.totalCount;
    currentCategoryId.value = categoryId;
    //currentPage.value = page;
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

const handleNodeClick = async (node) => {
  await fetchData(node.id, 1);
};

const handlePageChange = (page) => {
  currentPage.value = page;
  fetchData(currentCategoryId.value, page);
};

onMounted(() => {
  fetchData();
});

const viewPost = (postId) => {
  router.push({ name: "Post", params: { id: postId } });
};
</script>

<style scoped>
.mb-3 {
  width: 80%;
  padding-left: 2rem;
  margin-bottom: 1rem;
}

.sidebar .header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #ffffff;
  color: #409eff;
  padding: 0 20px;
}

.logo {
  font-size: 18px;
  font-weight: bold;
}

.el-menu-demo {
  background-color: transparent;
}
</style>
