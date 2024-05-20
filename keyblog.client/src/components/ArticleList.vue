<template>
  <div>
    <el-card v-if="articles.length === 0" shadow="always" class="mb-3">
      <div class="card-body">没有文章</div>
    </el-card>
    <el-card v-for="article in articles" :key="article.id" shadow="always" class="mb-3">
      <template #header>
        <div class="card-header">
          <span>{{ article.categoryId }}</span>
        </div>
      </template>
      <div class="card-body">
        <h5 class="card-title">{{ article.title }}</h5>
        <p class="card-text">{{ article.summary }}</p>
        <el-button
          type="link"
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
      :current-page="currentPage"
      @current-change="handlePageChange"
    />
  </div>
</template>

<script>
import { defineComponent } from "vue";
import "@/assets/style.scss";

export default defineComponent({
  name: "ArticleList",
  props: {
    articles: {
      type: Array,
      required: true,
    },
    total: {
      type: Number,
      required: true,
    },
    currentPage: {
      type: Number,
      required: true,
    },
    pageSize: {
      type: Number,
      required: true,
    },
  },
  emits: ["page-change", "view-article"],
  setup(props, { emit }) {
    const handlePageChange = (page) => {
      emit("update:currentPage", page);
    };

    const viewArticle = (articleId) => {
      emit("view-article", articleId);
    };

    return {
      handlePageChange,
      viewArticle,
    };
  },
});
</script>
