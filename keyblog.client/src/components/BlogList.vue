<template>
  <el-container>
    <el-header>
      <h2>Blog</h2>
      <span> Posts</span>
    </el-header>
    <el-main>
      <el-row :gutter="20">
        <el-col :span="16">
          <PostList
            :posts="posts"
            :total="total"
            :currentPage="currentPage"
            @update:currentPage="(newPage) => (currentPage = newPage)"
            :pageSize="pageSize"
            @page-change="handlePageChange"
            @view-post="viewPost"
          />
        </el-col>
        <el-col :span="8" class="mb-3" id="categoryNodes">
          <CategoryTree
            :categories="categoryNodes"
            :expandedKeys="expandedKeys"
            :currentCategoryId="currentCategoryId"
            @node-click="handleNodeClick"
            @remove-key="(key) => expandedKeys.splice(key, 1)"
            @add-key="(key) => expandedKeys.push(key)"
          />
        </el-col>
      </el-row>
    </el-main>
    <el-footer>Footer</el-footer>
  </el-container>
</template>

<script>
import { ref, onMounted } from "vue";
import axios from "axios";
import PostList from "./PostList.vue";
import CategoryTree from "./CategoryTree.vue";

export default {
  name: "BlogList",
  components: { PostList, CategoryTree },
  setup() {
    const posts = ref([]);
    const total = ref(0);
    const currentPage = ref(1);
    const pageSize = ref(6);
    const categoryNodes = ref([]);
    const currentCategoryId = ref(0);
    const expandedKeys = ref([]);
    const categoryTree = ref(null);

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

    const viewPost = (postId) => {
      this.$router.push({ name: "Post", params: { id: postId } });
    };

    return {
      posts,
      total,
      currentPage,
      currentCategoryId,
      pageSize,
      categoryNodes,
      expandedKeys,
      categoryTree,
      fetchData,
      handleNodeClick,
      handlePageChange,
      viewPost,
    };
  },
};
</script>

<style scoped>
.mb-3 {
  width: 80%;
  padding-left: 2rem;
  margin-bottom: 1rem;
}
</style>
