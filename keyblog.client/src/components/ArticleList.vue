<template>
  <div>
    <el-card v-if="articles.length === 0" shadow="always" class="mb-3">
      <div class="card-body">没有文章</div>
    </el-card>
    <el-card
      v-for="article in articles"
      :key="article.id"
      shadow="always"
      class="mb-3"
    >
      <!-- <template #header>
        <div class="card-header">
          <span>{{ article.categoryId }}</span>
        </div>
      </template> -->
      <div class="card-body">
        <h5 class="card-title">{{ article.title }}</h5>
        <p class="card-text">{{ article.summary }}</p>
        <el-button
          type="default"
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

<script setup>
import "@/components/styles/style.scss";

// 定义组件的 props
const props = defineProps({
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
});

// 定义组件的 emits
const emit = defineEmits(["page-change", "view-article"]);

// 处理分页改变的函数
const handlePageChange = (page) => {
  emit("update:currentPage", page);
};

// 查看文章的函数
const viewArticle = (articleId) => {
  emit("view-article", articleId);
};
</script>
