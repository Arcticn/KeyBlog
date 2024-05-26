<template>
  <div>
    <el-card v-if="posts.length === 0" shadow="always" class="mb-3">
      <div class="card-body">没有文章</div>
    </el-card>
    <el-card v-for="post in posts" :key="post.id" shadow="always" class="mb-3">
      <!-- <template #header>
        <div class="card-header">
          <span>{{ post.categoryId }}</span>
        </div>
      </template> -->
      <div class="card-body">
        <h5 class="card-title">{{ post.title }}</h5>
        <p class="card-text">{{ post.summary }}</p>
        <el-button type="default" @click="viewPost(post.id)">
          查看全文
        </el-button>
      </div>
    </el-card>
    <el-pagination
      background
      style="justify-content: center; margin-left: 5rem;"
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
  posts: {
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
const emit = defineEmits(["view-post","update:currentPage","page-change"]);

// 处理分页改变的函数
const handlePageChange = (newPage) => {
  emit("update:currentPage", newPage);

};

// 查看文章的函数
const viewPost = (postId) => {
  emit("view-post", postId);
};
</script>
