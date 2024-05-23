<template>
  <BaseHeader />
  <el-container>
    <el-main>

      <ArticleList
        :articles="articles"
        :total="total"
        :currentPage="currentPage"
        @update:currentPage="(newPage) => (currentPage = newPage)"
        :pageSize="pageSize"
        @page-change="handlePageChange"
        @view-article="viewArticle"
      />
    </el-main>
    <el-aside id="categoryNodes" style="padding-right: 4rem">
      <CategoryTree
        :categories="categoryNodes"
        :expandedKeys="expandedKeys"
        :currentCategoryId="currentCategoryId"
        @node-click="handleNodeClick"
      />
      <!-- <iframe height="800" style="border:none;" src="https://ac.yunyoujun.cn"></iframe> -->
    </el-aside>
  </el-container>

  <el-footer>Footer</el-footer>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import ArticleList from "./ArticleList.vue";
import CategoryTree from "./CategoryTree.vue";
import "element-plus/theme-chalk/display.css";

const articles = ref([]);
const total = ref(0);
const currentPage = ref(1);
const pageSize = ref(6);
const categoryNodes = ref([]);
const currentCategoryId = ref(0);
const expandedKeys = ref([]);
const router = useRouter();

const value1 = ref(true);
const value2 = ref(true);

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
    articles.value = data.articles.items;
    total.value = data.articles.totalCount;
    currentCategoryId.value = categoryId;
    currentPage.value = page;
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

const handleNodeClick = async (node) => {
  await fetchData(node.id, 1);
};

const handlePageChange = (page) => {
  fetchData(currentCategoryId.value, page);
};

onMounted(() => {
  fetchData();
});

const viewArticle = (articleId) => {
  router.push({ name: "Article", params: { id: articleId } });
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
