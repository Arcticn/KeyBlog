<template>
  <el-container>
    <el-header>
      <h2>Blog</h2>
      <span>Articles</span>
    </el-header>
    <el-main>
      <el-row :gutter="20">
        <el-col :span="16">
          <el-card v-if="articles.length === 0" shadow="always" class="mb-3">
            <div class="card-body">没有文章</div>
          </el-card>
          <el-card
            v-for="article in articles"
            :key="article.id"
            shadow="always"
            class="mb-3"
          >
            <template #header>
              <div class="card-header">
                <span>{{ article.categoryId }}</span>
              </div>
            </template>
            <div class="card-body">
              <h5 class="card-title">{{ article.title }}</h5>
              <p class="card-text">{{ article.summary }}</p>
              <el-button
                type="default"
                class="btn-outline-secondary"
                @click="viewArticle(article.id)"
              >
                查看全文
              </el-button>
            </div>
          </el-card>
          <el-pagination
            background
            style="justify-content: start"
            layout="prev, pager, next"
            :total="total"
            :page-size="pageSize"
            v-model:current-page="currentPage"
            @current-change="handlePageChange"
          />
        </el-col>
        <el-col :span="8" class="mb-3" id="categoryNodes">
          <el-tree
            ref="categoryTree"
            :data="categoryNodes"
            :props="defaultProps"
            @node-click="handleNodeClick"
            node-key="id"
            highlight-current
            :default-expanded-keys="expandedKeys"
          >
          </el-tree>
        </el-col>
      </el-row>
    </el-main>
    <el-footer>Footer</el-footer>
  </el-container>
</template>

<script>
import { ref, onMounted } from "vue";
import axios from "axios";

export default {
  name: "BlogList",
  setup() {
    const articles = ref([]);
    const total = ref(0);
    const currentPage = ref(1);
    const pageSize = ref(6);
    const categoryNodes = ref([]);
    const currentCategoryId = ref(0);
    const expandedKeys = ref([]);
    const defaultProps = ref({
      children: "children",
      label: "label",
    });
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
        articles.value = data.articles.items;
        total.value = data.articles.totalCount;
        currentCategoryId.value = data.currentCategoryId;
        currentPage.value = page;
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    const toggleNodeExpansion = (node) => {
      const index = expandedKeys.value.indexOf(node.id);
      if (index > -1) {
        expandedKeys.value.splice(index, 1);
        // 递归移除所有子节点
        removeChildKeys(node.children);
      } else {
        expandedKeys.value.push(node.id);
      }
    };

    const removeChildKeys = (children) => {
      if (!children) return;
      children.forEach((child) => {
        const index = expandedKeys.value.indexOf(child.id);
        if (index > -1) {
          expandedKeys.value.splice(index, 1);
        }
        removeChildKeys(child.children);
      });
    };

    const handleNodeClick = async (node) => {
      toggleNodeExpansion(node);
      await fetchData(node.id, 1);
    };
    const handlePageChange = (page) => {
      fetchData(currentCategoryId.value, page);
    };

    onMounted(() => {
      fetchData();
    });

    const viewArticle = (articleId) => {
      this.$router.push({ name: "Article", params: { id: articleId } });
    };

    return {
      articles,
      total,
      currentPage,
      currentCategoryId,
      pageSize,
      categoryNodes,
      defaultProps,
      fetchData,
      handleNodeClick,
      handlePageChange,
      viewArticle,
      expandedKeys,
      categoryTree,
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

.card-container {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.card-title {
  font-size: 1.25rem;
  font-weight: bold;
}

.card-text {
  margin-bottom: 1rem;
}

.btn-outline-secondary {
  border: 1px solid #dcdcdc;
  color: #606266;
}

.tree-container {
  width: 300px;
  margin: 20px auto;
}
</style>
